using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerActiveSkillCast : PlayerSkillCast
    {
        private ActiveSkillDisplay cooldownDisplay;

        public override void InitializeAction(Player player)
        {
            base.InitializeAction(player);
            cooldownDisplay = BattleUI.Instance.activeSkillButton.GetComponent<ActiveSkillDisplay>();
        }

        public override PlayerStatus GetStatus()
        {
            return PlayerStatus.SkillActive;
        }

        public override void StartAction()
        {
            base.StartAction();
            Player.CurrentSword.ActiveSkill.StartCooldown();
            UpdateSkillCooldown();
        }

        public void CooldownAllSword(IEnumerable<Sword> equippedSwords)
        {
            foreach (var sword in equippedSwords.
                Where(sword => sword.ActiveSkill != null))
            {
                sword.ActiveSkill.StartCooldown();
            }
            UpdateSkillCooldown();
        }
        public void UpdateSkillCooldown()
        {
            // 현재 무기 쿨타임 보여주기
            cooldownDisplay.DisplayCooldown(Player.CurrentSword);
        }
    }
}