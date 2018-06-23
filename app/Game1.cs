using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace app.Desktop
{
  public class Game1 : Game {
    GraphicsDeviceManager graphics;
    InputHelper inputHelper;

    SpriteBatch spriteBatch;

    SpriteFont title;

    Texture2D hillBackground;
    Texture2D bill;
		Vector2 billPosition;

    public Game1() {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
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
