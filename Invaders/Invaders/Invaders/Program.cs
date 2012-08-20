using System;

namespace Invaders
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (InvadersGame game = new InvadersGame())
            {
                game.Run();
            }
        }
    }
#endif
}

