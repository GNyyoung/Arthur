using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class GameUI : MonoBehaviour, IInstanceReceiver
{
    private GameUI(){}
    private static GameUI _instance;

    public static GameUI Instance
    {
        get
        {
            if (_instance == null)
            {
                GameUI[] instances = FindObjectsOfType<GameUI>();
                if (instances.Length == 0)
                {
                    GameUI newInstance = GameObject.Find("Canvas").AddComponent<GameUI>();
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
    public GameObject[] attackButton;
    public GameObject[] drawSkillCooldown;
    public GameObject activeSkillButton;

    private void Awake()
    {
        Instance.enabled = true;
        UIInstanceProvider.UIList.Add(this);
    }

    public void OnclickVertAttack()
    {
        _player.DoSwordAction(AttackDirection.Vertical);
    }

    public void OnClickHorzAttack()
    {
        _player.DoSwordAction(AttackDirection.Horizontal);
    }

    public void OnclickPierceAttack()
    {
        _player.DoSwordAction(AttackDirection.Pierce);
    }

    public void OnClickSkill()
    {
        _player.ActiveSwordSkill();
    }

    public void OnClickWeaponChange()
    {
        _player.ChangeCurrentSword();
    }

    public void SetInstance(object obj)
    {
        if (obj.GetType() == typeof(Player))
        {
            _player = obj as Player;
        }
    }
}
