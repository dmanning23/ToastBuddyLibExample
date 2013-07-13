using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using ToastBuddyLib;
using ResolutionBuddy;
using HadoukInput;
using FontBuddyLib;

namespace ToastBuddyLibExample.Windows
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
	
		ToastBuddy m_Messages;

		private InputState m_Input = new InputState();
		private ControllerWrapper _controller;

		FontBuddy InstructionFont;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.SupportedOrientations = DisplayOrientation.Default;
			Resolution.Init(ref graphics);
			Content.RootDirectory = "Content";
			graphics.IsFullScreen = true;

			Resolution.SetDesiredResolution(1024, 768);
			Resolution.SetScreenResolution(1024, 768, false);

			m_Messages = new ToastBuddy(this, "ArialBlack24", UpperRight, Resolution.TransformationMatrix);
			Components.Add(m_Messages);

			_controller = new ControllerWrapper(PlayerIndex.One);
			_controller.UseKeyboard = true;
		}

		public Vector2 UpperRight()
		{
			return new Vector2(Resolution.TitleSafeArea.Right, Resolution.TitleSafeArea.Top);
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
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

			InstructionFont = new FontBuddy();
			InstructionFont.Font = Content.Load<SpriteFont>("ArialBlack24");
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) ||
			    (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape)))
			{
				Exit();
			}

			//Update the input
			m_Input.Update();
			_controller.Update(m_Input);

			//Get the toast message component
			IServiceProvider services = Services;
			IMessageDisplay messageDisplay = (IMessageDisplay)services.GetService(typeof(IMessageDisplay));

			//check for button presses
			for (EKeystroke i = 0; i < (EKeystroke.RTrigger + 1); i++)
			{
				//if this button state changed, pop up a message
				if (_controller.KeystrokePress[(int)i])
				{
					//pop up a message
					messageDisplay.ShowFormattedMessage("Pressed {0}", i.ToString());
				}
			}

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			Resolution.ResetViewport();

			spriteBatch.Begin();

			//TODO: Add your drawing code here
			InstructionFont.Write("Press any button on the controller to pop up messages",
			                      new Vector2(Resolution.TitleSafeArea.Left, Resolution.TitleSafeArea.Top),
			                      Justify.Left,
			                      0.75f,
			                      Color.White,
			                      spriteBatch,
			                      0.0);

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}

