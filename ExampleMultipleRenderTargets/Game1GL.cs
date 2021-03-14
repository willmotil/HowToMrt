using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ExampleMultipleRenderTargets
{
    public class Game1GL : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        Texture2D texture;
        Effect sbMrtEffect;
        RenderTarget2D rt0, rt1;
        RenderTargetBinding[] renderTargetBinding = new RenderTargetBinding[2];

        public Game1GL()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Window.Title = "MRT Gl";
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = TextureDotCreate(GraphicsDevice, 0,255,255,255); // aqua texture
            sbMrtEffect = Content.Load<Effect>("SbMrtEffect");
            rt0 = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.None);
            rt1 = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.None);
            renderTargetBinding[0] = rt0;
            renderTargetBinding[1] = rt1;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            var w = GraphicsDevice.Viewport.Width / 2 - 10;
            var h = GraphicsDevice.Viewport.Height / 2 - 10;


            GraphicsDevice.SetRenderTargets(renderTargetBinding);
            GraphicsDevice.Clear(Color.DarkBlue);

            spriteBatch.Begin(effect : sbMrtEffect);
            spriteBatch.Draw(texture, new Rectangle(0, 0, w, h), Color.Moccasin);
            spriteBatch.End();


            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(rt0, new Rectangle(0, 0, w, h), Color.White);
            spriteBatch.Draw(rt1, new Rectangle(w +10, 0, w, h), Color.White);
            spriteBatch.End();


            base.Draw(gameTime);
        }

        public static Texture2D TextureDotCreate(GraphicsDevice device, int r, int g, int b, int a)
        {
            Color[] data = new Color[1];
            data[0] = new Color(r, g, b, a);
            return TextureFromColorArray(device, data, 1, 1);
        }
        public static Texture2D TextureFromColorArray(GraphicsDevice device, Color[] data, int width, int height)
        {
            Texture2D tex = new Texture2D(device, width, height);
            tex.SetData<Color>(data);
            return tex;
        }


    }

}
