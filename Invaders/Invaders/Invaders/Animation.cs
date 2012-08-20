using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Invaders
{
    /// <summary>
    /// Animation is not implemented. Now it's just a static texture.
    /// </summary>
    class Animation : ICloneable
    {
        public Texture2D Texture;
        public Vector2 Position;
        public int FrameWidth;
        public int FrameHeight;
        public float Scale;

        public bool Visible;

        Rectangle destinationRectangle;

        public int Width
        {
            get { return (int)(FrameWidth * Scale); }
        }

        public int Height
        {
            get { return (int)(FrameHeight * Scale); }
        }

        public void Initialize(Texture2D texture, Vector2 position,
            int frameWidth, int frameHeight, float scale)
        {
            this.Texture = texture;
            this.Position = position;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.Scale = scale;

            Visible = true;

            destinationRectangle = new Rectangle();
        }

        public void Update(GameTime gameTime)
        {
            destinationRectangle.X = (int)Position.X;
            destinationRectangle.Y = (int)Position.Y;
            destinationRectangle.Width = Width;
            destinationRectangle.Height = Height;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
                spriteBatch.Draw(Texture, destinationRectangle, null, Color.White);
        }

        public object Clone()
        {
            return base.MemberwiseClone();
        }
    }
}
