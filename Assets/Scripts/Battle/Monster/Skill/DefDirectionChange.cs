using System;
using System.Collections;

namespace DefaultNamespace
{
    public class DefDirectionChange : MonsterSkill
    {
        protected override IEnumerator Skill()
        {
            var directions = Enum.GetValues(typeof(AttackDirection));
            AttackDirection direction;
            if ((int) Monster.DefenceDirection + 1 < directions.Length)
            {
                direction = Monster.DefenceDirection + 1;
            }
            else
            {
                direction = (AttackDirection)directions.GetValue(1);
            }
            
            Monster.ChangeDefenceDirection(direction);
            
            EndSkill();

            yield return null;
        }

        protected override bool IsPauseCooldown()
        {
            return true;
        }
    }
}