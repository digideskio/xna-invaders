using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Invaders
{
    class Invader : FlyingEntity
    {
        public Direction Direction;
        public int SquadPositionX;
        public int SquadPositionY;

        public int Damage;
        public int Health;
        //public float MoveSpeed;
        public int ScoreValue;

        public bool IsCrossedBottom;

        public void Initialize(Animation animation, Vector2 position,
            Direction direction, int scoreValue,
            int squadPositionX, int squadPositionY)
        {
            this.Direction = direction;
            Damage = 1;
            Health = 1;
            //this.MoveSpeed = moveSpeed;
            this.ScoreValue = scoreValue;
            this.SquadPositionX = squadPositionX;
            this.SquadPositionY = squadPositionY;

            IsCrossedBottom = false;

            base.Initialize(animation, position);
        }

        public override void Update(GameTime gameTime)
        {
            // Position updates in InvadersSquad
            if (Health <= 0 ||
                Position.X > ScreenSize.Width ||
                Position.X < - Width ||
                Position.Y < - Height)
            {
                Active = false;
                Animation.Visible = false;
            }
            else if (Position.Y > ScreenSize.Height)
            {
                IsCrossedBottom = true;
                Active = false;
                Animation.Visible = false;
            }

            Animation.Position = Position;
            Animation.Update(gameTime);
        }
    }
}
