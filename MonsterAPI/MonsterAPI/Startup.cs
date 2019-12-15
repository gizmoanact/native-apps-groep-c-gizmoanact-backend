using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NSwag.SwaggerGeneration.Processors.Security;
using NSwag;
using System.Security.Claims;
using MonsterAPI.Data;
using MonsterAPI.Model.Interface;
using MonsterAPI.Data.Repositories;

namespace MonsterAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            #region MVC
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            #endregion

            #region Configure DB-Context
            services.AddDbContext<ApplicationDBContext>(options =>
            {
                //mac connectionstring
                //options.UseSqlServer(Configuration.GetConnectionString("MacConnection"));
                //Docker connectionstring
                //options.UseSqlServer(Configuration.GetConnectionString("DockerConnection"));
                //Windows connectionstring
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            #endregion

            #region Api configuration
            //Used for swagger and documentation
            services.AddOpenApiDocument(o =>
            {
                o.Title = "MusicMonster API";
                o.Version = "alpha";
                o.DocumentName = "MusicMonster";
                o.Description = "The api powering an Angular and Android application";
                //o.DocumentProcessors.Add(new SecurityDefinitionAppender("JWT Token", new SwaggerSecurityScheme
                //{
                //    Type = SwaggerSecuritySchemeType.ApiKey,
                //    Name = "Authorization",
                //    In = SwaggerSecurityApiKeyLocation.Header,
                //    Description = "Copy 'Bearer' + valid JWT token into field"
                //}));
                //o.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT Token"));
            });
            #endregion

            #region Identity + Configuration
            services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDBContext>();

   
  

            #region Cors
        
            services.AddCors(options =>
                options.AddPolicy("AllowAllOrigins", builder =>
                    builder.AllowAnyOrigin()
                )
            );
            #endregion

            #region Dependency Injection
            services
                .AddScoped<IAlbumRepository, AlbumRepository>()
                .AddScoped<IArtistRepository, ArtistRepository>()
                .AddScoped<ILiedjeRepository, LiedjeRepository>();

            #endregion

       
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDBContext context
           )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Make sure database is created and model creating is called.
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            //Seed Data
            new DataInit(context).SeedData();

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseMvc();

            app.UseSwaggerUi3();
            app.UseSwagger();

            app.UseCors("AllowAllOrigins");
        }
    }
}
#endregion