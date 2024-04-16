using System.Web.Mvc;
using System.Linq;
using puntodeventa.Models; // Asegúrate de incluir tus modelos aquí
using System;
using WebGrease;
using System.Data.Entity;
using System.Collections.Generic;

namespace puntodeventa.Controllers
{
     public class VentasController : Controller
    {
        private PuntoVentaContext _context = new PuntoVentaContext();

        public ActionResult Crear()
        {
            if (Session["UserId"] != null && Convert.ToInt32(Session["UserType"]) == 1)
            {
                ViewBag.Categorias = new SelectList(_context.ProCatCategorias.ToList(), "id", "nombre");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public JsonResult GetSubcategorias(int idProCatCategoria)
        {
            var subcategorias = _context.ProCatSubcategorias
                .Where(s => s.id == idProCatCategoria)  // Asegúrate de que el nombre de la columna de clave foránea es correcto
                .Select(s => new { s.id, s.nombre })
                .ToList();
            return Json(subcategorias, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductos(int idProCatSubcategoria)
        {
            var productos = _context.ProProductos
                .Where(p => p.idProCatSubcategoria == idProCatSubcategoria)
                .Select(p => new { p.id, p.nombreProducto })
                .ToList();
            return Json(productos, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetallesProducto(int idProProducto)
        {
            var producto = _context.ProProductos
                .Where(p => p.id == idProProducto)
                .Select(p => new { p.precio, p.stock })
                .FirstOrDefault();

            if (producto == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            return Json(producto, JsonRequestBehavior.AllowGet);
        }

        private string GenerarFolio()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        [HttpPost]
        public ActionResult AgregarAlCarrito(int productoId, decimal cantidad)
        {
            var producto = _context.ProProductos.FirstOrDefault(p => p.id == productoId);
            if (producto == null)
            {
                return Json(new { success = false, message = "Producto no encontrado." });
            }

            List<ItemCarrito> carrito = Session["Carrito"] as List<ItemCarrito>;
            if (carrito == null)
            {
                carrito = new List<ItemCarrito>();
            }

            var itemExistente = carrito.FirstOrDefault(i => i.ProductoId == productoId);
            if (itemExistente != null)
            {
                itemExistente.Cantidad += cantidad;
            }
            else
            {
                carrito.Add(new ItemCarrito { ProductoId = productoId, NombreProducto = producto.nombreProducto, Precio = producto.precio, Cantidad = cantidad });
            }

            Session["Carrito"] = carrito;
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult FinalizarVenta(string folio, List<ProductoVenta> productos)
        {
            if (productos == null || !productos.Any())
            {
                return Json(new { success = false, message = "No hay productos para procesar la venta." });
            }

            if (Session["UserID"] == null)
            {
                return Json(new { success = false, message = "Usuario no autenticado." });
            }

            int userId = Convert.ToInt32(Session["UserID"]);

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var nuevaVenta = new VenVenta
                    {
                        idUsuUsuario = userId,
                        folio = folio,
                        fechaVenta = DateTime.Now,
                        idVenCatEstado = 1 // Asume 1 es "En proceso"
                    };

                    _context.VenVenta.Add(nuevaVenta);
                    _context.SaveChanges();

                    foreach (var item in productos)
                    {
                        var producto = _context.ProProductos.FirstOrDefault(p => p.nombreProducto == item.Nombre);
                        if (producto == null || producto.stock < item.Cantidad)
                        {
                            transaction.Rollback();
                            return Json(new { success = false, message = $"Stock insuficiente o producto no encontrado: {item.Nombre}" });
                        }

                        producto.stock -= item.Cantidad;
                        _context.Entry(producto).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();

                        var ventaProducto = new VenVentaProducto
                        {
                            idVenVenta = nuevaVenta.id,
                            idProProducto = producto.id,
                            cantidad = item.Cantidad,
                            total = item.Total
                        };

                        _context.VenVentaProducto.Add(ventaProducto);
                    }

                    _context.SaveChanges();

                    nuevaVenta.idVenCatEstado = 2; // Asume 2 es "Completada"
                    _context.Entry(nuevaVenta).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    transaction.Commit();
                    return Json(new { success = true, message = "Venta completada con éxito." });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    System.Diagnostics.Debug.WriteLine("Error en FinalizarVenta: " + ex.ToString());
                    LogException(ex);
                    return Json(new { success = false, message = "Error al finalizar la venta." });
                }

            }
        }

        private void LogException(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception: " + ex.ToString()); // Imprime toda la información de la excepción, incluyendo la excepción interna
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                System.Diagnostics.Debug.WriteLine("Inner Exception: " + ex.Message);
            }
        }

    }
}