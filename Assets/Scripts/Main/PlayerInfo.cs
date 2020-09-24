using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Main;
using UnityEngine;

public class PlayerInfo : MonoBehaviour, IBattleInfoProvider
{
    List<SwordInfo> ownedSwordList = new List<SwordInfo>();
    public List<int> EquipSwordIndexList { get; set; } = new List<int>();
    // Start is called before the first frame update

    private void Awake()
    {
        MainSceneManager.Instance.BattleInfoProviderList.Add(this);
        
        for (int i = 0; i < InventoryPanelUI.Instance.equipToggles.Length; i++)
        {
            // 나중에 저장된 데이터로부터 장착한 검 인덱스 가져오기
            EquipSwordIndexList.Add(-1);
        }
    }

    void Start()
    {
        UIInstanceProvider.SendInstance(this);
        
        // 테스트 코드.
        var testSword1 = new SwordInfo();
        testSword1.Initialize("OldSword", 1);
        ownedSwordList.Add(testSword1);
        
        var testSword2 = new SwordInfo();
        testSword2.Initialize("FastSword", 1);
        ownedSwordList.Add(testSword2);
        
        var testSword3 = new SwordInfo();
        testSword3.Initialize("Bat", 1);
        ownedSwordList.Add(testSword3);
        
        var testSword4 = new SwordInfo();
        testSword4.Initialize("Crowbar", 1);
        ownedSwordList.Add(testSword4);
        
        var testSword5 = new SwordInfo();
        testSword5.Initialize("Mop", 1);
        ownedSwordList.Add(testSword5);
        
        var testSword6 = new SwordInfo();
        testSword6.Initialize("Staff", 1);
        ownedSwordList.Add(testSword6);
        
        var testSword7 = new SwordInfo();
        testSword7.Initialize("Staff", 2);
        ownedSwordList.Add(testSword7);
        // 코드 끝.
    }

    public SwordInfo[] GetOwnedSwords()
    {
        var swords = new SwordInfo[ownedSwordList.Count];
        for (int i = 0; i < swords.Length; i++)
        {
            swords[i] = ownedSwordList[i];
        }

        return swords;
    }

    public void EquipSword(int swordIndex, int equipIndex)
    {
        Debug.Log($"{equipIndex}, {swordIndex}");
        EquipSwordIndexList[equipIndex] = swordIndex;
    }

    public void UnEquipSword(int equipIndex)
    {
        Debug.Log($"탈착 : {equipIndex}");
        EquipSwordIndexList[equipIndex] = -1;
    }

    public int FindEquipSwordIndex(int swordIndex)
    {
        if (EquipSwordIndexList.Contains(swordIndex) == true)
        {
            return EquipSwordIndexList.FindIndex(x => x == swordIndex);
        }
        else
        {
            return -1;
        }
    }

    public KeyValuePair<string, object> GetBattleInfo()
    {
        Debug.Log("플레이어 정보 수집");
        var swords = new List<SwordInfo>();
        foreach (var swordIndex in EquipSwordIndexList)
        {
            if (swordIndex >= 0)
            {
                swords.Add(ownedSwordList[swordIndex]);
            }
        }

        var info = new KeyValuePair<string, object>("Sword", swords.ToArray());
        return info;
    }
}
