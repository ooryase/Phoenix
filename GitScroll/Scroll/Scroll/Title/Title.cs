﻿using System;
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

        }

        public void EffectSet(Renderer renderer)
        {
            effect = renderer.Effects["Title"].Clone();

        }


        public override void Update(GameTime gameTime)
        {

            if (GameSystem.InputContllorer.IsPush(Keys.Down))
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
            }

        }
        public override void Draw(Renderer renderer)
        {
            if(!effectSet)
            {
                EffectSet(parent.Renderer);
                effectSet = true;
            }

            //renderer.Begin(effect);
            renderer.Begin();
            // renderer.DrawTexture("title", new Vector2(500, 200));

            renderer.DrawFont("k8x12LL", "YAKITORI TABETAI", new Vector2(300, 300),Color.Blue);
            

            for (int i = 0; i < selectLists.Count; i++)
            {
                //renderer.DrawFont("k8x12L", selectLists[i].action, new Vector2(400 + i * 100, 400 + i * 100));

                if ((int)selectManu == i)
                {
                    renderer.DrawFont("k8x12L", selectLists[i].action, new Vector2(410 + i * 100, 610 + i * 100), Color.Red);
                }
            }

            //renderer.DrawTexture("ReBarth",Vector2.Zero);

            renderer.End();

        }
    }
}
