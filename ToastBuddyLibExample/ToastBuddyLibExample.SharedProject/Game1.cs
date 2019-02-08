using FontBuddyLib;
using GameTimer;
using HadoukInput;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ResolutionBuddy;
using System;
using ToastBuddyLib;

namespace ToastBuddyLibExample
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

		private GameClock _clock;

		FontBuddy InstructionFont;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
			Content.RootDirectory = "Content";

			var resolution = new ResolutionComponent(this, graphics, new Point(1280, 720), new Point(1280, 720), false, false);

			m_Messages = new ToastBuddy(this, "Fonts\\ArialBlack48", UpperRight, Resolution.TransformationMatrix, Justify.Right);
			m_Messages.ShowTime = TimeSpan.FromSeconds(1.0);

			_controller = new ControllerWrapper(PlayerIndex.One);
			_controller.UseKeyboard = true;
			_clock = new GameClock();
        }

		public Vector2 UpperRight()
		{
			return new Vector2(Resolution.TitleSafeArea.Right, Resolution.TitleSafeArea.Top);
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
			InstructionFont.Font = Content.Load<SpriteFont>("Fonts\\ArialBlack24");
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
			    (Keyboard.GetState().IsKeyDown(Keys.Escape)))
			{
#if !__IOS__
				Exit();
#endif
			}

			//Update the input
			_clock.Update(gameTime);
            m_Input.Update();
			_controller.Update(m_Input);

			//Get the toast message component
			IServiceProvider services = Services;
			var messageDisplay = (IToastBuddy)services.GetService(typeof(IToastBuddy));

			//check for button presses
			for (EKeystroke i = 0; i < EKeystroke.Neutral; i++)
			{
				//if this button state changed, pop up a message
				if (_controller.CheckKeystroke(i))
				{
					//pop up a message
					messageDisplay.ShowFormattedMessage("Pressed {0}", Color.Yellow, i.ToString());
				}
			}

			if (Keyboard.GetState().IsKeyDown(Keys.Space))
			{
				//pop up a message
				messageDisplay.ShowMessage("Pressed Space!", Color.Yellow);
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

#if WINDOWS
			Resolution.ResetViewport();
#endif

			spriteBatch.Begin();

			//TODO: Add your drawing code here
			InstructionFont.Write("Press any direction on the controller to pop up messages",
			                      new Vector2(Resolution.TitleSafeArea.Left, Resolution.TitleSafeArea.Top),
			                      Justify.Left,
			                      0.75f,
			                      Color.White,
			                      spriteBatch,
								  _clock);

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
