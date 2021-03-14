using System;

namespace ExampleMultipleRenderTargets
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Game1DrawPrimitivesMrtGL()) game.Run();
            //using (var game = new Game1GL()) game.Run();
        }
    }
}
