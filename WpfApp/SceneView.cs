// // yhh - 
// // WpfApp - WpfApp - SceneView.cs
// // 2019 - 03 - 11 - 14:14
// // 2019 - 03 - 11 - 14:14

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Framework.WpfInterop;
using System;
using System.Globalization;
using Microsoft.Xna.Framework.Input;
using MonoGame.Framework.WpfInterop.Input;

namespace WpfApp
{
	public class SceneView : WpfGame
	{
		private IGraphicsDeviceService _graphicsDeviceService;
		private SpriteBatch spriteBatch;
		private Texture2D texture;
        private Camera2D camera;
        public WpfKeyboard keyboard;
        public WpfMouse mouse;

        public bool middlePressed;
        public bool middleIsRecord;
        public Vector2 lastVector;
        public Vector2 moveVector;
        private int gridSize = 25;

        protected override void Initialize()
		{
			_graphicsDeviceService = new WpfGraphicsDeviceService(this);
            camera = new Camera2D(GraphicsDevice);
            keyboard = new WpfKeyboard(this);
            mouse = new WpfMouse(this);

            base.Initialize();

			Content.RootDirectory = "Content";
			spriteBatch = new SpriteBatch(GraphicsDevice);
			texture = Content.Load<Texture2D>("10000");
		}

        #region Overrides of WpfGame

        /// <summary>
        /// The update method that is called to update your game logic.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var mouseState = mouse.GetState();
            if (mouseState.MiddleButton == ButtonState.Released)
                middleIsRecord = false;

            middlePressed = mouseState.MiddleButton == ButtonState.Pressed;

            MoveCamera();
            
        }

        #endregion

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.AliceBlue);

            base.Draw(gameTime);
            
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicWrap,
                DepthStencilState.Default, RasterizerState.CullNone, null, camera.GetViewMatrix());
            DrawGrid();
            spriteBatch.Draw(texture, Vector2.Zero, Color.White);
            DrawMouseLine();
            spriteBatch.End();
		}
        
		private void DrawGrid()
        {
            var widthGrid = Math.Ceiling((decimal)GraphicsDevice.Viewport.Width / gridSize);
            var heightGrid = Math.Ceiling((decimal)GraphicsDevice.Viewport.Height / gridSize);
            var maxWidthGrid = Math.Ceiling((decimal)(GraphicsDevice.Viewport.Width + camera.Position.X) /
                                            gridSize);
            var maxHeightGrid = Math.Ceiling((decimal)(GraphicsDevice.Viewport.Height + camera.Position.Y) /
                                             gridSize);
            var transformWorld = camera.ScreenToWorld(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);


            DrawHorizontalGrid(widthGrid, heightGrid, maxWidthGrid, maxHeightGrid, transformWorld);
            DrawVerticalGrid(widthGrid, heightGrid, maxWidthGrid, maxHeightGrid, transformWorld);
        }
        
        private void DrawHorizontalGrid(decimal width, decimal height, decimal maxWidth, decimal maxHeight,
            Vector2 transform)
        {
            for (var i = maxWidth - width; i < maxWidth; i++)
                for (var j = maxHeight - height; j < maxHeight; j++)
                    spriteBatch.DrawLine(camera.Position.X,
                        float.Parse((gridSize * j).ToString(CultureInfo.InvariantCulture)), transform.X,
                        float.Parse((gridSize * j).ToString(CultureInfo.InvariantCulture)),
                        Color.Green, 1f);
        }
        
        private void DrawVerticalGrid(decimal width, decimal height, decimal maxWidth, decimal maxHeight,
            Vector2 transform)
        {
            for (var i = maxWidth - width; i < maxWidth; i++)
                for (var j = maxHeight - height; j < maxHeight; j++)
                    spriteBatch.DrawLine(float.Parse((gridSize * i).ToString(CultureInfo.InvariantCulture)),
                        camera.Position.Y,
                        float.Parse((gridSize * i).ToString(CultureInfo.InvariantCulture)), transform.Y,
                        Color.Green, 1f);
        }

        private void DrawMouseLine()
        {
            var mousePosition = mouse.GetState().Position;
            if (mousePosition.Y > 0 && mousePosition.Y < GraphicsDevice.Viewport.Height)
                spriteBatch.DrawLine(new Vector2(camera.Position.X, mousePosition.Y + camera.Position.Y),
                    new Vector2(GraphicsDevice.Viewport.Width + camera.Position.X, mousePosition.Y + camera.Position.Y), Color.Red);

            if (mousePosition.X > 0 && mousePosition.X < GraphicsDevice.Viewport.Width)
                spriteBatch.DrawLine(new Vector2(mousePosition.X + camera.Position.X, camera.Position.Y),
                    new Vector2(mousePosition.X + camera.Position.X, GraphicsDevice.Viewport.Height + camera.Position.Y), Color.Red);
        }

        public void MoveCamera()
        {
            if (!middlePressed)
                return;

            var mouseState = mouse.GetState();

            if (!middleIsRecord)
            {
                middleIsRecord = true;
                lastVector = mouseState.Position.ToVector2();
            }

            moveVector = mouseState.Position.ToVector2();
            camera.Move(lastVector - moveVector);
            lastVector = moveVector;
        }
    }
}