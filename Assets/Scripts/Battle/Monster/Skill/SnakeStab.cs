using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class SnakeStab : MonsterSkill
    {
        protected override IEnumerator Skill()
        {
            var skillAttackInfoList = new List<SkillAttackInfo>();
            skillAttackInfoList.Add(new SkillAttackInfo(AttackDirection.Stab, PreDelay));
            skillAttackInfoList.Add(new SkillAttackInfo(AttackDirection.Stab, PreDelay));
            skillAttackInfoList.Add(new SkillAttackInfo(AttackDirection.Stab, PreDelay));
            skillAttackInfoList.Add(new SkillAttackInfo(AttackDirection.UpperSlash, PreDelay * 1.5f));
            skillAttackInfoList.Add(new SkillAttackInfo(AttackDirection.Slash, PreDelay * 1.5f));
            
            var skillDirectionList = new List<AttackDirection>();
            foreach (var skillAttackInfo in skillAttackInfoList)
            {
                skillDirectionList.Add(skillAttackInfo.direction);
            }
            yield return StartCoroutine(ShowSkillDirectionAlarm(skillDirectionList.ToArray()));
            
            PlaySkillAnimation();
            Monster.DefenceDirection = AttackDirection.None;
            yield return new WaitForSeconds(1.0f);
            
            int damage = Mathf.FloorToInt(2 * Monster.DamageMultiple);
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
            skillAnim = Resources.Load<AnimationClip>($"Animations/{Monster.MonsterStat.Name}/Skill1");
        }
    }
}