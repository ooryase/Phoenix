﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Scroll.Output;
using System.Collections.Generic;

namespace Scroll.GameSystem
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameMain : Game
    {
        GraphicsDeviceManager graphics;

        private Output.Renderer renderer;
        Sound sound;

        private List<Scene> scenes;
        private List<Scene> addScenes;

        internal Renderer Renderer { get => renderer; private set => renderer = value; }

        public GameMain()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            scenes = new List<Scene>();
            addScenes = new List<Scene>();

            graphics.PreferredBackBufferWidth = 1280; 
            graphics.PreferredBackBufferHeight = 960;


            InputContllorer.Init();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            renderer = new Output.Renderer(Content, GraphicsDevice);
            //scenes.Add(new Battle.BattleMain.Battle(this));
            sound = new Sound(Content);
            scenes.Add(new Title.Title(this,renderer,sound));



            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            renderer.LoadContent("Back", "Textures/");
            renderer.LoadContent("phoenix", "Textures/");
            renderer.LoadContent("wata", "Textures/");
            renderer.LoadContent("flog", "textures/");
            renderer.LoadContent("Wyvern", "Textures/");
            renderer.LoadContent("f", "Textures/");
            renderer.LoadContent("Maptip", "Textures/");
            renderer.LoadContent("FireEffect", "Textures/");
            renderer.LoadContent("FireEffect2", "Textures/");
            renderer.LoadContent("FireEffect3", "Textures/");
            renderer.LoadContent("ReBarth", "Textures/");
            renderer.LoadContent("Gauge", "Textures/");
            renderer.LoadContent("setumei", "Textures/");
            renderer.LoadContent("Title", "Textures/");

            renderer.LoadFontContent("k8x12L");
            renderer.LoadFontContent("k8x12LL","Fonts/");
            renderer.LoadFontContent("Bauhaus93", "Fonts/");
            renderer.LoadFontContent("minifont", "Fonts/");
            renderer.LoadFontContent("font48", "Fonts/");


            renderer.LoadEffectContent("Player", "Sheader/");
            renderer.LoadEffectContent("NoobEnemy", "Sheader/");
            renderer.LoadEffectContent("BackGraund", "Sheader/");
            renderer.LoadEffectContent("Arts", "Sheader/");
            renderer.LoadEffectContent("Block", "Sheader/");
            renderer.LoadEffectContent("AttackFire", "Sheader/");
            renderer.LoadEffectContent("DeathFire", "Sheader/");
            renderer.LoadEffectContent("ReBarthFire", "Sheader/");
            renderer.LoadEffectContent("Title", "Sheader/");

            sound.LoadSE("attack", "Sound/");
            sound.LoadBGM("bgm","Sound/");
            sound.LoadSE("charge2","Sound/");
            sound.LoadSE("rebirth","Sound/");
            sound.LoadBGM("titlebgm", "Sound/");

            /*RenderTarget2D m_renderTarget;
            m_renderTarget = new RenderTarget2D(GraphicsDevice,
            graphics.PreferredBackBufferWidth,
            graphics.PreferredBackBufferHeight,
            false,
            SurfaceFormat.Color,
            DepthFormat.Depth16);

            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1.0f, 0);

            GraphicsDevice.SetRenderTarget(m_renderTarget);
            GraphicsDevice.Clear(Color.Transparent);

            DrawContent();

            GraphicsDevice.SetRenderTarget(null);

            renderer.spriteBatch.Begin();
            // 保存されているゲーム画面を描画する
            renderer.spriteBatch.Draw(m_renderTarget, Vector2.Zero, Color.White);
            renderer.spriteBatch.End();*/
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            GameSystem.InputContllorer.InputUpdate();

            scenes.ForEach(s => s.Update(gameTime));

            scenes.RemoveAll(s => s.IsClose());
            AddScene();

            base.Update(gameTime);

            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.GhostWhite);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            scenes.ForEach(s => s.Draw(renderer));

            base.Draw(gameTime);
        }

        internal void AddSceneReservation(Scene scene)
        {
            addScenes.Add(scene);
        }

        private void AddScene()
        {
            if(addScenes.Count == 0)
                return;

            addScenes.ForEach(ascenes => scenes.Add(ascenes));
            addScenes.Clear();
        }
    }
}
