﻿using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class StrongAttack : PlayerSkill
    {
        protected override void SetSkillAnimation()
        {
            skillAnim = Resources.Load<AnimationClip>("Animations/Player/StrongAttack");
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
                    time > skillAnim.length * 0.67f)
                {
                    var hitMonsters = skillCastAction.GetRaycastHitMonsters(GetTotalRange());
                    foreach (var monsterObject in hitMonsters)
                    {
                        monsterObject.GetComponent<Monster>().TakeDamage(player, GetTotalDamage(), AttackDirection.Stab);
                    }
                    isDamage = true;
                }
                
                time += Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }
            
            EndSkill();
        }
    }
}