using MagicVilla_API.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Villa> Villas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Villa>().HasData(
                    new Villa { 
                        Id = 1,
                        Nombre = "Villa Real",
                        Detalle = "Detalle de la Villa..",
                        ImagenUrl = "",
                        Ocupantes = 5,
                        MetrosCuadrados = 50,
                        Tarifa = 200,
                        Amenidad = "",
                        FechaCreacion = DateTime.Now
                    },
                      new Villa
                      {
                          Id = 2,
                          Nombre = "Villa Vista a la Piscina",
                          Detalle = "Detalle de la Villa..",
                          ImagenUrl = "",
                          Ocupantes = 4,
                          MetrosCuadrados = 40,
                          Tarifa = 150,
                          Amenidad = "",
                          FechaCreacion = DateTime.Now
                      }
                );

        }
    }
}
