using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public class IdentitDbContext:IdentityDbContext<AppUser>
    {
        public IdentitDbContext(DbContextOptions<IdentitDbContext> options):base(options)
        {

        }
    }
}
