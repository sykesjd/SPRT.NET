using ClipLazor.Extention;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SPRT.NET;
using SPRT.NET.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddClipboard();
builder.Services.AddSingleton<TimingData>();

await builder.Build().RunAsync();
