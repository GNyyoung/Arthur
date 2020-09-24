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
            ActiveProgress = 0;
            Skill.Active();
        }

        public override void StopAction()
        {
            Skill.Inactive();
        }
        public override MonsterStatus GetStatus()
        {
            return MonsterStatus.Skill;
        }
        
        public void EndSkill()
        {
            Monster.StopCurrentStatus();
        }
        
        public void EnqueueSkill(MonsterSkill skill)
        {
            Monster.SkillStandbyQueue.Enqueue(skill);
        }

        public void AttackPlayer(int damage, AttackDirection direction)
        {
            Monster.Player.TakeDamage(Monster, damage, direction);
        }
    }
}