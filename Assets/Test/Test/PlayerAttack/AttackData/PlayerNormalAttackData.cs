using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attack;
using UnityEngine;

namespace Test
{
    [CreateAssetMenu(fileName = "PlayerNormalAttack", menuName = "攻击数据/玩家/普通攻击")]
    public class PlayerNormalAttackData : BaseAttackScriptobject
    {
        [Serializable]
        public struct PlayerNormalAttack
        {
            public int Damage;
        }

        public PlayerNormalAttack data = new PlayerNormalAttack();

        protected override IAttackLogic CreateAttackLogic()
        {
            throw new NotImplementedException();
        }
    }
}
