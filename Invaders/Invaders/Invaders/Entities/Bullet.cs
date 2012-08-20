using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Invaders
{
    class Bullet : FlyingEntity
    {
        public const float DefaultSpeed = 20f;

        public Direction Direction;
        public float Velocity;
        public int Damage;

        public void Initialize(Animation animation, Vector2 position,
            Direction direction, float velocity, int damage)
        {
            this.Direction = direction;
            this.Velocity = velocity;
            this.Damage = damage;

            base.Initialize(animation, position);
        }

        public override void Update(GameTime gameTime)
        {
            if (Direction == Direction.Up)
            {
                Position.Y -= Velocity;
            }
            else if (Direction == Direction.Down)
            {
                Position.Y += Velocity;
            }

            if (Position.X > ScreenSize.Width || Position.X < -Width
                || Position.Y > ScreenSize.Height || Position.Y < -Height)
            {
                Active = false;
                Animation.Visible = false;
            }
            else
            {
                Animation.Position = Position;
                Animation.Update(gameTime);
            }
        }
    }
}
