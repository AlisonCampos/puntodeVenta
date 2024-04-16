using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace puntodeventa.Models
{
    public class ProductoVenta
    {
        public decimal Stock { get; set; }
        public int IdProProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
    }
}