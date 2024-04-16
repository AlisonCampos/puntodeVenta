using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace puntodeventa.Models
{
    public class VentaModel
    {
        public string Folio { get; set; }
        public int UserId { get; set; }
        public List<ProductoVenta> Productos { get; set; }
    }

  /*  public class ProductoVenta
    {
        public int IdProProducto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Total { get; set; }
        public decimal Stock { get; set; }  // Asegúrate que esto es lo que deseas recibir
    }*/


}