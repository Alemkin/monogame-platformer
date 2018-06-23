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

		private SpriteFont title;

    private Body billBody;
    private Body groundBody;
		private Texture2D ground;
		private Texture2D hillBackground;
		private Texture2D bill;

		private Vector2 billPosition;

		private World world;

    // Simple camera controls
    private Matrix view;

    private Vector2 cameraPosition;
    private Vector2 screenCenter;
    private Vector2 groundOrigin;
    private Vector2 billOrigin;


    public Game1() {
			graphics = new GraphicsDeviceManager(this);
      graphics.PreferredBackBufferWidth = 800;
      graphics.PreferredBackBufferHeight = 480;

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
			billPosition = new Vector2(10, 10);
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent() {
      spriteBatch = new SpriteBatch(GraphicsDevice);
    	title = Content.Load<SpriteFont>("Font");
    	bill = Content.Load<Texture2D>("shot/1");
    	hillBackground = Content.Load<Texture2D>("hillBackground");
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

      if (inputHelper.IsNewKeyPress(Keys.Escape)) { Exit(); }

			if (inputHelper.IsKeyDown(Keys.Right)) {
				billPosition.X += 5;
      }

			if (inputHelper.IsKeyDown(Keys.Left)) {
        billPosition.X -= 5;
      }

			if (inputHelper.IsKeyDown(Keys.Down)) {
        billPosition.Y += 5;
      }

			if (inputHelper.IsKeyDown(Keys.Up)) {
        billPosition.Y -= 5;
      }

      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime) {
      GraphicsDevice.Clear(Color.CornflowerBlue);
      string currentGameTimeInSeconds = gameTime.TotalGameTime.Seconds.ToString();
      spriteBatch.Begin();

      spriteBatch.Draw(hillBackground, new Rectangle(0, 0, 800, 480), Color.White);
			spriteBatch.Draw(bill, billPosition, Color.White);
      spriteBatch.DrawString(title, "Elapsed Time: " + currentGameTimeInSeconds, new Vector2(20, 5), Color.White);
      spriteBatch.End();
    }
  }
}
