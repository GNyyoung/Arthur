﻿﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DefaultNamespace
{
    [System.Serializable]
    public class JsonMonster
    {
        public string Name;
        public string Type;
        public int HP;
        public float SwordDamageRate;
        public string Skill1;
        public string Skill2;
        public string Skill3;
        public string[] Skills;
        public bool IsPush;
        public string DefenceType;
        public string ImagePath;
        public string ToolPath;
    }

    [System.Serializable]
    public class JsonStage
    {
        public string MonsterName;
        public float SpawnTime;
        public bool IsStopSpawn = false;
    }

    [System.Serializable]
    public class JsonMonsterSkill
    {
        public string Name;
        public float FirstCooldown;
        public float Cooldown;
        public float PreDelay;
        public float PostDelay;
        public string ActiveType;
    }

    [System.Serializable]
    public class JsonStageInfo
    {
        public string Name;
        public float DamageMultiple;
        public float HPMultiple;
        public float CooldownMultiple;
    }

    [System.Serializable]
    public class JsonSword
    {
        public string Name;
        public string Desc;
        public int Damage;
        public float AttackCooldown;
        public float DamageTime;
        public int Durability;
        public int Length;
        public string ActiveSkill;
        public string DrawSkill;
        public string ImagePath;
    }

    [System.Serializable]
    public class JsonSwordSkill
    {
        public string Name;
        public string Type;
        public float AnimationTime;
        public int DurabilityCost;
        public float Cooldown;
        public float BaseBonus;
        public float LevelBonus;
        public int RangeBonus;
        public int RangeLevelBonus;
        public float Duration;
        public float DurationLevelBonus;
    }

    [System.Serializable]
    public class JsonLevelUpCost
    {
        public int Level;
        public int Cost;
    }

    [System.Serializable]
    public class JsonText
    {
        public string ID;
        public string Text;
    }

    public class JsonWrapper<T>
    {
        public T[] wrapper;
    }
    
    public static class JsonLoader
    {
        public static T[] LoadJsonFromClassName<T>()
        {
            T[] jsonClass = default;
            var classType = typeof(T).Name;
            string fileName = null;
            
            if (classType.Length > 4 && classType.Substring(0, 4).Equals("Json") == true)
            {
                fileName = classType.Substring(4);
                jsonClass = LoadJsonFromClassName<T>(fileName);

                return jsonClass;
            }
            else
            {
                Debug.LogWarning($"Json을 받을 수 없는 타입입니다. : {classType}");
                return null;
            }
        }

        public static T[] LoadJsonFromClassName<T>(string fileName)
        {
            T[] jsonClass = default;
            var jsonPath = $"Datas/{fileName}";
            try
            {
                var jsonString = Resources.Load(jsonPath) as TextAsset;
                jsonClass = JsonUtility.FromJson<JsonWrapper<T>>("{\"wrapper\":" + jsonString.text + "}").wrapper;
            }
            catch
            {
                Debug.LogWarning($"존재하지 않는 json 파일명 : {fileName}\n{jsonPath}");
            }
            
            return jsonClass;
        }
    }    
}

