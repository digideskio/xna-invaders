using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Invaders
{
    class FlyingEntity
    {
        public Animation Animation;
        public Vector2 Position;

        public bool Active;

        public int Width
        {
            get { return Animation.Width; }
        }

        public int Height
        {
            get { return Animation.Height; }
        }

        public void Initialize(Animation animation, Vector2 position)
        {
            this.Animation = animation;
            this.Position = position;

            Active = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            Animation.Position = Position;
            Animation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch);
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }
    }
}
