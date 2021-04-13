using Blazor.Realm;
using Blazor.Realm.Async.Extensions;
using Blazor.Realm.Extensions;
using JASBlazor.Data;
using JASBlazor.Redux;
using JASBlazor.ViewModels;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Radzen;

namespace JASBlazor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTelerikBlazor();
            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<TooltipService>();
            services.AddScoped<ContextMenuService>();

            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));
            services.AddServerSideBlazor(o => o.DetailedErrors = true).AddMicrosoftIdentityConsentHandler();
            services.AddControllersWithViews()
                .AddMicrosoftIdentityUI();


            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizePage("/datagrid", "/MicrosoftIdentity/Account/SignIn");
                //options.Conventions.AuthorizeFolder("/Private");
                //options.Conventions.AllowAnonymousToPage("/Private/PublicPage");
                //options.Conventions.AllowAnonymousToFolder("/Private/PublicPages");
            });


            services.AddDBContextFactory<JASDBContext>(
                options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped
                    //UseOracle(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped
                    //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                    );



            services.AddTransient<IToDoViewModel, ToDoBasicViewModel>();
            services.AddSingleton<IDataGridViewModel, DataGridViewModel>();
            //services.AddScoped<IToDoViewModel, ToDoFinalViewModel>();

            // Now Wire Up our Store
            services.AddRealmStore(new AppState(), Reducers.RootReducer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IStoreBuilder<AppState> RealmStoreBuilder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production
                // scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            RealmStoreBuilder.UseRealmAsync<AppState>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}