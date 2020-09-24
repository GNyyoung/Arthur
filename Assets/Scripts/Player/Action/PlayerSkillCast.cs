using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class PlayerSkillCast : PlayerAction
    {
        public ICooldownObserver CooldownDisplayObserver { get; private set; }
        
        public PlayerSkill Skill { get; set; }

        public override void StartAction()
        {
            Skill.Active(this);
        }

        public override void StopAction()
        {
            Skill.Inactive();
        }

        public abstract override PlayerStatus GetStatus();

        public void EndSkill()
        {
            Player.StopCurrentStatus();
        }

        public bool IsAvailableSkill()
        {
            if (Skill.IsUsable == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}