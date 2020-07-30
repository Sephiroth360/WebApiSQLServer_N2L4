using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApiSQLServer_N2L4.Entities;
using WebApiSQLServer_N2L4.Models;
using WebApiSQLServer_N2L4.Services;

namespace WebApiSQLServer_N2L4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors("PermitirApiRequest")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
    public class ProductsController : ControllerBase
    {
        private readonly NorthwindDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IDataProtector _protector;
        private readonly BuildHashService _hash;

        public ProductsController(NorthwindDbContext context, IConfiguration configuration, IDataProtectionProvider protector, BuildHashService hash)
        {
            _context = context;
            _configuration = configuration;
            _protector = protector.CreateProtector(_configuration["ProtectionKey"]);

            _hash = hash;
        }

        // GET: api/Products
        [HttpGet]
        public ActionResult<IEnumerable<Products>> GetProducts()
        {
            return _context.Products.ToList();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> GetProducts(int id)
        {
            var products = await _context.Products.FindAsync(id);

            if (products == null)
            {
                return NotFound();
            }

            return products;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducts(int id, Products products)
        {
            if (id != products.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(products).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Products>> PostProducts(Products products)
        {
            _context.Products.Add(products);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProducts", new { id = products.ProductId }, products);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Products>> DeleteProducts(int id)
        {
            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }

            _context.Products.Remove(products);
            await _context.SaveChangesAsync();

            return products;
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        [HttpPost("Encriptar")]
        public ActionResult Encriptar([FromBody] string value)
        {
            var textoCifrado = _protector.Protect(value);
            var textoDesencriptado = _protector.Unprotect(textoCifrado);

            return Ok(new { value, textoCifrado, textoDesencriptado });
        }

        [HttpPost("Hash")]
        public ActionResult<HashResult> HashPassword(HashResult password)
        {
            return _hash.BuildHash(password.Input);
        }

        [HttpPost("VerifyHash")]
        public ActionResult<bool> VerifyHash(HashResult password)
        {
            return _hash.VerifyHash(password);
        }
    }
}
