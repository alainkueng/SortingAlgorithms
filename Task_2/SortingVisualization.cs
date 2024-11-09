using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Task_2.Algorithms;
using Task_2.Assets;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;

namespace Task_2
{
    public class SortingVisualization : Game
    {
        private const int WindowHeight = 800;
        private const int WindowWidth = 1000;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _gameFont;
        private SoundEffect _soundEffect;
        private Song backgroundMusic;

        private MouseState _mState;
        private ISortingAlgorithm _selectedAlgorithm;

        private bool _reachedEnd = true;
        private bool _sortingComplete = false;

        private float elapsedTime = 0f;
        private int frameCount = 0;
        private int fps = 0;
        private int _barWidth;
        private int _pixelAmount = 10;
        private int _endCounter = 0;
        private int _frameCounter = 0;
        private int _frameDelay = 4; // Delay in frames
        private int _topOffset = 50;
        private int _leftOffset = 50;
        private int _rightOffset = 50;
        private int _swapCounter = 0;
        
        private double _elapsedTimeSinceEnd;
        private const double ExitDelay = 700; // delay to being able to exit Game

        private List<Sprite> _sprites = new List<Sprite>();
        private List<(int x, int y)> _sortedIndices = new List<(int x, int y)>();
        private List<int> _intList = new List<int>();
        private List<Sprite> _lastChangedSprites = new List<Sprite>();
        private List<ISortingAlgorithm> _sortOptions = new List<ISortingAlgorithm>();

        private Button _startButton;
        private ToggleButton _toggleButton;
        private MultiToggleButton _multiToggleButton;
        private InputField _inputField;
        private DropDown _dropDown;
        private InputField _frameDelayInputField;
        private Stopwatch _sortingStopwatch = new Stopwatch();

        private Texture2D backgroundTexture;
        private Texture2D inGameBackgroudTexture;
        private Texture2D musicOn;
        private Texture2D musicOff;

        public SortingVisualization()
        {
            _graphics = new GraphicsDeviceManager(this);
            IsFixedTimeStep= true;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 30.0);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.Title = "Sorting Algorithms";
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = WindowWidth;
            _graphics.PreferredBackBufferHeight = WindowHeight;
            _graphics.ApplyChanges();

            InitializeSortOptions();

            base.Initialize();
        }

        private void InitializeSortOptions()
        {
            _sortOptions.Add(new MergeSort());
            _sortOptions.Add(new QuickSort());
            _sortOptions.Add(new BubbleSort());
            _sortOptions.Add(new HeapSort());
            _sortOptions.Add(new InsertionSort());
            _sortOptions.Add(new SelectionSort());
            _sortOptions.Add(new RadixSort());
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadResources();
            InitializeUIElements();

            _selectedAlgorithm = _dropDown.SelectedAlgorithm;
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;
            CreateSprites();
        }

        private void LoadResources()
        {
            _gameFont = Content.Load<SpriteFont>("galleryFont");
            _soundEffect = Content.Load<SoundEffect>("Blip");
            backgroundMusic = Content.Load<Song>("stardust-danijel-zambo-main-version-03-13-1372");
        }

        private void InitializeUIElements()
        {
            int buttonWidth = 500;
            int buttonHeight = 45;
            int inputFieldWidth = 500;
            int inputFieldHeight = 45;
            int spacing = 5;

            float centerX = WindowWidth / 2f;
            float centerY = WindowHeight / 2f;

            backgroundTexture = Content.Load<Texture2D>("background");
            inGameBackgroudTexture = Content.Load<Texture2D>("gameBackground");
            musicOn = Content.Load<Texture2D>("MusicOn");
            musicOff = Content.Load<Texture2D>("MusicOff");

            List<string> fpsOptions = new List<string> { "30 frames per second", "60 frames per second", "120 frames per second", "240 frames per second" };

            _startButton = new Button(new Vector2(buttonWidth, buttonHeight), new Vector2(centerX - buttonWidth / 2f, centerY - buttonHeight * 2 - 100), GraphicsDevice, _gameFont, "Start");

            _multiToggleButton = new MultiToggleButton(new Vector2(inputFieldWidth, inputFieldHeight), new Vector2(centerX - inputFieldWidth / 2f, centerY - buttonHeight - inputFieldHeight - spacing * 2), GraphicsDevice, _gameFont, fpsOptions);

            _inputField = new InputField(new Vector2(inputFieldWidth, inputFieldHeight), new Vector2(centerX - inputFieldWidth / 2f, centerY - inputFieldHeight - spacing), GraphicsDevice, _gameFont, _pixelAmount.ToString(), 10, "Data Points:");

            _frameDelayInputField = new InputField(new Vector2(inputFieldWidth, inputFieldHeight), new Vector2(centerX - inputFieldWidth / 2f, centerY + spacing), GraphicsDevice, _gameFont, _frameDelay.ToString(), 0, "Frame delay:");

            _dropDown = new DropDown(_sortOptions, new Vector2(buttonWidth, buttonHeight), new Vector2(centerX - buttonWidth / 2f, centerY + inputFieldHeight + spacing * 2), GraphicsDevice, _gameFont);

            _toggleButton = new ToggleButton(new Vector2(25, 25), new Vector2(0, 0), GraphicsDevice, _gameFont, "");
        }

        protected override void Update(GameTime gameTime)
        {
            
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsedTime += deltaTime;
            frameCount++;
            if (elapsedTime >= 1f)
            {
                fps = frameCount;
                frameCount = 0;
                elapsedTime = 0f;
            }
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / int.Parse(_multiToggleButton.CurrentState.Split(' ')[0]));

            UpdateUIElements(gameTime);

            if (_reachedEnd)
            {
                HandleReachedEndState();

                _elapsedTimeSinceEnd += gameTime.ElapsedGameTime.TotalMilliseconds;

                if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) && _elapsedTimeSinceEnd >= ExitDelay)
                {
                    Exit();
                }
            }
            else
            {
                UpdateSortingState(gameTime);
                if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)))
                {
                    _reachedEnd = true;
                    _sortingComplete = false;

                    _sortingStopwatch.Reset();
                    _elapsedTimeSinceEnd = 0;
                }
            }
            if (_toggleButton.IsChecked)
            {
                MediaPlayer.Pause();
            }
            else
            {
                MediaPlayer.Resume();
            }

            base.Update(gameTime);
        }

        private void UpdateUIElements(GameTime gameTime)
        {
            if (_reachedEnd)
            {
                _startButton.Update(Mouse.GetState());
                _inputField.Update(gameTime, Mouse.GetState());
                _dropDown.Update(gameTime, Mouse.GetState());
                _frameDelayInputField.Update(gameTime, Mouse.GetState());
                _toggleButton.Update(Mouse.GetState());
                _multiToggleButton.Update(Mouse.GetState());
            }
        }

        private void HandleReachedEndState()
        {
            _mState = Mouse.GetState();

            if (_startButton.IsPressed)
            {
                CreateSprites();
                _reachedEnd = false;
            }
            int.TryParse(_frameDelayInputField.Text, out int newFrameDelay);
            _frameDelay = newFrameDelay >= 0 ? newFrameDelay : _frameDelay;
        }

        private void UpdateSortingState(GameTime gameTime)
        {
            _mState = Mouse.GetState();
            _frameCounter++;

            if (_frameCounter >= _frameDelay)
            {
                PerformSortingStep();
            }
        }

        private void PerformSortingStep()
        {
            _frameCounter = 0;
            int endList = _sortedIndices.Count;
            int pick = 0;

            if (pick < endList)
            {
                if (pick == 0 && !_sortingStopwatch.IsRunning)
                {
                    _sortingStopwatch.Start();
                }
                (int x, int y) = _sortedIndices[pick];
                Swap(x, y);
                _sortedIndices.RemoveAt(pick);

                PlaySwapSound(_sprites[x].Number, _sprites[y].Number);
                pick++;
            }

            HandleSortingCompletion(pick, endList);
        }

        private void HandleSortingCompletion(int pick, int endList)
        {
            if (pick == endList && !_sortingComplete)
            {
                ResetLastChangedSpritesColor();
                _sortingComplete = true;
                _sortingStopwatch.Stop();
            }

            if (pick == endList && _sortingComplete)
            {
                PerformEndAnimation();
            }
        }

        private void PerformEndAnimation()
        {
            if (_endCounter == _pixelAmount)
            {
                if (_mState.LeftButton == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    _reachedEnd = true;
                    _sortingComplete = false;
                    _sortingStopwatch.Reset();
                }
            }
            else
            {
                ChangeColor(_sprites[_endCounter], Color.LightGreen);
                PlaySpriteSound(_sprites[_endCounter].Number);
                _endCounter++;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (!_reachedEnd)
            {
                DrawSprites();
                DrawCounters();
            }

            if (_reachedEnd)
            {
                DrawUIElements();
                
            }
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_gameFont, "FPS: " + fps, new Vector2(WindowWidth-40,0), Color.Red, 0, Vector2.Zero, new Vector2(0.3f), SpriteEffects.None, 0);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawCounters()
        {
            _spriteBatch.Begin();

      
            _spriteBatch.DrawString(_gameFont, $"Swaps: {_swapCounter}", new Vector2(200, 40), Color.Black, 0, Vector2.Zero, new Vector2(0.7f), SpriteEffects.None, 0 );
            float delay = _frameDelay != 0 ? (float)fps / _frameDelay : (float)fps;
            _spriteBatch.DrawString(_gameFont, $"Sorting Frame Rate: {delay:0.###}", new Vector2(500, 40), Color.Black, 0, Vector2.Zero, new Vector2(0.7f), SpriteEffects.None, 0);
            double elapsedTime = _sortingStopwatch.Elapsed.TotalSeconds;
            _spriteBatch.DrawString(_gameFont, $"Time: {elapsedTime:0.###} s", new Vector2(10, 40), Color.Black, 0, Vector2.Zero, new Vector2(0.7f), SpriteEffects.None, 0);
            _spriteBatch.DrawString(_gameFont, $"Data points: {_pixelAmount}", new Vector2(200, 10), Color.Black, 0, Vector2.Zero, new Vector2(0.7f), SpriteEffects.None, 0);
            _spriteBatch.DrawString(_gameFont, $"{_selectedAlgorithm.Name}", new Vector2(10, 10), Color.Black, 0, Vector2.Zero, new Vector2(0.7f), SpriteEffects.None, 0);
            _spriteBatch.DrawString(_gameFont, $"Frame delay: {_frameDelay}", new Vector2(500, 10), Color.Black, 0, Vector2.Zero, new Vector2(0.7f), SpriteEffects.None, 0);
            _spriteBatch.DrawString(_gameFont, "ESC to return", new Vector2(WindowWidth-200, 10), Color.Red, 0, Vector2.Zero, new Vector2(0.7f), SpriteEffects.None, 0);
            _spriteBatch.End();
        }

        private void DrawSprites()
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(inGameBackgroudTexture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.LightGoldenrodYellow);
            _spriteBatch.End();
            foreach (Sprite sprite in _sprites)
            {
                sprite.Draw(_spriteBatch);
            }
        }

        private void DrawUIElements()
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            _spriteBatch.End();

            _startButton.Draw(_spriteBatch);
            _inputField.Draw(_spriteBatch);
            _dropDown.Draw(_spriteBatch);
            _frameDelayInputField.Draw(_spriteBatch);
            _toggleButton.Draw(_spriteBatch);
            _multiToggleButton.Draw(_spriteBatch);
            _spriteBatch.Begin();
            _spriteBatch.Draw(_toggleButton.IsChecked ? musicOff : musicOn, new Rectangle(2, 2, 20, 20), Color.White);
            _spriteBatch.End();
        }
        private void Swap(int i, int j)
        {
            Exchange(_sprites[i], _sprites[j], true);
            _swapCounter++; 
        }

        private void ResetLastChangedSpritesColor()
        {
            foreach (Sprite sprite in _lastChangedSprites)
            {
                ChangeColor(sprite, Color.LightGoldenrodYellow);
            }
            _lastChangedSprites.Clear();
        }

        private void PlaySwapSound(float number1, float number2)
        {
            float pitch1 = Math.Clamp(number1 / _pixelAmount, -1.0f, 1.0f);
            float pitch2 = Math.Clamp(number2 / _pixelAmount, -1.0f, 1.0f);

            _soundEffect.Play(1.0f, pitch1, 0.0f);
            _soundEffect.Play(1.0f, pitch2, 0.0f);
        }

        private void PlaySpriteSound(float number)
        {
            float pitch = Math.Clamp(number / _pixelAmount, -1.0f, 1.0f);
            _soundEffect.Play(1.0f, pitch, 0.0f);
        }
        private void Exchange(Sprite first, Sprite second, bool changeColoring = false)
        {
            if (_lastChangedSprites.Count != 0)
            {
                ResetLastChangedSpritesColor();
            }

            float firstPosition = first.Position.X;
            float secondPosition = second.Position.X;

            float y1 = first.Position.Y;
            float y2 = second.Position.Y;
            float number2 = second.Number;
            float number1 = first.Number;
            first.Position = new Vector2(secondPosition, y1);
            first.Number = number2;

            second.Position = new Vector2(firstPosition, y2);
            second.Number = number1;

            if (changeColoring)
            {
                ChangeColor(first, Color.Red);
                ChangeColor(second, Color.Red);
                _lastChangedSprites.Add(first);
                _lastChangedSprites.Add(second);
            }
        }

        private void ChangeColor(Sprite sprite, Color color)
        {
            Color[] colorData = new Color[sprite.Texture.Width * sprite.Texture.Height];
            sprite.Texture.GetData(colorData);

            for (int i = 0; i < colorData.Length; i++)
            {
                colorData[i] = color;
            }

            sprite.Texture.SetData(colorData);
        }
        private void CreateSprites()
        {

            _selectedAlgorithm = _dropDown.SelectedAlgorithm;
            int MaxPixelAmount = WindowWidth - _leftOffset - _rightOffset;
            _pixelAmount = Math.Min(int.Parse(_inputField.Text), MaxPixelAmount);
            _barWidth = Math.Max(1, (WindowWidth - _leftOffset - _rightOffset) / _pixelAmount);

            // Calculate the actual right offset to make sure the bars fill the entire width
            int adjustedRightOffset = WindowWidth - _leftOffset - (_barWidth * _pixelAmount);

            _swapCounter = 0;
            _endCounter = 0;
            _sprites.Clear();
            _sortedIndices.Clear();
            _intList.Clear();
            _lastChangedSprites.Clear();

            int totalWidthCovered = 0;

            for (int i = 0; i < _pixelAmount; i++)
            {
                int barHeight = (int)Math.Round((float)(WindowHeight - _topOffset) * (i + 1) / _pixelAmount);
                int currentBarWidth = _barWidth;

                totalWidthCovered += currentBarWidth;

                Texture2D pixel = new Texture2D(GraphicsDevice, currentBarWidth, barHeight);
                Color[] colorData = new Color[currentBarWidth * barHeight];
                for (int j = 0; j < colorData.Length; j++)
                {
                    colorData[j] = Color.LightGoldenrodYellow;
                }
                pixel.SetData(colorData);
                Sprite sprite = new Sprite(this, pixel, new Vector2(_leftOffset + ((totalWidthCovered - currentBarWidth)), WindowHeight - barHeight), i);
                _sprites.Add(sprite);
            }

            Random rnd = new Random();
            for (int i = 0; i < _pixelAmount * 20; i++)
            {
                int index1 = rnd.Next(_sprites.Count);
                int index2 = rnd.Next(_sprites.Count);
                Exchange(_sprites[index1], _sprites[index2]);
            }

            foreach (Sprite sprite in _sprites)
            {
                _intList.Add((int)sprite.Number);
            }

            _selectedAlgorithm.Sort(_intList);
            _sortedIndices = _selectedAlgorithm.Indices;
        }
    }
}
