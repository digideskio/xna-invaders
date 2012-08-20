using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Invaders
{
    class InvadersSquad
    {
        public Animation[] Animations;
        public List<Invader> Invaders;

        // Moving
        public Vector2 Position;
        public Direction Direction;
        Direction nextDirection;
        float downStartPosition;
        const int stepDown = 20;
        public float MoveSpeed;

        // Shooting
        public List<Bullet> Bullets;
        public Animation BulletAnimation;
        public SoundEffect BulletSound;
        double lastShootTime;
        public int ShootDelay;
        int maxShot;

        // Other
        public Vector2 SpaceBetween;
        public Vector2 InvaderSize;
        public const int SquadWidth = 6;
        public const int SquadHeight = 5;

        public bool IsCrossedBottom;

        public int[] Score = { 10, 20, 30, 40, 50 };

        public int Width
        {
            get
            {
                return SquadWidth * (int)InvaderSize.X
                    + (SquadWidth - 1) * (int)SpaceBetween.X;
            }
        }

        public int Height
        {
            get
            {
                return SquadHeight * (int)InvaderSize.Y
                    + (SquadHeight - 1) * (int)SpaceBetween.Y;
            }
        }

        public void Initialize(List<Invader> invaders, Animation[] animations,
            List<Bullet> bullets, Animation bulletAnimation, SoundEffect bulletSound)
        {
            this.Invaders = invaders;
            this.Animations = animations;
            this.Bullets = bullets;
            this.BulletAnimation = bulletAnimation;
            this.BulletSound = bulletSound;
            SpaceBetween = new Vector2(10, 10);

            InvaderSize.X = animations[0].Width;
            InvaderSize.Y = animations[0].Height;
            for (int i = 1; i < animations.Length; i++)
            {
                if (InvaderSize.X < animations[i].Width)
                    InvaderSize.X = animations[i].Width;
                if (InvaderSize.Y < animations[i].Height)
                    InvaderSize.Y = animations[i].Height;
            }
        }

        public void NewRound(int round)
        {
            Invaders.Clear();

            Position = new Vector2(
                (ScreenSize.Width - Width) / 2,
                1);

            Random random = new Random();
            Direction = (Direction)random.Next(2, 4);

            for (int i = 0; i < SquadHeight; i++)
            {
                for (int j = 0; j < SquadWidth; j++)
                {
                    Invader invader = new Invader();
                    invader.Initialize(
                        (Animation)Animations[SquadHeight - 1 - i].Clone(),
                        CalcInvaderPosition(Position, j, i),
                        Direction,
                        Score[SquadHeight - 1 - i],
                        j, i);

                    Invaders.Add(invader);
                }
            }

            MoveSpeed = 1.0f + round * 0.3f;
            IsCrossedBottom = false;
            maxShot = round + 2;
            ShootDelay = 700 - 5 * round;
        }

        public void Update(GameTime gameTime)
        {
            if (Direction == Direction.Left)
            {
                Position.X -= MoveSpeed;
                if (Position.X < 0)
                {
                    Position.X = 0;
                    nextDirection = Direction.Right;
                    Direction = Direction.Down;
                    downStartPosition = Position.Y;
                }
            }
            else if (Direction == Direction.Right)
            {
                Position.X += MoveSpeed;
                if (Position.X + Width > ScreenSize.Width)
                {
                    Position.X = ScreenSize.Width - Width;
                    nextDirection = Direction.Left;
                    Direction = Direction.Down;
                    downStartPosition = Position.Y;
                }
            }
            else if (Direction == Direction.Down)
            {
                Position.Y += MoveSpeed;
                if (Position.Y > downStartPosition + stepDown)
                {
                    Position.Y = downStartPosition + stepDown;
                    Direction = nextDirection;
                }
            }

            for (int i = Invaders.Count - 1; i >= 0; i--)
            {
                Invaders[i].Position = CalcInvaderPosition(Position,
                    Invaders[i].SquadPositionX, Invaders[i].SquadPositionY);
                Invaders[i].Update(gameTime);

                if (Invaders[i].IsCrossedBottom)
                    IsCrossedBottom = true;

                if (!Invaders[i].Active)
                    Invaders.RemoveAt(i);
            }

            // Shooting
            if (Bullets.Count < maxShot && 
                ShootDelay < gameTime.TotalGameTime.TotalMilliseconds - lastShootTime)
            {
                Random r = new Random((int)gameTime.TotalGameTime.TotalMilliseconds);

                int f = r.Next(0, Invaders.Count);

                Vector2 bulletPosition = Invaders[f].Position;
                bulletPosition.X += Invaders[f].Width / 2;
                bulletPosition.Y += Invaders[f].Height;

                Bullet bullet = new Bullet();
                bullet.Initialize((Animation)BulletAnimation.Clone(),
                    bulletPosition, Direction.Down, Bullet.DefaultSpeed * 0.25f, 1);

                Bullets.Add(bullet);
                BulletSound.Play();

                lastShootTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Invader invader in Invaders)
            {
                invader.Draw(spriteBatch);
            }
        }

        public Vector2 CalcInvaderPosition(Vector2 mainPosition, int onX, int onY)
        {
            return new Vector2(
                mainPosition.X + onX * (InvaderSize.X + SpaceBetween.X),
                mainPosition.Y + onY * (InvaderSize.Y + SpaceBetween.Y));
        }
    }
}
