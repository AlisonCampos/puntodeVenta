using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace puntodeventa.Models
{
    [Table("ProProducto")]
    public class ProProducto
    {
        public int id { get; set; }
        public string nombreProducto { get; set; }
        public string descripcion { get; set; }
        public int idProCatCategoria { get; set; }
        public int idProCatSubcategoria { get; set; }
        public decimal maximo { get; set; }
        public decimal minimo { get; set; }
        public decimal stock { get; set; }
        public decimal costo { get; set; }
        public decimal precio { get; set; }
        public byte[] image { get; set; }

        // Relación con ProCatCategoria
        public virtual ProCatCategoria ProCatCategoria { get; set; }

        // Relación con ProCatSubcategoria
        public virtual ProCatSubcategoria ProCatSubcategoria { get; set; }
    }
}