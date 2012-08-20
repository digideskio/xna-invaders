using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Invaders
{
    class Player : FlyingEntity
    {
        public int Health;
        public int Score;
        public float MoveSpeed;

        List<Bullet> bullets;

        // TryShoot
        Animation bulletAnimation;
        SoundEffect bulletSound;
        int bulletDamage;
        double lastShootTime;
        public int ShootDelay;

        public Player(List<Bullet> bullets)
        {
            this.bullets = bullets;
        }

        public void Initialize(Animation animation, Vector2 position,
            Animation bulletAnimation, SoundEffect bulletSound)
        {
            this.bulletAnimation = bulletAnimation;
            this.bulletSound = bulletSound;

            Health = 3;
            Score = 0;
            MoveSpeed = 8.0f;
            lastShootTime = 0;
            ShootDelay = 500;
            bulletDamage = 1;
            
            base.Initialize(animation, position);
        }

        public bool TryShoot(GameTime gameTime, List<Bullet> shots)
        {
            // Check fire time
            if (gameTime.TotalGameTime.TotalMilliseconds - lastShootTime < ShootDelay)
            {
                return false;
            }

            lastShootTime = gameTime.TotalGameTime.TotalMilliseconds;

            // Calculate bullet position
            Vector2 bulletPosition = new Vector2(
                Position.X + Width / 2 - bulletAnimation.Width / 2,
                Position.Y + Height - bulletAnimation.Height);

            // Create bullet
            Bullet bullet = new Bullet();
            bullet.Initialize((Animation)bulletAnimation.Clone(), bulletPosition,
                Direction.Up, Bullet.DefaultSpeed, bulletDamage);

            shots.Add(bullet);
            if (bulletSound != null)
                bulletSound.Play();

            return true;
        }

        public void ResetPosition()
        {
            Position.X = (ScreenSize.Width - Width) / 2;
            Position.Y = ScreenSize.Height - Height - 1;
        }

        public void Update(GameTime gameTime, MouseState currentMS, MouseState previousMS,
            KeyboardState currentKS, KeyboardState previosKS)
        {
            // Mouse move
            this.Position.X += currentMS.X - previousMS.X;
            this.Position.Y += currentMS.Y - previousMS.Y;

            // Keyboard move
            if (currentKS.IsKeyDown(Keys.Up)
                || currentKS.IsKeyDown(Keys.W))
            {
                this.Position.Y -= this.MoveSpeed;
            }
            if (currentKS.IsKeyDown(Keys.Down)
                || currentKS.IsKeyDown(Keys.S))
            {
                this.Position.Y += this.MoveSpeed;
            }
            if (currentKS.IsKeyDown(Keys.Left)
                || currentKS.IsKeyDown(Keys.A))
            {
                this.Position.X -= this.MoveSpeed;
            }
            if (currentKS.IsKeyDown(Keys.Right)
                || currentKS.IsKeyDown(Keys.D))
            {
                this.Position.X += this.MoveSpeed;
            }

            // Fire
            if (currentKS.IsKeyDown(Keys.Space)
                || currentMS.LeftButton == ButtonState.Pressed)
            {
                this.TryShoot(gameTime, bullets);
            }
            
            this.Position.X = MathHelper.Clamp(this.Position.X,
                0, ScreenSize.Width - this.Width);
            this.Position.Y = MathHelper.Clamp(this.Position.Y,
                0, ScreenSize.Height - this.Height);

            this.Update(gameTime);
        }
    }
}
