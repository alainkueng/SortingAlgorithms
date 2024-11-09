using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Task_2.Assets
{
    internal class Button
    {
        private Texture2D texture;
        private Vector2 position;
        private Rectangle bounds;
        private bool isHovered;
        private bool isPressed;
        private SpriteFont font;
        private string text;
        private int borderWidth = 2;
        private Color borderColor = Color.Black;

        public event EventHandler Click;

        public Button(Vector2 size, Vector2 position, GraphicsDevice graphic, SpriteFont font, string text)
        {
            this.position = position;
            bounds = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            texture = new Texture2D(graphic, bounds.Width, bounds.Height);
            Color[] colorData = new Color[bounds.Width * bounds.Height];
            for (int i = 0; i < colorData.Length; i++)
                colorData[i] = Color.White;
            texture.SetData(colorData);
            this.font = font;
            this.text = text;
        }

        public void Update(MouseState mouseState)
        {
            Point mousePosition = new Point(mouseState.X, mouseState.Y);
            if (bounds.Contains(mousePosition))
            {
                isHovered = true;
                if (mouseState.LeftButton == ButtonState.Pressed)
                    isPressed = true;
                else if (isPressed)
                {
                    isPressed = false;
                    Click?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                isHovered = false;
                isPressed = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color color = isPressed ? Color.Gray : isHovered ? Color.YellowGreen : Color.LightGoldenrodYellow;
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, color);

            // Draw border
            Texture2D borderTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            borderTexture.SetData(new[] { borderColor });
            spriteBatch.Draw(borderTexture, new Rectangle(bounds.Left, bounds.Top, borderWidth, bounds.Height), borderColor);
            spriteBatch.Draw(borderTexture, new Rectangle(bounds.Right - borderWidth, bounds.Top, borderWidth, bounds.Height), borderColor);
            spriteBatch.Draw(borderTexture, new Rectangle(bounds.Left, bounds.Top, bounds.Width, borderWidth), borderColor);
            spriteBatch.Draw(borderTexture, new Rectangle(bounds.Left, bounds.Bottom - borderWidth, bounds.Width, borderWidth), borderColor);

            // Scale font size
            float fontScale = 0.75f;
            Vector2 textSize = font.MeasureString(text) * fontScale;
            Vector2 textPosition = new Vector2(bounds.Center.X - textSize.X / 2, bounds.Center.Y - textSize.Y / 2);
            spriteBatch.DrawString(font, text, textPosition, Color.Black, 0, Vector2.Zero, fontScale, SpriteEffects.None, 1);

            spriteBatch.End();
        }

        public Vector2 Position { get => position; set => position = value; }
        public bool IsPressed { get => isPressed; }
    }
}