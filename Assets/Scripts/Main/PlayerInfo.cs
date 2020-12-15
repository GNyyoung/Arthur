using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Main;
using Main;
using UnityEngine;

struct Equipments
{
    public Equipments(List<SwordInfo> ownedSwordList, List<int> equippedSwordIndexList)
    {
        this.ownedSwordList = ownedSwordList;
        this.equippedSwordIndexList = equippedSwordIndexList;
    }
    
    public List<SwordInfo> ownedSwordList;
    public List<int> equippedSwordIndexList;
}
public class PlayerInfo : MonoBehaviour, IInfoProvider
{
    List<SwordInfo> ownedSwordList = new List<SwordInfo>();
    /// <summary>
    /// 각 번호 별 현재 장착한 무기의 인덱스. 장착하지 않은 번호는 -1로 표시됩니다.
    /// </summary>
    public List<int> EquipSwordIndexList { get; set; } = new List<int>();
    private Equipments equipments;
    

    private void Awake()
    {
        AddProvider();

        if (InformationReceiver.Instance.InformationDictionary.ContainsKey("Equipment"))
        {
            equipments = (Equipments)InformationReceiver.Instance.InformationDictionary["Equipment"];
            for (int i = 0; i < equipments.equippedSwordIndexList.Count; i++)
            {
                EquipSwordIndexList.Add(equipments.equippedSwordIndexList[i]);
            }
        }
        else
        {
            for (int i = 0; i < InventoryPanelUI.Instance.equipToggles.Length; i++)
            {
                // 나중에 저장된 데이터로부터 장착한 검 인덱스 가져오기
                EquipSwordIndexList.Add(-1);
            }   
        }
    }

    void Start()
    {
        InstanceProvider.SendInstance(this);

        if (equipments.ownedSwordList == null)
        {
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
            testSword4.Initialize("Hammer", 1);
            ownedSwordList.Add(testSword4);
        
            var testSword6 = new SwordInfo();
            testSword6.Initialize("Staff", 1);
            ownedSwordList.Add(testSword6);
        
            var testSword7 = new SwordInfo();
            testSword7.Initialize("Swordfish", 1);
            ownedSwordList.Add(testSword7);
            // 코드 끝.
        }
        else
        {
            ownedSwordList = equipments.ownedSwordList;
        }
    }

    public SwordInfo GetSword(int swordIndex)
    {
        return ownedSwordList[swordIndex];
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
        Debug.Log($"{equipIndex}번 칸에 {swordIndex} 장착");
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

    public void AddProvider()
    {
        MainSceneManager.Instance.AddBattleInfoProvider(this);
    }

    public KeyValuePair<string, object>[] GetInfo()
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
        
        var equipmentSave = new Equipments(ownedSwordList, EquipSwordIndexList);

        var info = new[]
        {
            new KeyValuePair<string, object>("Sword", swords.ToArray()),
            new KeyValuePair<string, object>("Equipment", equipmentSave), 
        };
        return info;
    }

    public void LevelUpWeapon(int swordIndex)
    {
        if (MoneyManager.Instance.SpendGold(Data.Instance.GetLevelUpCost(ownedSwordList[swordIndex].Level + 1)) == true)
        {
            ownedSwordList[swordIndex].LevelUp();
            InventoryPanelUI.Instance.ShowSwordInfo(swordIndex);
            MainSound.Instance.OutPutSwordLevelUp();
        }
    }
}
