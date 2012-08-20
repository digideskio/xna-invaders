using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Invaders
{
    static class Collisions
    {
        public static void BetweenPlayerAndInvaders(Player player,
            List<Invader> invaders)
        {
            Rectangle playerRec = player.GetRectangle();

            for (int i = invaders.Count - 1; i >= 0; i--)
            {
                if (playerRec.Intersects(invaders[i].GetRectangle()))
                {
                    player.Health -= invaders[i].Damage;
                    player.Score += invaders[i].ScoreValue;
                    invaders[i].Health = 0;
                    // Explosion?
                    invaders.RemoveAt(i);
                }
            }
        }

        public static void BetweenInvadersAndPlayersBullets(Player player, List<Invader> invaders,
            List<Bullet> bullets)
        {
            for (int i = invaders.Count - 1; i >= 0; i--)
            {
                Rectangle invader = invaders[i].GetRectangle();

                for (int j = bullets.Count - 1; j >= 0; j--)
                {
                    if (invader.Intersects(bullets[j].GetRectangle()))
                    {
                        invaders[i].Health -= bullets[j].Damage;
                        bullets[j].Active = false;
                        bullets.RemoveAt(j);
                    }
                }

                if (invaders[i].Health <= 0)
                {
                    invaders[i].Active = false;
                    player.Score += invaders[i].ScoreValue;
                    invaders.RemoveAt(i);
                }
            }
        }

        public static void BetweenPlayerAndInvadersBullets(Player player,
            List<Bullet> bullets)
        {
            Rectangle playerRec = player.GetRectangle();

            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                if (playerRec.Intersects(bullets[i].GetRectangle()))
                {
                    player.Health -= bullets[i].Damage;
                    bullets[i].Active = false;
                    bullets.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Check for collision between player's bullets and invaders' bullets
        /// </summary>
        /// <returns>Numbers of collisions</returns>
        public static int BetweenBullets(List<Bullet> playersBullets,
            List<Bullet> invadersBullets)
        {
            int collision = 0;

            for (int i = playersBullets.Count - 1; i >= 0; i--)
            {
                Rectangle bullet = playersBullets[i].GetRectangle();

                for (int j = invadersBullets.Count - 1; j >= 0; j--)
                {
                    if (bullet.Intersects(invadersBullets[j].GetRectangle()))
                    {
                        invadersBullets[j].Active = false;
                        invadersBullets.RemoveAt(j);

                        playersBullets[i].Active = false;
                        playersBullets.RemoveAt(i);

                        collision++;
                        break;
                    }
                }
            }
            return collision;
        }
    }
}
