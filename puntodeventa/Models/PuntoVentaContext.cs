using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace puntodeventa.Models
{
    public class PuntoVentaContext : DbContext
    {
        public PuntoVentaContext() : base("PuntoVentaDatabase")
        {
        }

        public DbSet<UsuUsuario> UsuUsuarios { get; set; }
        public DbSet<UsuCatEstado> UsuCatEstado { get; set; }
        public DbSet<UsuCatTipoUsuario> UsuCatTipoUsuarios { get; set; }
        public DbSet<ProCatCategoria> ProCatCategorias { get; set; }
        public DbSet<ProCatSubcategoria> ProCatSubcategorias { get; set; }
        public DbSet<ProProducto> ProProductos { get; set; }
        public DbSet<VenCatEstado> VenCatEstado { get; set; }
        public DbSet<VenVenta> VenVenta { get; set; }
        public DbSet<VenVentaProducto> VenVentaProducto { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configura el nombre correcto de la tabla para VenVenta
            modelBuilder.Entity<VenVenta>().ToTable("VenVenta");  // Asegúrate de que coincida con el nombre en la base de datos

            // Configuración de las relaciones para UsuUsuario
            modelBuilder.Entity<UsuUsuario>()
                .HasRequired(u => u.Estado)
                .WithMany()
                .HasForeignKey(u => u.usuCatEstado);

            modelBuilder.Entity<UsuUsuario>()
                .HasRequired(u => u.TipoUsuario)
                .WithMany()
                .HasForeignKey(u => u.usuCatTipoUsuario);

            // Configuración de las relaciones para ProProducto
            modelBuilder.Entity<ProProducto>()
                .HasRequired(p => p.ProCatCategoria)
                .WithMany(c => c.ProProductos)
                .HasForeignKey(p => p.idProCatCategoria);

            modelBuilder.Entity<ProProducto>()
                .HasRequired(p => p.ProCatSubcategoria)
                .WithMany(s => s.ProProductos)
                .HasForeignKey(p => p.idProCatSubcategoria);

            // Configuración de las relaciones para VenVentaProducto
            modelBuilder.Entity<VenVentaProducto>()
                .HasRequired(vp => vp.VenVenta)
                .WithMany(v => v.VenVentaProductos)
                .HasForeignKey(vp => vp.idVenVenta);

            modelBuilder.Entity<VenVenta>()
            .ToTable("VenVenta")  // Asegura que el nombre de la tabla sea el correcto
            .HasKey(v => v.id)  // Define la clave primaria
            .HasRequired(v => v.VenCatEstado)  // Establece la relación obligatoria con VenCatEstado
            .WithMany()  // No se especifica la propiedad de navegación inversa
            .HasForeignKey(v => v.idVenCatEstado)  // Usa el nombre correcto de la columna FK en la base de datos
            .WillCascadeOnDelete(false);  // Opcional: Desactiva el borrado en cascada si es necesario

            // Añade configuraciones adicionales según sea necesario
        }

    }
}