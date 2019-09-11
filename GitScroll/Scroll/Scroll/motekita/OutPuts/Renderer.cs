using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot.OutPuts
{
    class Renderer
    {
        private ContentManager contentManager;
        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;

        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        private Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();

        private Texture2D texture;

        //コンストラクタ
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="graphics"></param>
        public Renderer(ContentManager content, GraphicsDevice graphics)
        {
            contentManager = content;
            graphicsDevice = graphics;
            spriteBatch = new SpriteBatch(graphicsDevice);


            texture = new Texture2D(graphics, 1, 1);

            texture.SetData<Color>(new Color[] { Color.White });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="filepath"></param>
        public void LoadContent(String assetName, string filepath = "./")
        {
            if (textures.ContainsKey(assetName))
            {
#if DEBUG//デバッグ時のみ実行
                Console.WriteLine(assetName + "はすでに読み込まれています。" +
                    "\nプログラムを確認してください。");
#endif
                
                return;
            }
            textures.Add(assetName, contentManager.Load<Texture2D>(filepath + assetName));
        }

        public void LoadFontContent(String assetName, string filepath = "./")
        {
            fonts.Add(assetName, contentManager.Load<SpriteFont>(filepath + assetName));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Unload()
        {
            textures.Clear();
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
            spriteBatch.Draw(textures[assetName], position, Color.White * alpha);
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
            
            spriteBatch.Draw(textures[assetName], position, rect, color);
        }

        public void DrawTexture(string assetName, Vector2 position,
    float scale , Color color)
        {
            //public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);

            spriteBatch.Draw(textures[assetName], position ,null,color,0,Vector2.Zero,  scale,SpriteEffects.None,0);
        }

        public void DrawTexture(string assetName, Vector2 position,
    float scale, Color color, float radian, Vector2 origin)
        {
            //public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);

            spriteBatch.Draw(textures[assetName], position, null, color, radian, origin, scale, SpriteEffects.None, 0);
        }



        public void DrawTexture(Vector2 position)
        {
            spriteBatch.Draw(texture,position , new Rectangle(-32, -32, 64, 64), Color.Black);
            //spriteBatch.Draw(texture, position, Color.White);

        }
        public void DrawTexture(Vector2 pos, Rectangle scalePos, float alpha)
        {
            spriteBatch.Draw(texture,pos, scalePos, Color.Black * alpha);
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
