using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Windows.Forms;
using Assets;
using HexTbs.Battle;

namespace HexTbs
{
   /// <summary>
   /// This is the main type for your game.
   /// </summary>
   public class Game1 : Game
   {
      GraphicsDeviceManager graphics;
      SpriteBatch spriteBatch;

      BattleFrame Battle { get; set; }

      public Game1()
      {
         graphics = new GraphicsDeviceManager(this);
         Content.RootDirectory = "Content";
         IsMouseVisible = true;

         Form f = (Form)Form.FromHandle(this.Window.Handle);
         f.MinimizeBox = false;
      }

      /// <summary>
      /// Allows the game to perform any initialization it needs to before starting to run.
      /// This is where it can query for any required services and load any non-graphic
      /// related content.  Calling base.Initialize will enumerate through any components
      /// and initialize them as well.
      /// </summary>
      protected override void Initialize()
      {
         /*CursorLoader.game = this;
         CursorLoader.SetCursor(AssetCursor.arrow);*/

         base.Initialize();
      }

      /// <summary>
      /// LoadContent will be called once per game and is the place to load
      /// all of your content.
      /// </summary>
      protected override void LoadContent()
      {
         // Create a new SpriteBatch, which can be used to draw textures.
         spriteBatch = new SpriteBatch(GraphicsDevice);

         int sw = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
         int sh = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
         int ww = 1024;
         int wh = 786;

         graphics.PreferredBackBufferWidth = ww;
         graphics.PreferredBackBufferHeight = wh;
         graphics.IsFullScreen = false;
         this.Window.Position = new Point((sw - ww) / 2, (sh - wh) / 2);
         graphics.ApplyChanges();

         Statics.InitStatics(Content);
         Statics.ScreenSize = new Point(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

         Battle = new BattleFrame();
      }

      private bool reseting = false;
      protected void ResetGame()
      {
         reseting = true;
         Battle = null;
         Battle = new BattleFrame();
         reseting = false;
      }

      /// <summary>
      /// UnloadContent will be called once per game and is the place to unload
      /// game-specific content.
      /// </summary>
      protected override void UnloadContent()
      {
         // TODO: Unload any non ContentManager content here
      }

      /// <summary>
      /// Allows the game to run logic such as updating the world,
      /// checking for collisions, gathering input, and playing audio.
      /// </summary>
      /// <param name="gameTime">Provides a snapshot of timing values.</param>
      protected override void Update(GameTime gameTime)
      {
         if (Battle != null)
            Battle.Update(gameTime);

         if (Battle != null)
         {
            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F2) && !reseting)
               ResetGame();
         }

         base.Draw(gameTime);
      }

      /// <summary>
      /// This is called when the game should draw itself.
      /// </summary>
      /// <param name="gameTime">Provides a snapshot of timing values.</param>
      protected override void Draw(GameTime gameTime)
      {
         GraphicsDevice.Clear(Color.Black);
         if (Battle != null)
            Battle.Draw(spriteBatch);
         MouseController.Update(gameTime);
         base.Update(gameTime);
      }
   }
}
