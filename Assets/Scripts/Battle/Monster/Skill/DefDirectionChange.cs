using System;
using System.Collections;

namespace DefaultNamespace
{
    public class DefDirectionChange : MonsterSkill
    {
        protected override IEnumerator Skill()
        {
            // var directions = Enum.GetValues(typeof(AttackDirection));
            // AttackDirection direction;
            // if ((int) Monster.DefenceDirection + 1 < directions.Length)
            // {
            //     direction = Monster.DefenceDirection + 1;
            // }
            // else
            // {
            //     direction = (AttackDirection)directions.GetValue(1);
            // }
            //
            // // Monster.ChangeDefenceDirection(direction);
            // // DefenceVariety와 통합 필요
            // Monster.DefenceDirection = direction;
            // Monster.MonsterData.CurrentDefenceDirection = direction;
            // Monster.transform.Find("Canvas").GetComponent<MonsterDebugUI>().DefenceDirectionText.text = $"Def:{Monster.DefenceDirection.ToString()}";
            // // 코드 끝

            DefenceVariety.NextDirection(Monster);
            
            EndSkill();

            yield return null;
        }

        protected override bool IsPauseCooldown()
        {
            return false;
        }

        protected override void SetSkillAnimation()
        {
            
        }
    }
}