using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using WebApiSQLServer_N2L4.Data;
using WebApiSQLServer_N2L4.Entities;
using WebApiSQLServer_N2L4.Helpers;
using WebApiSQLServer_N2L4.Models;
using WebApiSQLServer_N2L4.Services;

namespace WebApiSQLServer_N2L4
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => {

                options.AddPolicy("PermitirApiRequest", builder => builder.WithOrigins("*").WithMethods("*").WithHeaders("*"));

            });

            services.AddScoped<BuildHashService>();

            services.AddDataProtection();

            services.AddTransient<BuildTokenService>();

            services.AddScoped<CRUDService>();

            services.AddControllers(options => 
            {
                options.Filters.Add(typeof(FiltroErrores));
            });

            services.AddDbContext<NorthwindDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("NorthwindDb")));

            services.AddDbContext<LoginDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("LoginDb")));

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<LoginDbContext>().AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"])),
                ClockSkew = TimeSpan.Zero
            });
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var options = new RewriteOptions().AddRedirectToHttpsPermanent();

            app.UseRewriter(options);

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            //app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
