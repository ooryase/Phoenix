using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scroll.Output
{
    class Renderer
    {
        private ContentManager contentManager;
        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;

        private Dictionary<string, Texture2D> textures;
        private Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();
        private Dictionary<string, Effect> effects;

        private Texture2D texture;

        public GraphicsDevice GraphicsDevice { get => graphicsDevice; private set => graphicsDevice = value; }
        public Dictionary<string, Texture2D> Textures { get => textures; private set => textures = value; }
        public Dictionary<string, Effect> Effects { get => effects; private set => effects = value; }

        //コンストラクタ
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="graphics"></param>
        public Renderer(ContentManager content, GraphicsDevice graphics)
        {
            contentManager = content;
            GraphicsDevice = graphics;
            spriteBatch = new SpriteBatch(GraphicsDevice);


            texture = new Texture2D(graphics, 1, 1);

            texture.SetData<Color>(new Color[] { Color.White });

            textures = new Dictionary<string, Texture2D>();
            fonts = new Dictionary<string, SpriteFont>();
            Effects = new Dictionary<string, Effect>();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="filepath"></param>
        public void LoadContent(String assetName, string filepath = "./")
        {
            Textures.Add(assetName, contentManager.Load<Texture2D>(filepath + assetName));
        }

        public void LoadFontContent(String assetName, string filepath = "./")
        {
            fonts.Add(assetName, contentManager.Load<SpriteFont>(filepath + assetName));
        }

        public void LoadEffectContent(String assetName, string filepath = "./")
        {
            effects.Add(assetName, contentManager.Load<Effect>(filepath + assetName));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Unload()
        {
            Textures.Clear();
        }

        /// <summary>
        /// 描画開始
        /// </summary>
        public void Begin()
        {
            spriteBatch.Begin();
        }

        /// <summary>
        /// 描画終了
        /// </summary>
        public void End()
        {
            spriteBatch.End();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="position"></param>
        /// <param name="alpha"></param>
        public void DrawTexture(string assetName, Vector2 position, float alpha = 1.0f)
        {
            spriteBatch.Draw(Textures[assetName], position, Color.White * alpha);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="position"></param>
        /// <param name="rect"></param>
        /// <param name="alpha"></param>
        public void DrawTexture(string assetName, Vector2 position,
            Rectangle rect, Color color)
        {
            
            spriteBatch.Draw(Textures[assetName], position, rect, color);
        }

        public void DrawTexture(string assetName, Vector2 position,
    float scale , Color color)
        {
            //public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);

            spriteBatch.Draw(Textures[assetName], position ,null,color,0,Vector2.Zero,  scale,SpriteEffects.None,0);
        }

        public void DrawTexture(string assetName, Vector2 position,
    float scale, Color color, float radian, Vector2 origin)
        {
            //public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);

            spriteBatch.Draw(Textures[assetName], position, null, color, radian, origin, scale, SpriteEffects.None, 0);
        }



        public void DrawTexture(Vector2 position)
        {
            spriteBatch.Draw(texture,position , new Rectangle(-32, -32, 64, 64), Color.Black);
            //spriteBatch.Draw(texture, position, Color.White);

        }

        public void DrawTexture(Vector2 pos, Rectangle scalePos, float alpha)
        {
            spriteBatch.Draw(texture,pos, scalePos, Color.White * alpha);
        }


        public void DrawTextureLarge(Vector2 position)
        {
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, 128, 128), Color.Black);
            //spriteBatch.Draw(texture, position, Color.White);

        }



        public void DrawFont(String fontName,String text, Vector2 pos)
        {
            spriteBatch.DrawString(fonts[fontName], text, pos, Color.Black);
        }

        public void DrawFont(String fontName, String text, Vector2 pos, Color color)
        {
            spriteBatch.DrawString(fonts[fontName], text, pos, color);
        }
    }
}
