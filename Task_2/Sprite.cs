using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Task_2
{
    internal class Sprite : DrawableGameComponent
    {
        private Texture2D texture;
        private Vector2 position;
        private float number;

        public Sprite(Game game, Texture2D texture, Vector2 position, int number) : base(game)
        {
            this.texture = texture;
            this.position = position;
            this.number = number;
           
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, Color.LightGoldenrodYellow);
            spriteBatch.End();
        }


        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public float Number
        {
            get { return number; }
            set { number = value; }
        }
    }
}