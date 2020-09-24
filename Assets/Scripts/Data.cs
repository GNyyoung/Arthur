using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Data
{
    private Data(){}
    private static Data _instance;

    public static Data Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Data();
            }

            return _instance;
        }
    }

    public bool isLoaded { get; private set; } = false;
    public Dictionary<string, JsonMonster> MonsterData { get; private set; } = new Dictionary<string, JsonMonster>();
    public Dictionary<string, JsonStage[]> StageData { get; private set; } = new Dictionary<string, JsonStage[]>();
    public Dictionary<string, JsonMonsterSkill> MonsterSkillData { get; private set; } = new Dictionary<string, JsonMonsterSkill>();
    public Dictionary<string, JsonSword> SwordData { get; private set; } = new Dictionary<string, JsonSword>();

    public void LoadJsonData()
    {
        // 몬스터 데이터 추가
        var jsonMonsterData = JsonLoader.LoadJsonFromClassName<JsonMonster>();
        foreach (var data in jsonMonsterData)
        {
            MonsterData.Add(data.Name, data);
        }
        
        //몬스터 스킬 데이터 추가
        var jsonMonsterSkillData = JsonLoader.LoadJsonFromClassName<JsonMonsterSkill>();
        foreach (var data in jsonMonsterSkillData)
        {
            MonsterSkillData.Add(data.SkillName, data);
        }


        // 스테이지 데이터 추가.
        var jsonStageNameData = JsonLoader.LoadJsonFromClassName<JsonStageName>();
        foreach (var stageName in jsonStageNameData)
        {
            var jsonStageData = JsonLoader.LoadJsonFromClassName<JsonStage>(stageName.Name);
            Debug.Log(stageName.Name);
            StageData.Add(stageName.Name, jsonStageData);
        }
        
        // 검 데이터 추가
        var jsonSwordData = JsonLoader.LoadJsonFromClassName<JsonSword>();
        Debug.Log(jsonSwordData.Length);
        foreach (var sword in jsonSwordData)
        {
            Debug.Log(sword.Name);
            SwordData.Add(sword.Name, sword);
        }

        isLoaded = true;
    }

    public JsonMonster GetMonster(string monsterName)
    {
        if (MonsterData.TryGetValue(monsterName, out var value) == true)
        {
            return value;
        }
        else
        {
            return null;
        }
    }

    public JsonStage[] GetStage(string stageName)
    {
        if (StageData.TryGetValue(stageName, out var value))
        {
            return value;
        }
        else
        {
            return null;
        }
    }

    public JsonMonsterSkill GetMonsterSkill(string skillName)
    {
        if (MonsterSkillData.TryGetValue(skillName, out var value) == true)
        {
            return value;
        }
        else
        {
            return null;
        }
    }

    public JsonSword GetSword(string swordName)
    {
        if (SwordData.TryGetValue(swordName, out var value) == true)
        {
            return value;
        }
        else
        {
            return null;
        }
    }
}
