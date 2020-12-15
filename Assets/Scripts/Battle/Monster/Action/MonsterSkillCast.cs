using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class MonsterSkillCast : MonsterAction
    {
        public float ActiveProgress { get; protected set; }
        public MonsterSkill Skill { get; set; }

        public override void StartAction()
        {
            this.enabled = true;
            ActiveProgress = 0;
            Skill.Active();
        }

        public override void StopAction()
        {
            Skill.Inactive();
            ActiveProgress = 0;
            Skill = null;
            this.enabled = false;
        }

        public override void TerminateAction()
        {
            if (Skill != null)
            {
                Skill.Terminate();   
            }
            ActiveProgress = 0;
            Skill = null;
            this.enabled = false;
        }

        public override MonsterStatus GetStatus()
        {
            return MonsterStatus.Skill;
        }

        public void ActiveCurrentSkill()
        {
            Monster.ChangeStatus(this);
        }
        
        public void EndSkill()
        {
            Monster.StopCurrentStatus();
        }
        
        public void AddStandbySkill(MonsterSkill skill)
        {
            Monster.SkillStandbyList.Add(skill);
        }

        public void AttackPlayer(int damage, AttackDirection direction)
        {
            if (Monster.Player.TakeDamage(Monster, damage, direction) == true)
            {
                Skill.UseHitEffect();
            }
        }

        public void AddCharacterEffect(CharacterEffect target, Effect effect, float duration, float bonusRate = 0)
        {
            Debug.Log($"{target.gameObject.name}에게 {effect.ToString()} 부여");
            target.AddEffect(effect, duration, bonusRate);
        }
    }
}