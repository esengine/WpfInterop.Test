// // yhh - 
// // WpfApp - WpfApp - SecondView.cs
// // 2019 - 03 - 11 - 14:18
// // 2019 - 03 - 11 - 14:18

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;

namespace WpfApp
{
	public class SecondView : WpfGame
	{
		private IGraphicsDeviceService _graphicsDeviceService;
		private SpriteBatch spriteBatch;
		private Texture2D texture;

		protected override void Initialize()
		{
			_graphicsDeviceService = new WpfGraphicsDeviceService(this);

			base.Initialize();

			Content.RootDirectory = "Content";
			spriteBatch = new SpriteBatch(GraphicsDevice);
			texture = Content.Load<Texture2D>("10000");
		}

		protected override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			spriteBatch.Begin();
			spriteBatch.Draw(texture, Vector2.Zero, Color.White);
			spriteBatch.End();
		}
	}
}