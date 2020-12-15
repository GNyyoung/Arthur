using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public delegate void OnClickAction();
    public class BattleUI : MonoBehaviour, IInstanceReceiver
    {
        private BattleUI(){}
        private static BattleUI _instance;
    
        public static BattleUI Instance
        {
            get
            {
                if (_instance == null)
                {
                    BattleUI[] instances = FindObjectsOfType<BattleUI>();
                    if (instances.Length == 0)
                    {
                        BattleUI newInstance = GameObject.Find("Canvas").AddComponent<BattleUI>();
                        _instance = newInstance;
                    }
                    else if (instances.Length >= 1)
                    {
                        for (int i = 1; i > instances.Length; i++)
                        {
                            Destroy(instances[i]);
                        }
    
                        _instance = instances[0];
                    }
                }
    
                return _instance;
            }
        }
        private Player _player;
        public GameObject[] attackButtons;
        public GameObject[] drawSkillCooldown;
        public GameObject activeSkillButton;
        public GameObject drawButton;
        public DurabilityRemainDisplay durabilityRemainDisplay;
    
        private void Awake()
        {
            UINavigation.baseTransform = gameObject.transform;
            Instance.enabled = true;
            InstanceProvider.ReceiverList.Add(this);
        }
    
        public void OnclickSlash()
        {
            _player.DoAction(InputActionType.Slash);
        }
    
        public void OnClickUpperSlash()
        {
            _player.DoAction(InputActionType.UpperSlash);
        }
    
        public void OnclickStab()
        {
            _player.DoAction(InputActionType.Stab);
        }
    
        public void OnClickSkill()
        {
            _player.DoAction(InputActionType.Skill);
        }
    
        public void OnClickWeaponChange()
        {
            _player.DoAction(InputActionType.Draw);
        }
    
        public void SetInstance(object obj)
        {
            if (obj.GetType() == typeof(Player))
            {
                _player = obj as Player;
            }
        }

        public void OpenOption()
        {
            UINavigation.Push("BattleOption");
        }

        public void BlockAllButtons()
        {
            foreach (var attackButton in attackButtons)
            {
                attackButton.GetComponent<Button>().interactable = false;
            }

            activeSkillButton.GetComponent<Button>().interactable = false;
            drawButton.GetComponent<Button>().interactable = false;
        }
    }
}
