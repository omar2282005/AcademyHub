using AcademyHub.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AcademyHub.Data
{
    public class IdentityAppDbContext :IdentityDbContext<ApplicationUser>
    {
        public IdentityAppDbContext(
       DbContextOptions<IdentityAppDbContext> options)
       : base(options)
        {
        }
    }
}
