using OpenTK.Windowing.Desktop;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            GameWindowSettings gws = GameWindowSettings.Default;
            NativeWindowSettings nws = new NativeWindowSettings();
            nws.Size = new OpenTK.Mathematics.Vector2i(800, 800);

            Window window = new Window(gws, nws);
            window.Run();
        }
    }
}
