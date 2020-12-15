using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class ControlDebug : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                BattleUI.Instance.OnclickSlash();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                BattleUI.Instance.OnClickUpperSlash();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                BattleUI.Instance.OnclickStab();
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                BattleUI.Instance.OnClickWeaponChange();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                BattleUI.Instance.OnClickSkill();
            }
        }
    }
}