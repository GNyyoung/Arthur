using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    private readonly Dictionary<string, JsonMonster> _monsterJsonDataSet = new Dictionary<string, JsonMonster>();
    private readonly Dictionary<string, JsonStage[]> _stageSpawnJsonDataSet = new Dictionary<string, JsonStage[]>();
    private readonly Dictionary<string, JsonStageInfo> _stageInfoJsonDataSet = new Dictionary<string, JsonStageInfo>();
    private readonly Dictionary<string, JsonMonsterSkill> _monsterSkillJsonDataSet = new Dictionary<string, JsonMonsterSkill>();
    private readonly Dictionary<string, JsonSword> _swordJsonDataSet = new Dictionary<string, JsonSword>();
    private readonly Dictionary<string, JsonSwordSkill> _swordSkillJsonDataSet = new Dictionary<string, JsonSwordSkill>();
    private readonly Dictionary<int, int> _levelUpCostDataSet = new Dictionary<int, int>();
    private readonly Dictionary<string, string> textJsonDataSet = new Dictionary<string, string>();

    public void LoadJsonData()
    {
        // 몬스터 데이터 추가
        var jsonMonsterData = JsonLoader.LoadJsonFromClassName<JsonMonster>();
        foreach (var data in jsonMonsterData)
        {
            data.Skills = new[] {data.Skill1, data.Skill2, data.Skill3};
            _monsterJsonDataSet.Add(data.Name, data);
        }
        
        //몬스터 스킬 데이터 추가
        var jsonMonsterSkillData = JsonLoader.LoadJsonFromClassName<JsonMonsterSkill>();
        foreach (var data in jsonMonsterSkillData)
        {
            _monsterSkillJsonDataSet.Add(data.Name, data);
        }

        // 스테이지 데이터 추가.
        var jsonStageInfoData = JsonLoader.LoadJsonFromClassName<JsonStageInfo>();
        foreach (var stageInfo in jsonStageInfoData)
        {
            var jsonStageData = JsonLoader.LoadJsonFromClassName<JsonStage>(stageInfo.Name);
            Debug.Log(stageInfo.Name);
            _stageSpawnJsonDataSet.Add(stageInfo.Name, jsonStageData);
            
            // 스테이지 정보 추가
            _stageInfoJsonDataSet.Add(stageInfo.Name, stageInfo);
        }
        
        // 검 데이터 추가
        var jsonSwordData = JsonLoader.LoadJsonFromClassName<JsonSword>();
        var builder = new StringBuilder();
        builder.Append($"추가된 검 데이터 : {jsonSwordData.Length}개\n");
        foreach (var sword in jsonSwordData)
        {
            builder.Append($"{sword.Name}\n");
            _swordJsonDataSet.Add(sword.Name, sword);
        }
        Debug.Log(builder);
        
        // 검 스킬 데이터 추가
        var jsonSwordSkillData = JsonLoader.LoadJsonFromClassName<JsonSwordSkill>();
        foreach (var swordSkill in jsonSwordSkillData)
        {
            _swordSkillJsonDataSet.Add(swordSkill.Name, swordSkill);
        }
        
        // 레벨업 비용 데이터 추가
        var jsonLevelUpCostData = JsonLoader.LoadJsonFromClassName<JsonLevelUpCost>();
        foreach (var levelUpCost in jsonLevelUpCostData)
        {
            _levelUpCostDataSet.Add(levelUpCost.Level, levelUpCost.Cost);
        }
        
        // 텍스트 추가
        var jsonTextData = JsonLoader.LoadJsonFromClassName<JsonText>();
        foreach (var jsonText in jsonTextData)
        {
            textJsonDataSet.Add(jsonText.ID, jsonText.Text);
        }

        isLoaded = true;
    }

    public JsonMonster GetMonster(string monsterName)
    {
        if (_monsterJsonDataSet.TryGetValue(monsterName, out var value) == true)
        {
            return value;
        }
        else
        {
            return null;
        }
    }

    public JsonStage[] GetStageSpawn(string stageName)
    {
        if (_stageSpawnJsonDataSet.TryGetValue(stageName, out var value))
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
        if (_monsterSkillJsonDataSet.TryGetValue(skillName, out var value) == true)
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
        if (_swordJsonDataSet.TryGetValue(swordName, out var value) == true)
        {
            return value;
        }
        else
        {
            return null;
        }
    }

    public JsonSwordSkill GetSwordSkill(string swordSkillName)
    {
        if (_swordSkillJsonDataSet.TryGetValue(swordSkillName, out var value) == true)
        {
            return value;
        }
        else
        {
            return null;
        }
    }

    public int GetLevelUpCost(int level)
    {
        return _levelUpCostDataSet[level];
    }

    public JsonStageInfo GetStageInfo(string stageName)
    {
        if (_stageInfoJsonDataSet.TryGetValue(stageName, out var stageInfo) == true)
        {
            return stageInfo;
        }
        else
        {
            return null;
        }
    }

    public string GetText(string ID)
    {
        if (textJsonDataSet.TryGetValue(ID, out var text) == true)
        {
            return text;
        }
        else
        {
            return null;
        }
    }

    // 스테이지 제작용으로 만든 메소드
    public void RemoveAllJson()
    {
        isLoaded = false;
        _monsterJsonDataSet.Clear();
        _stageSpawnJsonDataSet.Clear();
        _stageInfoJsonDataSet.Clear();
        _monsterSkillJsonDataSet.Clear();
        _swordJsonDataSet.Clear();
        _swordSkillJsonDataSet.Clear();
        _levelUpCostDataSet.Clear();
    }
}
