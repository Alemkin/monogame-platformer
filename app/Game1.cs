using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace app.Desktop
{
  public class Game1 : Game {
    private GraphicsDeviceManager graphics;
		private InputHelper inputHelper;

		private SpriteBatch spriteBatch;

    private SpriteFont font;

    private Body billBody;
    private Body platformBody;

    private enum BillState { ATTACKING, FIGHT_STANCE, IDLE, WALKING }

		private Texture2D platform;
    private Texture2D hillBackground;
    Rectangle rectangle1;
    Rectangle rectangle2;
    private Texture2D billIdle;
    private Texture2D billAttacking;
    private Texture2D billFight;

    private Texture2D billWalkingRightLeg;
    private Texture2D billWalkingLeftLeg;

    private BillState billState = BillState.IDLE;

		private Vector2 billPosition;
    private Vector2 platformPosition;
    private Vector2 titlePosition;
    private Vector2 frameratePosition;

		private World world;

    // Simple camera controls
    private Matrix view;

    private Vector2 cameraPosition;
    private Vector2 screenCenter;

    private int currentFramerate;


    public Game1() {
			graphics = new GraphicsDeviceManager(this);
      graphics.PreferredBackBufferWidth = 1280;
      graphics.PreferredBackBufferHeight = 720;

			Content.RootDirectory = "Content";

      world = new World (new Vector2 (0, 9.82f));
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize() {
      base.Initialize();
  		inputHelper = new InputHelper();
      billPosition = screenCenter;
      titlePosition = new Vector2(20, 5);
      frameratePosition = new Vector2(20, 25);
      rectangle1 = new Rectangle (0, 0, 1280, 720);
      rectangle2 = new Rectangle (1280, 0, 1280, 720);
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent() {
      spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

    	font = Content.Load<SpriteFont>("Font");
      billIdle = Content.Load<Texture2D>("shot/1");
      billAttacking = Content.Load<Texture2D>("shot/2");
      billFight = Content.Load<Texture2D>("shot/3");
      billWalkingRightLeg = Content.Load<Texture2D>("walking/1");
      billWalkingLeftLeg = Content.Load<Texture2D>("walking/2");
      hillBackground = Content.Load<Texture2D>("hillBackground");
      platform = Content.Load<Texture2D>("2dplatform");

      view = Matrix.Identity;
      cameraPosition = Vector2.Zero;
      screenCenter = new Vector2 (graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f);
      platformPosition = new Vector2(5f, 650f);

      // Farseer expects objects to be scaled to MKS (meters, kilos, seconds)
      // 1 meters equals 64 pixels here
      //ConvertUnits.SetDisplayUnitToSimUnitRatio (64f);
    }

    protected override void UnloadContent() {
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime) {
      inputHelper.Update();

      UpdateFramerate(gameTime);
      HandleKeyboardInput();
      HandleBackgroundScroll();
      //We update the world
      world.Step ((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);

      base.Update(gameTime);
    }

    private void UpdateFramerate(GameTime gameTime) {
      var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
      var floatFramerate = 1.0f / deltaTime;
      currentFramerate = (int)Math.Round(floatFramerate);
    }

    private void HandleBackgroundScroll() {
      if (rectangle1.X + hillBackground.Width <= 0) {
        rectangle1.X = rectangle2.X + hillBackground.Width;        
      }
      if (rectangle2.X + hillBackground.Width <= 0) {
        rectangle2.X = rectangle1.X + hillBackground.Width;        
      }
    }

    private void HandleKeyboardInput() {
      if (inputHelper.IsNewKeyPress (Keys.Escape)) { Exit (); }

      billState = BillState.IDLE;
      if (inputHelper.IsKeyDown(Keys.Space) && !inputHelper.IsNewKeyPress(Keys.Space)) {
        billState = BillState.FIGHT_STANCE;
      }

      if (inputHelper.IsKeyDown (Keys.Right)) {
        billState = BillState.WALKING;
        billPosition.X += 5f;
        titlePosition.X += 5f;
        frameratePosition.X += 5f;
        cameraPosition.X -= 5f;
        rectangle1.X += 5;
        rectangle2.X += 5;
      } else if (inputHelper.IsKeyDown (Keys.Left)) {
        billPosition.X -= 5f;
        titlePosition.X -= 5f;
        frameratePosition.X -= 5f;
        cameraPosition.X += 5f;
        rectangle1.X -= 5;
        rectangle2.X -= 5;
        billState = BillState.WALKING;
      }

      if (inputHelper.IsKeyDown(Keys.Down)) {
        billPosition.Y += 5f;
        billState = BillState.WALKING;
        //cameraPosition.Y -= 5f;
      } else if (inputHelper.IsKeyDown(Keys.Up)) {
        billPosition.Y -= 5f;
        billState = BillState.WALKING;
        //cameraPosition.Y += 5f;
      }

      if (inputHelper.IsNewKeyPress(Keys.Space)) {
        billState = BillState.ATTACKING;
      } 

      view = Matrix.CreateTranslation (new Vector3 (cameraPosition - screenCenter, 0f)) * Matrix.CreateTranslation (new Vector3 (screenCenter, 0f));
    }
    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime) {
      GraphicsDevice.Clear(Color.CornflowerBlue);
      string currentGameTimeInSeconds = gameTime.TotalGameTime.Seconds.ToString();
      spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, view);

      spriteBatch.Draw(hillBackground, rectangle1, Color.White);
      spriteBatch.Draw(hillBackground, rectangle2, Color.White);

      DrawBill();

      spriteBatch.Draw(platform, platformPosition, Color.White);
      spriteBatch.DrawString(font, "Elapsed Time: " + currentGameTimeInSeconds + " seconds", titlePosition, Color.White);
      spriteBatch.DrawString(font, "Framerate: " + currentFramerate.ToString(), frameratePosition, Color.White);
      spriteBatch.End();
    }

    private int billWalkingPosition = 0;
    private void DrawBill() {
      var thirdFrameWalkingTime = currentFramerate / 3;
      var twoThirdsFrameWalkingTime = thirdFrameWalkingTime * 2;
      switch (billState) {
        case BillState.IDLE:
          spriteBatch.Draw(billIdle, billPosition, Color.White);
          break;
        case BillState.ATTACKING:
          spriteBatch.Draw(billAttacking, billPosition, Color.White);
          break;
        case BillState.WALKING:
          if (billWalkingPosition <= thirdFrameWalkingTime) {
            billWalkingPosition++;
            spriteBatch.Draw(billWalkingLeftLeg, billPosition, Color.White);
          } else if (billWalkingPosition > thirdFrameWalkingTime && billWalkingPosition <= twoThirdsFrameWalkingTime){
            billWalkingPosition++;
            spriteBatch.Draw(billWalkingRightLeg, billPosition, Color.White);            
          } else {
            billWalkingPosition = 0;
            spriteBatch.Draw(billWalkingLeftLeg, billPosition, Color.White);
          }
          break;
        case BillState.FIGHT_STANCE:
          billWalkingPosition = 0;
          spriteBatch.Draw(billFight, billPosition, Color.White);
          break;
      }
    }
  }
}
