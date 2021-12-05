namespace SofTrustTask
{
    public class StartUp
    {

        public void ConfigureServices(IServiceCollection services) => services.AddRazorPages();

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            // Устанавливаем возможность маршрутизации
            app.UseRouting();

            // Устанавливаем возможность переадрессации 
            app.UseHttpsRedirection();

            // Делаем статические файлы видимыми
            app.UseStaticFiles();

            // Делаем возможным использование Razor страниц
            app.UseEndpoints(endpoints => endpoints.MapRazorPages());


            
        }
    }
}
