using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerDefend : PlayerAction
    {
        public AttackDirection defendDirection { get; set; }
        public override void StartAction()
        {
            
        }

        public override void StopAction()
        {
            
        }

        public override PlayerStatus GetStatus()
        {
            return PlayerStatus.Defend;
        }

        public bool IsDefendCondition(AttackDirection direction)
        {
            if (GetDefensibleMonster(direction).Length > 0)
            {
                return true;
            }
            else
            {
                return false;   
            }
        }

        private Monster[] GetDefensibleMonster(AttackDirection direction)
        {
            var hits = Player.GetRaycastHitMonsters(Player.CurrentSword.Length);
            var defensibleMonster = new List<Monster>();
            foreach (var hit in hits)
            {
                var monster = hit.collider.GetComponent<Monster>();
                if (monster.IsCastSkill(direction) == true)
                {
                    defensibleMonster.Add(monster);
                }
            }

            return defensibleMonster.ToArray();
        }

        // 이전 코드.
        // 적 공격을 모두 취소시킴
        // IEnumerator Defend()
        // {
        //     Debug.Log("플레이어 방어");
        //     var defensibleMonster = GetDefensibleMonster(defendDirection);
        //     for (int i = 0; i < defensibleMonster.Length; i++)
        //     {
        //         defensibleMonster[i].StopCurrentStatus();
        //     }
        //     
        //     for (int i = 0; i < 30; i++)
        //     {
        //         yield return new WaitForFixedUpdate();
        //     }
        //     
        //     Player.StopCurrentStatus();
        // }

    }
}