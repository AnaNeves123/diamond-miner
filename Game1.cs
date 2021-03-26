using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System;
using IPCA.MonoGame;

namespace pigeonMiner
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player Pombo;
        private SpriteFont arial12;
        private string[] levelNames = { "2level.txt" };//, "level1.txt" };
        private int currentLevel = 0;
        private double levelTime = 0f;
        private int lifeCount = 3;
        private bool rDown = false;
        private bool isWin = false;


        public const int tileSize = 32;
        public char[,] level;

        private SpriteSheet _sheet;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            new KeyboardManager(this);
            KeyboardManager.Register(Keys.Escape, KeysState.GoingDown, Exit);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            LoadLevel(levelNames[currentLevel]);
            _graphics.PreferredBackBufferHeight = tileSize * (level.GetLength(1) + 1);
            _graphics.PreferredBackBufferWidth = tileSize * level.GetLength(0);
            _graphics.ApplyChanges();



            base.Initialize();
        }


        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _sheet = new SpriteSheet(this, "TextureDiamondMiner");
            //arial12 = Content.Load<SpriteFont>("arial12");
        }

        protected override void Update(GameTime gameTime)
        {
            if (!isWin) levelTime += gameTime.ElapsedGameTime.TotalSeconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!rDown && Keyboard.GetState().IsKeyDown(Keys.R))
            {
                rDown = true;
                lifeCount--;
                if (isWin || lifeCount < 0)
                {
                    currentLevel = 0;
                    levelTime = 0f;
                    lifeCount = 3;
                    isWin = false;
                }
                Initialize();

            }
            else if (Keyboard.GetState().IsKeyUp(Keys.R))
            {
                rDown = false;
            }


            if (Victory())
            {
                if (currentLevel < levelNames.Length - 1)
                {
                    currentLevel++;
                    Initialize();
                }
                else
                {
                    isWin = true;
                }
            }

            if (!isWin) Pombo.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.BurlyWood);

            _spriteBatch.Begin();

            Rectangle position = new Rectangle(0, 0, tileSize, tileSize);
            for (int x = 0; x < level.GetLength(0); x++)
            {
                for (int y = 0; y < level.GetLength(1); y++)
                {
                    position.X = x * tileSize;
                    position.Y = y * tileSize;

                    switch (level[x, y])
                    {
                        case 'X':
                            _spriteBatch.Draw(_sheet.Sheet, position, _sheet["Wall_Black.png"], Color.White);
                            break;
                        case 'T':
                            _spriteBatch.Draw(_sheet.Sheet, position, _sheet["Dirt.png"], Color.White);
                            break;
                        case 'O':
                            _spriteBatch.Draw(_sheet.Sheet, position, _sheet["Pedra.png"], Color.White);
                            break;
                        case 'E':
                            _spriteBatch.Draw(_sheet.Sheet, position, _sheet["bomb.png"], Color.White);
                            break;
                        case 'D':
                            _spriteBatch.Draw(_sheet.Sheet, position, _sheet["Diamond.png"], Color.White);
                            break;
                        case 'S':
                            _spriteBatch.Draw(_sheet.Sheet, position, _sheet["Door.png"], Color.White);
                            break;
                    }
                }

            }
            Pombo.Draw(_spriteBatch, _sheet);

            if (isWin)
            {
                Vector2 windowSize = new Vector2(_graphics.PreferredBackBufferWidth,
                    _graphics.PreferredBackBufferHeight);


                Texture2D pixel = new Texture2D(GraphicsDevice, 1, 1);
            }
        }


        public bool Victory()
        {
            if (level[Pombo.getP().X, Pombo.getP().Y] == 'S') return true;
            return false;
        }

        public char Tile(int x, int y)
        {
            return level[x, y];
        }

        public void destroyTile(int x, int y)
        {
            level[x, y] = ' ';
        }

        void LoadLevel(string levelFile)
        {

            string[] linhas = File.ReadAllLines("Content/" + levelFile);
            int nrLinhas = linhas.Length;
            int nrColunas = linhas[0].Length;

            level = new char[nrColunas, nrLinhas];
            for (int x = 0; x < nrColunas; x++)
            {
                for (int y = 0; y < nrLinhas; y++)
                {
                    if (linhas[y][x] == 'Y')
                    {

                        Pombo = new Player(this, x, y);
                        level[x, y] = ' ';
                    }
                    else
                    {
                        level[x, y] = linhas[y][x];
                    }

                }
            }
        }
    }
}

