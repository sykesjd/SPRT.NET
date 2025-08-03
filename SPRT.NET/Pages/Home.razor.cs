using ClipLazor.Components;
using Microsoft.AspNetCore.Components;
using SPRT.NET.Models;

namespace SPRT.NET.Pages;

public partial class Home
{
    [Inject]
    private IClipLazor Clipboard { get; set; }
    [Inject]
    private TimingData TimingData { get; set; }

    async void PasteFromDiag()
    {
        TimingData.FromDiag = await Clipboard.ReadTextAsync();
        StateHasChanged();
    }

    async void PasteToDiag()
    {
        TimingData.ToDiag = await Clipboard.ReadTextAsync();
        StateHasChanged();
    }

    async void CopyModNote()
    {
        await Clipboard.WriteTextAsync(TimingData.ModNote.AsMemory());
    }
}
