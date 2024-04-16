using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace puntodeventa.Models
{
    

    public class ProductoVenta
    {
        public string Nombre { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Total { get; set; }
    }

}