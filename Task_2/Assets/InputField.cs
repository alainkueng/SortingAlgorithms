using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace Task_2
{
    internal class InputField
    {
        private Texture2D texture;
        private Vector2 position;
        private Rectangle bounds;
        float y_textSize;
        private bool isActive;
        private string text;
        private SpriteFont font;
        private float blinkTimer;
        private float blinkInterval;
        private bool drawCursor;
        private KeyboardState oldState;
        private int minValue;
        private int borderWidth = 2;
        private String label;
        private Color borderColor = Color.Black;

        public InputField(Vector2 size, Vector2 position, GraphicsDevice graphic, SpriteFont font, String input_text, int minValue, String labeling)
        {
            this.position = position;
            this.bounds = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            this.font = font;

            texture = new Texture2D(graphic, bounds.Width, bounds.Height);
            Color[] colorData = new Color[bounds.Width * bounds.Height];
            for (int i = 0; i < colorData.Length; i++)
            {
                colorData[i] = Color.White;
            }
            texture.SetData(colorData);

            text = input_text;
            blinkInterval = 500f;
            blinkTimer = 0f;
            drawCursor = false;
            oldState = Keyboard.GetState();
            this.minValue = minValue;
            this.label = labeling;
            this.y_textSize = font.MeasureString(input_text).Y;
        }

        public void Update(GameTime gameTime, MouseState mouseState)
        {
            Point mousePosition = new Point(mouseState.X, mouseState.Y);
            if (bounds.Contains(mousePosition) && mouseState.LeftButton == ButtonState.Pressed)
            {
                isActive = true;
            }
            else if (!bounds.Contains(mousePosition) && mouseState.LeftButton == ButtonState.Pressed)
            {
                if (text.Length == 0 || int.Parse(text) < minValue)
                {
                    text = "10";
                }
                isActive = false;
            }

            if (isActive)
            {
                KeyboardState newState = Keyboard.GetState();
                Keys[] keysPressed = newState.GetPressedKeys();

                foreach (Keys key in keysPressed)
                {
                    if (!oldState.IsKeyDown(key))
                    {
                        if (key == Keys.Back && text.Length > 0)
                        {
                            text = text.Remove(text.Length - 1);
                        }
                        else if (key >= Keys.D0 && key <= Keys.D9)
                        {
                            char number = (char)('0' + (key - Keys.D0));
                            text += number.ToString();
                        }
                        else if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
                        {
                            char number = (char)('0' + (key - Keys.NumPad0));
                            text += number.ToString();
                        }
                    }
                }
                oldState = newState;

                blinkTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (blinkTimer >= blinkInterval)
                {
                    drawCursor = !drawCursor;
                    blinkTimer = 0f;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color color = isActive ? Color.YellowGreen : Color.LightGoldenrodYellow;
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

        
            float centerY =  textSize.Y != 0 ? bounds.Center.Y - textSize.Y / 2: bounds.Center.Y - y_textSize* fontScale / 2;
            Vector2 textPosition = new Vector2(position.X + 5, centerY);
            spriteBatch.DrawString(font, text, new Vector2(textPosition.X + 170, textPosition.Y) , Color.Black, 0, Vector2.Zero, fontScale, SpriteEffects.None, 1);

            spriteBatch.DrawString(font, label, new Vector2(position.X + 5, centerY), Color.Black, 0, Vector2.Zero, fontScale, SpriteEffects.None, 1);

            if (isActive && drawCursor)
            {
                Vector2 cursorPosition = new Vector2(170 + position.X + 5 + textSize.X, centerY);
                spriteBatch.DrawString(font, "|", cursorPosition, Color.Black, 0, Vector2.Zero, fontScale, SpriteEffects.None, 1);
            }

            spriteBatch.End();
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public string Text
        {
            get { return text; }
        }
    }
}