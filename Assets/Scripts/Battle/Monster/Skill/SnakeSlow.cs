using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class SnakeSlow : MonsterSkill
    {
        protected override IEnumerator Skill()
        {
            PlaySkillAnimation();
            yield return new WaitForSeconds(PreDelay);
            var hitPlayerObj = SkillCastAction.GetRaycastHitPlayer(1);
            if (hitPlayerObj != null)
            {
                Debug.Log(hitPlayerObj.GetInstanceID());
                var playerEffect = hitPlayerObj.GetComponent<Player>().CharacterEffect;
                SkillCastAction.AddCharacterEffect(playerEffect, Effect.Slow, 5.0f, 2f);    
            }
            yield return StartCoroutine(WaitForPostDelay());
            
            EndSkill();
        }

        protected override bool IsPauseCooldown()
        {
            return SkillCooldownCondition.IsPauseCooldownWhenIdle(Monster);
        }

        protected override void SetSkillAnimation()
        {
            skillAnim = Resources.Load<AnimationClip>($"Animations/{Monster.MonsterStat.Name}/Skill3");
        }
    }
}