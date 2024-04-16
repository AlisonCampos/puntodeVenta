﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace puntodeventa.Models
{
    [Table("ProCatSubcategoria")]
    public class ProCatSubcategoria
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }

        // Relación con ProProducto
        public virtual ICollection<ProProducto> ProProductos { get; set; }
    }
}