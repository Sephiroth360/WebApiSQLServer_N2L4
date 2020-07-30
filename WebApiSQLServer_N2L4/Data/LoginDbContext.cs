using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSQLServer_N2L4.Models;

namespace WebApiSQLServer_N2L4.Data
{
    public class LoginDbContext : IdentityDbContext<ApplicationUser>
    {
        public LoginDbContext(DbContextOptions<LoginDbContext> options) : base(options)
        {

        }
    }
}
