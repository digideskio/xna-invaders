using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Invaders
{
    static class ScreenSize
    {
        public static Viewport Viewport;

        public static int Width
        {
            get { return Viewport.Width; }
        }

        public static int Height
        {
            get { return Viewport.Height; }
        }
    }
}
