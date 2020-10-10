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
        public ParticleSystem effectObject;

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
            Monster.Player.TakeDamage(Monster, damage, direction);
        }
    }
}