using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Shoot.OutPuts;

namespace Shoot.Title
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


        public Title(GameMain gameMain) : base(gameMain)
        {

            selectManu = Manu.GAMESTART;

            selectLists = new List<SelectListItem>();

            selectLists.Add(new SelectListItem(Manu.GAMESTART, "Game Start", "キャラクターを選ん(だ気になっ)てゲームを開始します"));
            selectLists.Add(new SelectListItem(Manu.PRACTICE, "SpellPractice", "敵の譜面の練習をします(未実装)"));
            selectLists.Add(new SelectListItem(Manu.OPTION, "Option", "ゲームの設定をします(未実装)"));
            selectLists.Add(new SelectListItem(Manu.EXIT, "Exit", "ゲームを終了します"));

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
            if(GameSystem.InputContllorer.IsPush(Keys.Z))
            {
                parent.AddSceneReservation(new Battle.BattleMain.Battle(parent));
                isClose = true;
            }

        }
        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture("title", new Vector2(500, 200));

            for (int i = 0; i < selectLists.Count; i++)
            {
                renderer.DrawFont("k8x12L", selectLists[i].action, new Vector2(400 + i * 100, 400 + i * 100));

                if ((int)selectManu == i)
                {
                    renderer.DrawFont("k8x12L", selectLists[i].action, new Vector2(410 + i * 100, 410 + i * 100), Color.Red);
                }
            }



        }
    }
}
