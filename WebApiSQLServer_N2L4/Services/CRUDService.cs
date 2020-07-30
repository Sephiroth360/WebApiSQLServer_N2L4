using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSQLServer_N2L4.Entities;

namespace WebApiSQLServer_N2L4.Services
{
    public class CRUDService
    {
        public async Task<IEnumerable<Products>> CreateProducts(NorthwindDbContext context, Products products)
        {
            var productos = await context.Products.FromSqlRaw("InsertarProductos @NombreProducto, @PrecioUnidad, @Cantidad",
                                                               new SqlParameter("@NombreProducto", products.ProductName),
                                                               new SqlParameter("@PrecioUnidad", products.UnitPrice),
                                                               new SqlParameter("@Cantidad", products.UnitsInStock)
                                                              ).ToListAsync();

            return productos;
        }

        public async Task<IEnumerable<Products>> ReadProducts(NorthwindDbContext context)
        {
            var productos = await context.Products.FromSqlRaw("LeerProductos").ToListAsync();

            return productos;
        }

        public async Task<IEnumerable<Products>> DeleteProduct(NorthwindDbContext context, int id)
        {
            var productos = await context.Products.FromSqlRaw("EliminarProductos @Id",
                                                new SqlParameter("@Id", id)
                                               ).ToListAsync();

            return productos;
        }
        public async Task<IEnumerable<Products>> ReadProductById(NorthwindDbContext context, int id)
        {
            var productos = await context.Products.FromSqlRaw("LeerProductosId @Id",
                                                    new SqlParameter("@Id", id)
                                                   ).ToListAsync();

            return productos;
        }
        public async Task<IEnumerable<Products>> FilterProducts(NorthwindDbContext context, int value)
        {
            var productos = await context.Products.FromSqlRaw("FiltrarProductos_Precio @FilterPrice",
                                                                new SqlParameter("@FilterPrice", value)).ToListAsync();

            return productos;
        }
    }

}
