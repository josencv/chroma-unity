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
                    ColorUtil.FromRGB(50, 113, 78),
                    ColorUtil.FromRGB(43, 93, 65)
                )
            },
            {
                BoxColor.Teal,
                new BoxColorScheme(
                    ColorUtil.FromRGB(0, 104, 104),
                    ColorUtil.FromRGB(0, 87, 87)
                )
            },
            {
                BoxColor.Cyan,
                new BoxColorScheme(
                    ColorUtil.FromRGB(47, 144, 143),
                    ColorUtil.FromRGB(46, 125, 126)
                )
            },
            {
                BoxColor.Blue,
                new BoxColorScheme(
                    ColorUtil.FromRGB(30, 73, 104),
                    ColorUtil.FromRGB(25, 62, 89)
                )
            },
            {
                BoxColor.Purple,
                new BoxColorScheme(
                    ColorUtil.FromRGB(92, 63, 113),
                    ColorUtil.FromRGB(74, 48, 92)
                )
            },
            {
                BoxColor.Burgundy,
                new BoxColorScheme(
                    ColorUtil.FromRGB(102, 40, 56),
                    ColorUtil.FromRGB(84, 35, 49)
                )
            },
            {
                BoxColor.Pink,
                new BoxColorScheme(
                    ColorUtil.FromRGB(171, 63, 95),
                    ColorUtil.FromRGB(151, 56, 84)
                )
            },
            {
                BoxColor.Red,
                new BoxColorScheme(
                    ColorUtil.FromRGB(163, 40, 28),
                    ColorUtil.FromRGB(133, 31, 21)
                )
            },
            {
                BoxColor.Orange,
                new BoxColorScheme(
                    ColorUtil.FromRGB(203, 74, 11),
                    ColorUtil.FromRGB(174, 70, 10)
                )
            },
            {
                BoxColor.Yellow,
                new BoxColorScheme(
                    ColorUtil.FromRGB(163, 156, 0),
                    ColorUtil.FromRGB(137, 131, 0)
                )
            },
            {
                BoxColor.Black,
                new BoxColorScheme(
                    ColorUtil.FromRGB(20, 20, 20),
                    ColorUtil.FromRGB(10, 10, 10)
                )
            },
            {
                BoxColor.Gray,
                new BoxColorScheme(
                    ColorUtil.FromRGB(114, 114, 114),
                    ColorUtil.FromRGB(102, 102, 102)
                )
            },
            {
                BoxColor.Brown,
                new BoxColorScheme(
                    ColorUtil.FromRGB(89, 51, 16),
                    ColorUtil.FromRGB(59, 34, 11)
                )
            },
        };
    }
}
