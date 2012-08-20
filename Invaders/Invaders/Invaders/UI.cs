using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Invaders
{
    class UI
    {
        SpriteFont font;

        int score;
        int playerHealth;
        Vector2 scorePosition;
        Texture2D playerTexture;
        Vector2 healthPosition;

        public void Initialize(SpriteFont spriteFont, Texture2D playerTexture)
        {
            this.font = spriteFont;
            this.playerTexture = playerTexture;
        }

        public void Update(GameTime gameTime, int score, int playerHealth)
        {
            this.score = score;
            this.playerHealth = playerHealth;

            scorePosition = Vector2.Zero;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, score.ToString(), scorePosition, Color.White, 0f,
                Vector2.Zero, 0.2105f, SpriteEffects.None, 0f);

            float scale = 0.5f;
            float tmp = playerTexture.Width * scale;
            for (int i = 0; i < playerHealth; i++)
            {
                spriteBatch.Draw(playerTexture, new Vector2(ScreenSize.Width - (i + 1) * tmp - 3, 0),
                    null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
        }

        public void DrawAtCenter(SpriteBatch spriteBatch, Vector2 offset, string text, float scale, Color color)
        {
            Vector2 size = font.MeasureString(text);
            size.X *= scale;
            size.Y *= scale;

            Vector2 position = new Vector2(
                offset.X + (ScreenSize.Width - size.X) / 2,
                offset.Y + (ScreenSize.Height - size.Y) / 2);

            spriteBatch.DrawString(font, text, position, color, 0f,
                Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }
}
