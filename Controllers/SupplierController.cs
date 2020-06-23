using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.BindingTargets;
namespace SportsStore.Controllers
{
    [Route("api/suppliers")]
        // [Authorize]

    public class SupplierController : Controller
    {
        private StoreAppContext context;
        public SupplierController(StoreAppContext ctx)
        {
            context = ctx;
        }
        [HttpGet]
        public IEnumerable<Supplier> GetSuppliers()
        {
            return context.Suppliers;
        }
        [HttpPost]
        public IActionResult CreateSupplier([FromBody]SupplierData sdata)
        {
            if (ModelState.IsValid)
            {
                Supplier s = sdata.Supplier;
                context.Add(s);
                context.SaveChanges();
                return Ok(s.SupplierId);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteSupplier(long id)
        {
            context.Remove(new Supplier { SupplierId = id });
            context.SaveChanges();
            return Ok(id);
        }

        [HttpPut("{id}")]
        public IActionResult ReplaceSupplier(long id,
                  [FromBody] SupplierData sdata)
        {
            if (ModelState.IsValid)
            {
                Supplier s = sdata.Supplier;
                s.SupplierId = id;
                context.Update(s);
                context.SaveChanges();
                return Ok(s);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
