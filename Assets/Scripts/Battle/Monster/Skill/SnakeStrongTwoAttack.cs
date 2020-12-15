using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class SnakeStrongTwoAttack : MonsterSkill
    {
        protected override IEnumerator Skill()
        {
            var skillAttackInfoList = new List<SkillAttackInfo>();
            skillAttackInfoList.Add(new SkillAttackInfo(AttackDirection.Slash, PreDelay));
            skillAttackInfoList.Add(new SkillAttackInfo(AttackDirection.Stab, PreDelay));
            
            var skillDirectionList = new List<AttackDirection>();
            foreach (var skillAttackInfo in skillAttackInfoList)
            {
                skillDirectionList.Add(skillAttackInfo.direction);
            }
            yield return StartCoroutine(ShowSkillDirectionAlarm(skillDirectionList.ToArray()));
            
            PlaySkillAnimation();
            Monster.DefenceDirection = AttackDirection.None;
            yield return new WaitForSeconds(0.5f);
            
            int damage = Mathf.FloorToInt(5 * Monster.DamageMultiple);
            yield return attackCoroutine = StartCoroutine(AttackPlayer(skillAttackInfoList, damage));

            yield return StartCoroutine(WaitForPostDelay());
            
            EndSkill();
        }

        protected override bool IsPauseCooldown()
        {
            return SkillCooldownCondition.IsPauseCooldownWhenIdle(Monster);
        }

        protected override void SetSkillAnimation()
        {
            skillAnim = Resources.Load<AnimationClip>($"Animations/{Monster.MonsterStat.Name}/Skill2");
        }
    }
}