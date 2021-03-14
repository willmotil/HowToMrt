using System;

namespace ExampleMultipleRenderTargetsDx
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Game1DrawPrimitivesMrtDx()) game.Run();
            //using (var game = new Game1Dx()) game.Run();
        }
    }
}
