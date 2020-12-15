using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    // 적이 스킬을 사용할 때 클릭한 버튼 방향이 적 스킬 방향과 일치하면 방어상태로 들어감.
    // 적이 스킬을 사용하면 스킬 무력화하고 방어 해제함.
    public class PlayerDefend : PlayerAction
    {
        public AttackDirection defendDirection { get; set; }

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
            var hitMonsters = GetRaycastHitMonsters(Player.CurrentSword.Length);
            var defensibleMonster = new List<Monster>();
            foreach (var hit in hitMonsters)
            {
                var monster = hit.GetComponent<Monster>();
                if (monster.IsCastSkill(direction) == true)
                {
                    defensibleMonster.Add(monster);
                }
            }

            return defensibleMonster.ToArray();
        }
        
        public void SucceedDefence()
        {
            Debug.Log("방어 성공");
            Player.Animator.SetTrigger("DefendFinish");
            Player.StopCurrentStatus();
        }
    }
}