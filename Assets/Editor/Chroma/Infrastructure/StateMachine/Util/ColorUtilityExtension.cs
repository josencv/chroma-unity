using UnityEngine;

namespace Chroma.Editor.Infrastructure.StateMachine.Util
{
    public static class ColorUtils
    {
        public static Color FromRGB(int r, int g, int b)
        {
            return new Color(r / 255f, g / 255f, b / 255f);
        }
    }
}
