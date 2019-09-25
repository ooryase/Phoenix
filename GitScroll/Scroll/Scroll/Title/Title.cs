using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Scroll.GameSystem;
using Scroll.Output;

namespace Scroll.Title
{
    class Title : Scene
    {
        private enum Manu
        {
            GAMESTART,
            PRACTICE,
            OPTION,
            EXIT,
            MAX 
        }
        private Manu selectManu;

        private bool effectSet;
        private Effect effect;

        private int time;

        private enum State
        {
            SELECT,
            FALL,
            UP,
            REBIRTH
        }
        private State state;

        //private List<Tuple<Manu, string, string>> selectList;
        private class SelectListItem
        {
            public readonly Manu manu;
            public readonly String action;
            public readonly String description;
            public SelectListItem(Manu m, String s, String d)
            {
                manu = m;
                action = s;
                description = d;
            }
        }
        private List<SelectListItem> selectLists;


        public Title(GameMain gameMain,Renderer renderer,Sound sound)
            : base(gameMain,renderer,sound)
        {
            effectSet = false;
            selectManu = Manu.GAMESTART;

            selectLists = new List<SelectListItem>();

            selectLists.Add(new SelectListItem(Manu.GAMESTART, "Game Start", "キャラクターを選ん(だ気になっ)てゲームを開始します"));
            //selectLists.Add(new SelectListItem(Manu.PRACTICE, "SpellPractice", "敵の譜面の練習をします(未実装)"));
            //selectLists.Add(new SelectListItem(Manu.OPTION, "Option", "ゲームの設定をします(未実装)"));
            //selectLists.Add(new SelectListItem(Manu.EXIT, "Exit", "ゲームを終了します"));

            time = 0;
            state = State.SELECT;
        }

        public void EffectSet(Renderer renderer)
        {
            effect = renderer.Effects["Title"].Clone();

        }


        public override void Update(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.Milliseconds;



            /*if (GameSystem.InputContllorer.IsPush(Keys.Down))
            {
                selectManu = (Manu)(((int)selectManu + 1 ) % (int)Manu.MAX);
            }
            if(GameSystem.InputContllorer.IsPush(Keys.Up))
            {
                selectManu = (Manu)(((int)selectManu - 1 + (int)Manu.MAX) % (int)Manu.MAX);
            }
            if(GameSystem.InputContllorer.IsPush(Buttons.A) || GameSystem.InputContllorer.IsPush(Keys.Z))
            {
                parent.AddSceneReservation(new Battle.Battle(parent,renderer,sound));
                isClose = true;
            }*/

            if(state != State.SELECT &&
                GameSystem.InputContllorer.IsPush(Buttons.A) || GameSystem.InputContllorer.IsPush(Keys.Z))
            {
                parent.AddSceneReservation(new Battle.Battle(parent, renderer, sound));
                isClose = true;
                sound.PlaySE("rebirth");
            }

            switch (state)
            {
                case State.SELECT:
                    if (GameSystem.InputContllorer.IsPush(Buttons.A) || GameSystem.InputContllorer.IsPush(Keys.Z))
                    {
                        state = State.FALL;
                        time = 0;
                    }
                    break;
                case State.FALL:
                    if(time > 3000)
                    {
                        state = State.UP;
                        time = 0;
                    }
                    break;
                case State.UP:
                    if(time > 3000)
                    {
                        state = State.REBIRTH;
                        time = 0;
                        sound.PlaySE("rebirth");
                    }
                    break;
                case State.REBIRTH:
                    if(time > 500)
                    {
                        parent.AddSceneReservation(new Battle.Battle(parent, renderer, sound));
                        isClose = true;
                    }
                    break;
            }

        }
        public override void Draw(Renderer renderer)
        {
            if(!effectSet)
            {
                //EffectSet(parent.Renderer);
                sound.PlayBGM("titlebgm");

                effectSet = true;
            }

            //renderer.Begin(effect);
            renderer.Begin();
            // renderer.DrawTexture("title", new Vector2(500, 200));
            if(state == State.UP)
                renderer.DrawTexture("Title", Vector2.Zero, 1.6f,Color.White * ( 1f - time / 3000f));
            else if (state != State.REBIRTH)
                renderer.DrawTexture("Title", Vector2.Zero, 1.6f, Color.White);

            //renderer.DrawFont("k8x12LL", "YAKITORI TABETAI", new Vector2(300, 300),Color.Blue);


            /*for (int i = 0; i < selectLists.Count; i++)
            {
                //renderer.DrawFont("k8x12L", selectLists[i].action, new Vector2(400 + i * 100, 400 + i * 100));

                if ((int)selectManu == i)
                {
                    renderer.DrawFont("font48", selectLists[i].action, new Vector2(410 + i * 100, 710 + i * 100), Color.Red);
                }
            }*/

            switch (state)
            {
                case State.SELECT:
                    renderer.DrawFont("font48", selectLists[0].action, new Vector2(210, 710), Color.Red);
                    renderer.DrawTexture("phoenix",new Vector2(490f,610f),new Rectangle((time / 200 % 5) * 300,0,300,300),Color.White);
                    break;
                case State.FALL:
                    if (time < 2000)
                        renderer.DrawTexture("phoenix", new Vector2(490f, 610f + time / 40f), new Rectangle((time / 200 % 10) * 300, 600, 300, 300), Color.White);
                    else
                        renderer.DrawTexture("phoenix", new Vector2(490f, 660f), new Rectangle(2700, 600, 300, 300), Color.White);
                    break;
                case State.UP:
                    if (time < 2000)
                        renderer.DrawTexture("phoenix", new Vector2(490f, 660f - time / 8f), new Rectangle((time / 200 % 10) * 300, 900, 300, 300), Color.White);
                    else
                        renderer.DrawTexture("phoenix", new Vector2(490f, 410f), new Rectangle(2700, 900, 300, 300), Color.White);
                    break;
                case State.REBIRTH:
                    renderer.DrawTexture("phoenix", new Vector2(490f, 410f), new Rectangle(2700, 900, 300, 300), Color.White);
                    renderer.DrawTexture("ReBarth", new Vector2(490f + 150f - time / 80f * 160f
                        , 410f + 150f - time / 80f * 150f ),time / 80f , Color.White);
                    if(time > 250)
                        renderer.DrawTexture(Vector2.Zero, new Rectangle(0, 0, 1280, 960), (time - 250f) / 250f);

                    break;
            }


            //renderer.DrawTexture("ReBarth",Vector2.Zero);

            renderer.End();

        }
    }
}
