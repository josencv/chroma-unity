using UnityEngine;

namespace Chroma.Editor.Infrastructure.StateMachine.Util
{
    public static class ColorUtil
    {
        public static Color FromRGB(int r, int g, int b)
        {
            return new Color(r / 255f, g / 255f, b / 255f);
        }

        public static Color FromRGBA(int r, int g, int b, int a)
        {
            return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
        }
    }
}
