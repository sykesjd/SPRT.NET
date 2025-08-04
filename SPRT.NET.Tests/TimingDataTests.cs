using System.Text.Json;
using SPRT.NET.Models;

namespace SPRT.NET.Tests;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1045:Avoid using TheoryData type arguments that might not be serializable", Justification = "Who cares?")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "It _is_ necessary...")]
public class TimingDataTests
{
    public static readonly TheoryData<TimingData, float?> TotalTimeData = new()
    {
        {
            new TimingData
            {
                Framerate = 60,
                FromDiag = JsonSerializer.Serialize(new
                {
                    Cmt = "0.501"
                }),
                ToDiag = JsonSerializer.Serialize(new
                {
                    Cmt = "1.754"
                })
            },
            1.250f
        },
        {
            new TimingData
            {
                Framerate = 60,
                FromDiag = JsonSerializer.Serialize(new
                {
                    Cmt = "0.501"
                }),
                ToDiag = JsonSerializer.Serialize(new
                {
                    Cmt = "1.754"
                }),
                Modifier = (decimal)4.5
            },
            5.750f
        },
        {
            new TimingData
            {
                Framerate = 60,
                FromDiag = JsonSerializer.Serialize(new
                {
                    Cmt = "0.501"
                }),
                ToDiag = JsonSerializer.Serialize(new
                {
                    Cmt = "wtf"
                })
            },
            null
        },
        {
            new TimingData
            {
                Framerate = 60,
                FromDiag = "kekw",
                ToDiag = JsonSerializer.Serialize(new
                {
                    Cmt = "1.754"
                })
            },
            null
        },
        {
            new TimingData
            {
                Framerate = 60,
                FromDiag = JsonSerializer.Serialize(new
                {
                    Cmt = "0.501"
                })
            },
            null
        },
        {
            new TimingData
            {
                Framerate = 0,
                FromDiag = JsonSerializer.Serialize(new
                {
                    Cmt = "0.501"
                }),
                ToDiag = JsonSerializer.Serialize(new
                {
                    Cmt = "1.754"
                })
            },
            null
        },
        {
            new TimingData
            {
                FromDiag = JsonSerializer.Serialize(new
                {
                    Cmt = "0.501"
                }),
                ToDiag = JsonSerializer.Serialize(new
                {
                    Cmt = "1.754"
                })
            },
            null
        }
    };

    [Theory]
    [MemberData(nameof(TotalTimeData))]
    public void TotalTime_CalculatesCorrectTime(TimingData sut, float? expectedTime)
    {
        Assert.Equal(expectedTime, (float?)sut.TotalTime);
    }

    public static readonly TheoryData<TimingData, string> DisplayTimeData = new()
    {
        {
            new TimingData
            {
                Framerate = 60,
                FromDiag = JsonSerializer.Serialize(new
                {
                    Cmt = "0.501"
                }),
                ToDiag = JsonSerializer.Serialize(new
                {
                    Cmt = "3673.502"
                })
            },
            "1:01:13"
        },
        {
            new TimingData
            {
                Framerate = 60,
                FromDiag = JsonSerializer.Serialize(new
                {
                    Cmt = "0.501"
                }),
                ToDiag = JsonSerializer.Serialize(new
                {
                    Cmt = "429.502"
                })
            },
            "7:09"
        },
        {
            new TimingData
            {
                Framerate = 60,
                FromDiag = JsonSerializer.Serialize(new
                {
                    Cmt = "0.501"
                }),
                ToDiag = JsonSerializer.Serialize(new
                {
                    Cmt = "10.252"
                })
            },
            "9.750"
        },
        {
            new TimingData
            {
                FromDiag = JsonSerializer.Serialize(new
                {
                    Cmt = "0.501"
                }),
                ToDiag = JsonSerializer.Serialize(new
                {
                    Cmt = "10.252"
                })
            },
            null
        }
    };

    [Theory]
    [MemberData(nameof(DisplayTimeData))]
    public void FinalTimeDisplay_DisplaysFormattedTime(TimingData sut, string expectedTimeDisplay)
    {
        Assert.Equal(expectedTimeDisplay, sut.FinalTimeDisplay);
    }

    public static readonly TheoryData<TimingData, string> ModNoteData = new()
    {
        {
            new TimingData
            {
                Framerate = 60,
                FromDiag = JsonSerializer.Serialize(new
                {
                    Cmt = "0.501"
                }),
                ToDiag = JsonSerializer.Serialize(new
                {
                    Cmt = "10.252"
                })
            },
            $"Mod note: Time starts at 0.500 and ends at 10.250, at 60 fps. {Environment.NewLine}Retimed using SPRT.NET"
        },
        {
            new TimingData
            {
                FromDiag = JsonSerializer.Serialize(new
                {
                    Cmt = "0.501"
                }),
                ToDiag = JsonSerializer.Serialize(new
                {
                    Cmt = "10.252"
                })
            },
            null
        }
    };

    [Theory]
    [MemberData(nameof(ModNoteData))]
    public void ModNote_BuildsFullModNote(TimingData sut, string expectedNote)
    {
        Assert.Equal(expectedNote, sut.ModNote);
    }
}
