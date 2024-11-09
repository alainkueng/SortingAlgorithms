using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Task_2.Algorithms;

namespace Task_2.Assets
{
    internal class DropDown
    {
        private List<ISortingAlgorithm> algorithms;
        public Vector2 position;
        private Vector2 size;
        private Texture2D texture;
        private SpriteFont font;
        private Rectangle bounds;
        private bool isHovered;
        private bool isOpen;
        private int selectedIndex;

        public event EventHandler<ISortingAlgorithm> AlgorithmSelected;

        public DropDown(List<ISortingAlgorithm> algorithms, Vector2 size, Vector2 position, GraphicsDevice graphicsDevice, SpriteFont font)
        {
            this.algorithms = algorithms;
            this.position = position;
            this.size = size;
            this.font = font;

            bounds = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);

            // Create a Texture2D that matches the size of the Rectangle
            texture = new Texture2D(graphicsDevice, bounds.Width, bounds.Height);
            Color[] colorData = new Color[bounds.Width * bounds.Height];
            for (int i = 0; i < colorData.Length; i++)
            {
                colorData[i] = Color.White;
            }
            texture.SetData(colorData);
            SelectedAlgorithm = algorithms[0];
            selectedIndex = 0;
        }
        public ISortingAlgorithm SelectedAlgorithm { get; private set; }
        public Vector2 Position { get; private set; }

        private double clickDelay = 200; // delay in milliseconds
        private double lastClickTime;

        public void Update(GameTime gameTime, MouseState mouseState)
        {
            Point mousePosition = new Point(mouseState.X, mouseState.Y);
            bool clickedOutside = true;

            if (bounds.Contains(mousePosition))
            {
                isHovered = true;
                clickedOutside = false;

                if (mouseState.LeftButton == ButtonState.Pressed && gameTime.TotalGameTime.TotalMilliseconds - lastClickTime > clickDelay)
                {
                    isOpen = !isOpen;
                    lastClickTime = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            else
            {
                isHovered = false;
            }

            if (isOpen)
            {
                int itemIndex = 0;
                for (int i = 0; i < algorithms.Count; i++)
                {
                    if (i != selectedIndex)
                    {
                        Rectangle itemBounds = new Rectangle((int)position.X, (int)position.Y + (int)size.Y * (itemIndex + 1), (int)size.X, (int)size.Y);

                        if (itemBounds.Contains(mousePosition) && mouseState.LeftButton == ButtonState.Pressed && gameTime.TotalGameTime.TotalMilliseconds - lastClickTime > clickDelay)
                        {
                            selectedIndex = GetIndexFromDisplayIndex(itemIndex);
                            isOpen = false;
                            AlgorithmSelected?.Invoke(this, algorithms[selectedIndex]);
                            SelectedAlgorithm = algorithms[selectedIndex];
                            lastClickTime = gameTime.TotalGameTime.TotalMilliseconds;
                            break;
                        }

                        itemIndex++;
                    }
                }

                if (clickedOutside && mouseState.LeftButton == ButtonState.Pressed && gameTime.TotalGameTime.TotalMilliseconds - lastClickTime > clickDelay)
                {
                    isOpen = false;
                    lastClickTime = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
        }

        private int GetIndexFromDisplayIndex(int displayIndex)
        {
            int actualIndex = 0;
            int currentDisplayIndex = 0;

            while (currentDisplayIndex <= displayIndex)
            {
                if (actualIndex != selectedIndex)
                {
                    currentDisplayIndex++;
                }
                if (currentDisplayIndex <= displayIndex)
                {
                    actualIndex++;
                }
            }

            return actualIndex;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Color mainColor = isHovered ? Color.YellowGreen : Color.LightGoldenrodYellow;

            spriteBatch.Draw(texture, position, mainColor);
            DrawBorder(spriteBatch, bounds, 1, Color.Black);

            float fontScale = 0.75f;
            Vector2 textSize = font.MeasureString(algorithms[selectedIndex].Name) * fontScale;
            float centerY = bounds.Center.Y - textSize.Y / 2;
            spriteBatch.DrawString(font, algorithms[selectedIndex].Name, new Vector2(position.X + 5, centerY), Color.Black, 0, Vector2.Zero, fontScale, SpriteEffects.None, 1);

            if (isOpen)
            {
                int itemIndex = 0;
                for (int i = 0; i < algorithms.Count; i++)
                {
                    if (i != selectedIndex)
                    {
                        Rectangle itemBounds = new Rectangle((int)position.X, (int)position.Y + (int)size.Y * (itemIndex + 1), (int)size.X, (int)size.Y);

                        Color itemColor = itemBounds.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) ? Color.YellowGreen : Color.LightGoldenrodYellow;

                        spriteBatch.Draw(texture, position + new Vector2(0, size.Y * (itemIndex + 1)), itemColor);

                        textSize = font.MeasureString(algorithms[i].Name) * fontScale;
                        centerY = (bounds.Center.Y - textSize.Y / 2) + (size.Y * (itemIndex + 1));
                        spriteBatch.DrawString(font, algorithms[i].Name, new Vector2(position.X + 5, centerY), Color.Black, 0, Vector2.Zero, fontScale, SpriteEffects.None, 1);

                        DrawBorder(spriteBatch, itemBounds, 1, Color.Black);

                        itemIndex++;
                    }
                }
            }

            spriteBatch.End();
        }
        private void DrawBorder(SpriteBatch spriteBatch, Rectangle rectangleToDraw, int borderWidth, Color borderColor)
        {
            spriteBatch.Draw(texture, new Rectangle(rectangleToDraw.Left, rectangleToDraw.Top, borderWidth, rectangleToDraw.Height), borderColor); // Left
            spriteBatch.Draw(texture, new Rectangle(rectangleToDraw.Right - borderWidth, rectangleToDraw.Top, borderWidth, rectangleToDraw.Height), borderColor); // Right
            spriteBatch.Draw(texture, new Rectangle(rectangleToDraw.Left, rectangleToDraw.Top, rectangleToDraw.Width, borderWidth), borderColor); // Top
            spriteBatch.Draw(texture, new Rectangle(rectangleToDraw.Left, rectangleToDraw.Bottom - borderWidth, rectangleToDraw.Width, borderWidth), borderColor); // Bottom
        }

    }
}