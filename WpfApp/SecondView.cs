// // yhh - 
// // WpfApp - WpfApp - SecondView.cs
// // 2019 - 03 - 11 - 14:18
// // 2019 - 03 - 11 - 14:18

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Framework.WpfInterop;

namespace WpfApp
{
	public class SecondView : WpfGame
	{
		private IGraphicsDeviceService _graphicsDeviceService;
		private SpriteBatch spriteBatch;
		private Texture2D texture;
        private Camera2D camera;

		protected override void Initialize()
		{
			_graphicsDeviceService = new WpfGraphicsDeviceService(this);
            camera = new Camera2D(GraphicsDevice);

			base.Initialize();

			Content.RootDirectory = "Content";
			spriteBatch = new SpriteBatch(GraphicsDevice);
			texture = Content.Load<Texture2D>("10000");
		}

		protected override void Draw(GameTime gameTime)
		{
            GraphicsDevice.Clear(Color.AliceBlue);

            base.Draw(gameTime);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicWrap,
                DepthStencilState.Default, RasterizerState.CullNone, null, camera.GetViewMatrix());
			spriteBatch.Draw(texture, Vector2.Zero, Color.White);
			spriteBatch.End();
		}
	}
}