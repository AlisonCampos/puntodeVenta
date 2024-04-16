using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace puntodeventa.Models
{
    public class ItemCarrito
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal => Precio * (decimal)Cantidad;
    }
}