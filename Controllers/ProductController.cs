using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using SportsStore.Models.BindingTargets;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;

namespace SportsStore.Models
{
    // set route attribute to make request as 'api/Product'
    [Route("api/[controller]")]
    // [Authorize]
    public class ProductController : Controller
    {
        private readonly StoreAppContext _context;
        // initiate database context
        public ProductController(StoreAppContext context)
        {
            _context = context;
        }

    //    [HttpGet("{id}")]
    //      public Product GetProduct(long id)
    //      {
    //          return _context.Products.Include(p => p.Supplier)
    //          .Include(p => p.Ratings)
    //          .FirstOrDefault(p => p.ProductId == id);
    //      }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public Product GetProduct(long id)
        {
            IQueryable<Product> query = _context.Products.Include(p => p.Ratings);

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                query = query.Include(p => p.Supplier)
                .ThenInclude(s => s.Products);
            }
            Product result = query.First(p => p.ProductId == id);
            if (result != null)
            {
                if (result.Supplier != null)
                {
                    result.Supplier.Products = result.Supplier.Products.Select(p =>
                    new Product
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        Category = p.Category,
                        Description = p.Description,
                        Price = p.Price,
                        image=p.image
                    });
                }
                if (result.Ratings != null)
                {
                    foreach (Rating r in result.Ratings)
                    {
                        r.Product = null;
                    }
                }
            }
            return result;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetProducts(string category, string search, bool related = false, bool metadata = false)
        {
            IQueryable<Product> query = _context.Products;

            if (!string.IsNullOrWhiteSpace(category))
            {
                string catLower = category.ToLower();
                query = query.Where(p => p.Category.ToLower().Contains(catLower));
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                string searchLower = search.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(searchLower)
                    || p.Description.ToLower().Contains(searchLower));
            }

            if (related )
            {
                query = query.Include(p => p.Supplier).Include(p => p.Ratings);
                List<Product> data = query.ToList();
                data.ForEach(p =>
                {
                    if (p.Supplier != null)
                    {
                        p.Supplier.Products = null;
                    }
                    if (p.Ratings != null)
                    {
                        p.Ratings.ForEach(r => r.Product = null);
                    }
                });
                return metadata ? CreateMetadata(data) : Ok(data);
            }
            else
            {
                return metadata ? CreateMetadata(query) : Ok(query);
            }
        }

        private IActionResult CreateMetadata(IEnumerable<Product> products)
        {
            return Ok(new
            {
                data = products,
                categories = _context.Products.Select(p => p.Category)
                    .Distinct().OrderBy(n => n)
            });
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductData pdata)
        {
            if (ModelState.IsValid)
            {
                Product p = pdata.Getproduct();
                if (p.Supplier != null && p.Supplier.SupplierId != 0)
                {
                    _context.Attach(p.Supplier);
                }
                _context.Add(p);
                _context.SaveChanges();
                return Ok(p.ProductId);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(long id)
        {
            _context.Products.Remove(new Product { ProductId = id });
            _context.SaveChanges();
            return Ok(id);
        }
        [HttpPut("{id}")]
        public IActionResult ReplaceProduct(long id, [FromBody] ProductData pdata)
        {
            if (ModelState.IsValid)
            {
                Product p = pdata.Getproduct();
                p.ProductId = id;
                if (p.Supplier != null && p.Supplier.SupplierId != 0)
                {
                    _context.Attach(p.Supplier);
                }
                _context.Update(p);
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
