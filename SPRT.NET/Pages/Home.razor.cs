using System.Text.Json;
using ClipLazor.Components;
using Microsoft.AspNetCore.Components;

namespace SPRT.NET.Pages;

public partial class Home
{
    [Inject]
    private IClipLazor Clipboard { get; set; }

    private int? Framerate { get; set; }
    private decimal? Modifier { get; set; }
    private string FromDiag { get; set; }
    private decimal? FromTime => GetTime(FromDiag);
    private string ToDiag { get; set; }
    private decimal? ToTime => GetTime(ToDiag);
    private decimal? TotalTime => GetTotalTime();
    private string FinalTimeDisplay => GetTimeDisplay(TotalTime);
    private string ModNote => GetModNote();

    private bool TryGetFrametime(out decimal frametime)
    {
        frametime = 0;

        if (!Framerate.HasValue)
            return false;

        if (Framerate.Value == 0)
            return false;

        frametime = (decimal)1.0 / Framerate.Value;
        return true;
    }

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private decimal? GetTime(string diag)
    {
        if (string.IsNullOrEmpty(diag))
            return null;

        if (!TryGetFrametime(out var frametime))
            return null;

        try
        {
            var diagObj = JsonSerializer.Deserialize<YTDiag>(diag, _jsonOptions);
            if (!decimal.TryParse(diagObj.Cmt, out var time))
                return null;

            return time - (time % frametime);
        }
        catch
        {
            return null;
        }
    }

    private class YTDiag
    {
        public string Cmt { get; set; }
    }

    private decimal? GetTotalTime()
    {
        if (!FromTime.HasValue)
            return null;

        if (!ToTime.HasValue)
            return null;

        var totalTime = ToTime.Value - FromTime.Value;
        if (Modifier.HasValue)
            totalTime += Modifier.Value;

        return totalTime;
    }

    private static string GetTimeDisplay(decimal? time)
    {
        if (!time.HasValue)
            return null;

        var displayTime = Math.Round(time.Value, 3, MidpointRounding.AwayFromZero);
        var ms = (int)((1000 * displayTime) % 1000);
        var totalS = (int)(displayTime - displayTime % 1);
        var s = totalS % 60;
        var totalM = (totalS - s) / 60;
        var m = totalM % 60;
        var h = (totalM - m) / 60;

        var displayStr = h != 0 ? $"{h}:{m:d2}:{s:d2}"
            : m != 0 ? $"{m}:{s:d2}" : $"{s}";
        if (ms != 0)
            displayStr = $"{displayStr}.{ms:d3}";

        return displayStr;
    }

    private string GetModNote()
    {
        if (!TotalTime.HasValue)
            return null;

        return $"Mod note: Time starts at {GetTimeDisplay(FromTime)} and ends at {GetTimeDisplay(ToTime)}, at {Framerate.Value} fps. {Environment.NewLine}Retimed using SPRT.NET";
    }

    async void PasteFromDiag()
    {
        FromDiag = await Clipboard.ReadTextAsync();
        StateHasChanged();
    }

    async void PasteToDiag()
    {
        ToDiag = await Clipboard.ReadTextAsync();
        StateHasChanged();
    }

    async void CopyModNote()
    {
        await Clipboard.WriteTextAsync(ModNote.AsMemory());
    }
}
