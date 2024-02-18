using Chroma.Editor.Infrastructure.StateMachine.Util;
using System.Collections.Generic;

namespace Chroma.Editor.Infrastructure.StateMachine.GridElements
{
    public static class BoxColorData
    {
        public static Dictionary<BoxColor, BoxColorScheme> Schemes = new Dictionary<BoxColor, BoxColorScheme>
        {
            {
                BoxColor.Green,
                new BoxColorScheme(
                    ColorUtils.FromRGB(50, 113, 78),
                    ColorUtils.FromRGB(43, 93, 65)
                )
            },
            {
                BoxColor.Teal,
                new BoxColorScheme(
                    ColorUtils.FromRGB(0, 104, 104),
                    ColorUtils.FromRGB(0, 87, 87)
                )
            },
            {
                BoxColor.Cyan,
                new BoxColorScheme(
                    ColorUtils.FromRGB(47, 144, 143),
                    ColorUtils.FromRGB(46, 125, 126)
                )
            },
            {
                BoxColor.Blue,
                new BoxColorScheme(
                    ColorUtils.FromRGB(30, 73, 104),
                    ColorUtils.FromRGB(25, 62, 89)
                )
            },
            {
                BoxColor.Purple,
                new BoxColorScheme(
                    ColorUtils.FromRGB(92, 63, 113),
                    ColorUtils.FromRGB(74, 48, 92)
                )
            },
            {
                BoxColor.Burgundy,
                new BoxColorScheme(
                    ColorUtils.FromRGB(102, 40, 56),
                    ColorUtils.FromRGB(84, 35, 49)
                )
            },
            {
                BoxColor.Pink,
                new BoxColorScheme(
                    ColorUtils.FromRGB(171, 63, 95),
                    ColorUtils.FromRGB(151, 56, 84)
                )
            },
            {
                BoxColor.Red,
                new BoxColorScheme(
                    ColorUtils.FromRGB(163, 40, 28),
                    ColorUtils.FromRGB(133, 31, 21)
                )
            },
            {
                BoxColor.Orange,
                new BoxColorScheme(
                    ColorUtils.FromRGB(203, 74, 11),
                    ColorUtils.FromRGB(174, 70, 10)
                )
            },
            {
                BoxColor.Yellow,
                new BoxColorScheme(
                    ColorUtils.FromRGB(163, 156, 0),
                    ColorUtils.FromRGB(137, 131, 0)
                )
            },
            {
                BoxColor.Black,
                new BoxColorScheme(
                    ColorUtils.FromRGB(20, 20, 20),
                    ColorUtils.FromRGB(10, 10, 10)
                )
            },
            {
                BoxColor.Gray,
                new BoxColorScheme(
                    ColorUtils.FromRGB(114, 114, 114),
                    ColorUtils.FromRGB(102, 102, 102)
                )
            },
            {
                BoxColor.Brown,
                new BoxColorScheme(
                    ColorUtils.FromRGB(89, 51, 16),
                    ColorUtils.FromRGB(59, 34, 11)
                )
            },
        };
    }
}
