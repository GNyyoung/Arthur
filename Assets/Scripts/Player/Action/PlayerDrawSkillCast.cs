﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerDrawSkillCast : PlayerSkillCast
    {
        private ICooldownObserver[] cooldownDisplay;

        // private void Start()
        // {
        //     GameObject[] cooldownDisplayObject = GameUI.Instance.drawSkillCooldown;
        //     cooldownDisplay = new ICooldownObserver[cooldownDisplayObject.Length];
        //     for (int i = 0; i < GameUI.Instance.drawSkillCooldown.Length; i++)
        //     {
        //         cooldownDisplay[i] = cooldownDisplayObject[i].GetComponent<DrawSkillDisplay>();
        //     }
        // }

        public override void InitializeAction(Player player)
        {
            base.InitializeAction(player);
            GameObject[] cooldownDisplayObject = GameUI.Instance.drawSkillCooldown;
            cooldownDisplay = new ICooldownObserver[cooldownDisplayObject.Length];
            for (int i = 0; i < GameUI.Instance.drawSkillCooldown.Length; i++)
            {
                cooldownDisplay[i] = cooldownDisplayObject[i].GetComponent<DrawSkillDisplay>();
            }
        }

        public override PlayerStatus GetStatus()
        {
            return PlayerStatus.DrawSkill;
        }

        public void CooldownReserveSword(LinkedListNode<Sword> currentSwordNode)
        {
            LinkedListNode<Sword> reserveSwordNode = currentSwordNode;
            for (int i = 1; i < currentSwordNode.List.Count; i++)
            {
                reserveSwordNode = reserveSwordNode.Next;
                reserveSwordNode.Value.DrawSkill.StartCooldown();
                cooldownDisplay[i - 1].DisplayCooldown(reserveSwordNode.Value);
            }
        }

        public void UpdateSkillCooldown(LinkedListNode<Sword> currentSwordNode)
        {
            // 이전 무기 쿨타임 돌려주기
            var previousSword = (currentSwordNode.Previous ?? currentSwordNode.List.Last).Value;
            Debug.Log($"교체된 무기 : {previousSword.gameObject.name}");
            previousSword.DrawSkill.StartCooldown();
            // 쿨타임 디스플레이 새로 넣어주기
            var nextNode = currentSwordNode;
            foreach (var display in cooldownDisplay)
            {
                nextNode = (nextNode.Next ?? currentSwordNode.List.First);
                if (nextNode.Equals(currentSwordNode) == true)
                {
                    break;
                }
                display.DisplayCooldown(nextNode.Value);
            }
        }
    }
}