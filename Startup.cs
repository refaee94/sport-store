using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using SportsStore.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SportsStore.Controllers;

namespace SportsStore
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

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader().AllowCredentials();
        }));

      services.BuildServiceProvider(true);

      services.AddDbContext<IdentityStoreAppContext>(opts =>
        {
          opts.UseSqlServer(Configuration.GetConnectionString("SportsStoreIdentityDb"));
        });

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });


            services.AddDbContext<StoreAppContext>(options => options.UseSqlServer(
                   Configuration.GetConnectionString("SprotStoreDb")));
            services.AddMvc().AddJsonOptions(opts => opts.SerializerSettings.ReferenceLoopHandling
               = ReferenceLoopHandling.Serialize);
            services.AddMvc().AddJsonOptions(opts =>
            {
                opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                opts.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
      //var customCookieAuthenticationEvents = new CustomCookieAuthenticationEvents();

      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
    //   services.AddDistributedSqlServerCache(options =>
    //  {
    //    options.ConnectionString =
    //     Configuration["ConnectionStrings:SprotStoreDb"];
    //    options.SchemaName = "dbo";
    //    options.TableName = "SessionData";


    //  });
     services.AddDistributedMemoryCache();//To Store session in Memory, This is default implementation of IDistributedCache

      services.AddSession(options =>
      {        options.Cookie.IsEssential = true;

        options.Cookie.Name = "SessionData";
        options.IdleTimeout = System.TimeSpan.FromHours(48);
        options.Cookie.HttpOnly = false;

      });
      services.Configure<CookiePolicyOptions>(options =>
{
  // This lambda determines whether user consent for non-essential cookies is needed for a given request.
  options.CheckConsentNeeded = context => true;
          options.MinimumSameSitePolicy = SameSiteMode.None;

});

 services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
          //options.Events = customCookieAuthenticationEvents;
        });

      //services.AddScoped<CustomCookieAuthenticationEvents>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [System.Obsolete]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, StoreAppContext storeAppContext)
        {
             if (env.IsDevelopment())
             {
                 app.UseDeveloperExceptionPage();
             }
             else
             {
                 app.UseHsts();
             }
            app.UseDeveloperExceptionPage();
      /*app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
      {
          HotModuleReplacement = true
      });*/
              app.UseCors("MyPolicy");
app.UseCookiePolicy();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
      storeAppContext.CreateSeedData();
      app.UseIdentity();

      // IdentitySeedData.SeedDataBaseAsync(app);
    }
    }
}
