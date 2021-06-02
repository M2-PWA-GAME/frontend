using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Blazored.LocalStorage;

using frontend.Service.Declaration;
using frontend.Service.Implementation;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace frontend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddBlazoredLocalStorage();

            // builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://medieval-warfare.herokuapp.com/") });
            // builder.Services.AddSingleton<IUserService, UserService>(provider => new UserService((ILocalStorageService)provider.GetService(typeof(ILocalStorageService))));
            builder.Services.AddSingleton<IUserService, UserService>();

            await builder.Build().RunAsync();
        }
    }
}
