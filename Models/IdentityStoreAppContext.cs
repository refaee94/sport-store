using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models
{
    public class IdentityStoreAppContext : IdentityDbContext<IdentityUser>
    {
        public IdentityStoreAppContext(DbContextOptions<IdentityStoreAppContext> options) : base(options)
        {
        }

        protected IdentityStoreAppContext()
        {
        }
    }
}
