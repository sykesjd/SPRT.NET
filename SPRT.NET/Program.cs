using System.Diagnostics.CodeAnalysis;
using ClipLazor.Extention;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SPRT.NET;
using SPRT.NET.Models;

[ExcludeFromCodeCoverage]
internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
        builder.Services.AddClipboard();
        builder.Services.AddSingleton<TimingData>();

        await builder.Build().RunAsync();
    }
}