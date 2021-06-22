using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using frontend.Service.Declaration;
using frontend.Service.Implementation;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace frontend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            // builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://medieval-warfare.herokuapp.com/") });
            // builder.Services.AddSingleton<IUserService, UserService>(provider => new UserService((ILocalStorageService)provider.GetService(typeof(ILocalStorageService))));

            UserService userService = null;
            builder.Services.AddSingleton<IUserService, UserService>(provider => { userService = new UserService(provider.GetService<IJSRuntime>());
                    return userService;
                });
            builder.Services.AddSingleton<IGameService, GameService>(provider => new GameService(userService));

            await builder.Build().RunAsync();
        }
    }
}
