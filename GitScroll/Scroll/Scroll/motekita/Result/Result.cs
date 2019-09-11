using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Shoot.OutPuts;

namespace Shoot.Result
{
    class Result : Scene
    {
        private int resultScore;
        private int time;

        public Result(GameMain gameMain, int score) : base(gameMain)
        {
            resultScore = score;
            time = 0;
        }



        public override void Update(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.Milliseconds;


            if (GameSystem.InputContllorer.IsPush(Microsoft.Xna.Framework.Input.Keys.Z))
            {
                parent.AddSceneReservation(new Title.Title(parent));
                isClose = true;
            }

        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawFont("k8x12L", "RESULT SCORE", new Vector2(400, 400));
            renderer.DrawFont("k8x12L", resultScore.ToString(), new Vector2(600,400));

        }
    


    }


}
