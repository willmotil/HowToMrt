using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ExampleMultipleRenderTargetsDx
{
    public class Game1DrawPrimitivesMrtDx : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        Texture2D texture;
        Effect mrtEffect;
        RenderTarget2D rt0, rt1;
        RenderTargetBinding[] renderTargetBinding = new RenderTargetBinding[2];
        Matrix camera, projection, viewprojection;
        Vector3 scollPositionOffset = new Vector3(0, 0, 0);

        public Game1DrawPrimitivesMrtDx()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Window.Title = "MRT Dx  DrawUserPrimitives";
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = TextureDotCreate(GraphicsDevice, 128, 255, 255, 255); // aqua texture
            mrtEffect = Content.Load<Effect>("MrtEffect");
            rt0 = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.None);
            rt1 = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.None);
            renderTargetBinding[0] = rt0;
            renderTargetBinding[1] = rt1;
        }

        public void Get2DSbCameraProjectionGL(GraphicsDevice device, Vector3 scrollposition , out Matrix camera, out Matrix projection)
        {
            camera = Matrix.CreateWorld(scrollposition, new Vector3(scrollposition.X, scrollposition.Y, 1), Vector3.Down);
            projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width,  GraphicsDevice.Viewport.Height, 0, 0, 1); //gl

            //camera = Matrix.CreateWorld(scollPositionOffset, new Vector3(0, 0, 1), Vector3.Down);  // or -1 or up i forget im not adding this part this gets Matrix.Invert(camera) to make the view.
            //projection = Matrix.CreateScale(1, -1, 1) * Matrix.CreateOrthographicOffCenter(0, w, h, 0, 0, 1); // maybe i forget
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Get2DSbCameraProjectionGL(GraphicsDevice, scollPositionOffset, out camera, out projection);
            viewprojection = projection;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            mrtEffect.Parameters["ViewProjection"].SetValue(viewprojection);

            GraphicsDevice.SetRenderTargets(renderTargetBinding);
            GraphicsDevice.Clear(Color.DarkBlue);
            mrtEffect.Parameters["DiffuseTexture"].SetValue(texture);
            DrawUserColoredRectangle( new Rectangle(100, 100, 200, 200), Color.Aqua, mrtEffect);
            DrawUserColoredRectangle( new Rectangle(150, 150, 200, 200), Color.Red, mrtEffect);


            // draw the rendertargets to screen.
            var hw = GraphicsDevice.Viewport.Width / 2 - 20;
            var hh = GraphicsDevice.Viewport.Height / 2 - 20;

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(rt0, new Rectangle(10, 10, hw, hh), Color.White);
            spriteBatch.Draw(rt1, new Rectangle(hw + 20, 10, hw, hh), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void DrawUserColoredRectangle(Rectangle r, Color color, Effect effect)
        {
            VertexPositionColorTexture[] quad = new VertexPositionColorTexture[6];
            quad[0] = new VertexPositionColorTexture(new Vector3(r.Left, r.Top, 0f), color, new Vector2(0f, 0f));
            quad[1] = new VertexPositionColorTexture(new Vector3(r.Left, r.Bottom, 0f), color, new Vector2(0f, 1f));
            quad[2] = new VertexPositionColorTexture(new Vector3(r.Right, r.Bottom, 0f), color, new Vector2(1f, 1f));
            quad[3] = new VertexPositionColorTexture(new Vector3(r.Right, r.Top, 0f), color, new Vector2(1f, 0f));

            short[] indices = new short[6];
            if (GraphicsDevice.RasterizerState == RasterizerState.CullClockwise)
            {
                indices[0] = 0; indices[1] = 1; indices[2] = 2;
                indices[3] = 2; indices[4] = 3; indices[5] = 0;
            }
            else
            {
                indices[0] = 0; indices[1] = 2; indices[2] = 1;
                indices[3] = 2; indices[4] = 0; indices[5] = 3;
            }

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, quad, 0, 4, indices, 0, 2);
            }
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
