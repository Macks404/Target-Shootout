using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;


namespace game
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D targetSprite;
        Texture2D crosshairsSprite;
        Texture2D backgroundSprite;

        SpriteFont gameFont;

        Vector2 targetPos = new Vector2(200,200);
        const int targetRad = 45; //size sprite is 90x90 meaning radius is 45
        int score = 0;
        bool mouseReleased = true;

        string interval = "Click";

        MouseState mouseState;

        Stopwatch stopwatch = new Stopwatch();
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Window.Title = "Target Shootout";
            _graphics.PreferredBackBufferWidth = 400;
            _graphics.PreferredBackBufferHeight = 400;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            targetSprite = Content.Load<Texture2D>("target");
            crosshairsSprite = Content.Load<Texture2D>("crosshairs");
            backgroundSprite = Content.Load<Texture2D>("sky");
            gameFont = Content.Load<SpriteFont>("galleryFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit(); 

            mouseState = Mouse.GetState();

            if(mouseState.LeftButton == ButtonState.Pressed && mouseReleased)
            {
                float targetDist = Vector2.Distance(targetPos,mouseState.Position.ToVector2());
                if(targetDist < targetRad)
                {
                    stopwatch.Stop();
                    score++;
                    int intervalS = stopwatch.Elapsed.Seconds;
                    int intervalMS = stopwatch.Elapsed.Milliseconds;   
                    interval = intervalS+"s : "+intervalMS+"ms";       
                    
                    Random rand = new Random();
                    targetPos.X = rand.Next(0,_graphics.PreferredBackBufferWidth);
                    targetPos.Y = rand.Next(0,_graphics.PreferredBackBufferHeight);
                    stopwatch.Reset();
                    stopwatch.Start();
                }

                mouseReleased=false;
            }
            if(mouseState.LeftButton == ButtonState.Released)
            {
                mouseReleased=true;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(backgroundSprite,new Vector2(0,0),Color.White);
            _spriteBatch.Draw(targetSprite,new Vector2(targetPos.X-targetRad,targetPos.Y-targetRad),Color.White);
            _spriteBatch.DrawString(gameFont,score.ToString(),new Vector2(0,50),Color.White);
            _spriteBatch.DrawString(gameFont,interval.ToString(),new Vector2(0,0),Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
