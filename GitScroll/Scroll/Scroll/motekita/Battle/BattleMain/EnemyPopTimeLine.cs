using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot.Battle.BattleMain
{
    class EnemyPopTimeLine
    {
        public Enemy.VirtualEnemy enemy;
        public int nextTime;

        public EnemyPopTimeLine(Enemy.VirtualEnemy e, int t)
        {
            enemy = e;
            nextTime = t;
        }

    }
}
