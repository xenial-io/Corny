using System;
using System.Runtime.InteropServices;

namespace Xenial.Delicious.Corny
{
    public static class RenderableExtentions
    {
        public static bool IsCursorVisible(this IRenderable renderable)
        {
#if FULL_FRAMEWORK
            return Console.CursorVisible;
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Console.CursorVisible;
            }
            return true;
#endif
        }

        public static void SetCursorVisible(this IRenderable renderable, bool cursorVisible)
        {
            if (!Console.IsOutputRedirected)
            {
#if FULL_FRAMEWORK
                Console.CursorVisible = cursorVisible;
                Console.CursorSize = cursorVisible ? 100 : 1;
#else
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Console.CursorVisible = cursorVisible;
                    Console.CursorSize = cursorVisible ? 100 : 1;
                }
#endif
            }
        }
    }
}
