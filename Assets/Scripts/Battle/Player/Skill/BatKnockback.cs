using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class BatKnockback : PlayerSkill
    {
        const float knockbackDist = 2;
        private const float knockbackTime = 0.3f;
        
        protected override void SetSkillAnimation()
        {
            skillAnim = Resources.Load<AnimationClip>("Animations/Player/UpperSwing");
        }

        public override IEnumerator Skill()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            bool isDamage = false;
            
            ApplySkillAnimation();

            float time = 0;

            while (time < skillAnim.length)
            {
                if (isDamage == false &&
                    time > skillAnim.length * 0.7f)
                {
                    var hitMonsters = skillCastAction.GetRaycastHitMonsters(GetTotalRange());
                    foreach (var monsterObject in hitMonsters)
                    {
                        monsterObject.GetComponent<Monster>().TakeDamage(player, GetTotalDamage(), AttackDirection.UpperSlash);
                    }
                    StartCoroutine(KnockbackMonsters(new List<GameObject>(hitMonsters)));
                    isDamage = true;
                }
                
                time += Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }
            
            EndSkill();
        }

        IEnumerator KnockbackMonsters(List<GameObject> hitMonsterList)
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            float time = 0;
            Vector3 moveDistPerFrame = Vector3.right * (knockbackDist * Time.fixedDeltaTime / knockbackTime);

            while (time < knockbackTime)
            {
                for (int i = hitMonsterList.Count - 1; i >= 0; i--)
                {
                    if (hitMonsterList[i] == null)
                    {
                        hitMonsterList.RemoveAt(i);
                    }
                    else
                    {
                        hitMonsterList[i].transform.position += moveDistPerFrame;
                    }
                }

                time += Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }
        }
    }
}