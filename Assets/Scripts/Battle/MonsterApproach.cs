﻿﻿using System.Collections;
using System.Collections.Generic;
  using System.Linq;
  using UnityEngine;

namespace DefaultNamespace
{
    public class MonsterApproach
    {
        private MonsterApproach(){}
        private static MonsterApproach _instance;

        public static MonsterApproach Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MonsterApproach();
                }

                return _instance;
            }
        }
        
        private readonly Stack<GameObject> _approachedNormalStack = new Stack<GameObject>();
        private readonly Stack<GameObject> _approachedBossStack = new Stack<GameObject>();
        
        public GameObject FirstApproachedNormal { private get; set; }
        public GameObject FirstApproachedBoss { private get; set; }
        public bool IsPush { get; set; } = false;
        public const float RETREAT_DELAY = 0.5f;

        /// <summary>
        /// 현재 충돌한 몬스터가 있으면 true를 반환.
        /// </summary>
        public bool IsExistCollide()
        {
            if (FirstApproachedNormal == null &&
                FirstApproachedBoss == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 매개변수로 받은 몬스터를 스택에서 정리함.
        /// </summary>
        public void RemoveApproach(GameObject monsterObject)
        {
            var monsterType = monsterObject.GetComponent<Monster>().MonsterType;
            if (monsterType != MonsterType.Passage)
            {
                var approachedStack = GetApproachedStackWithType(monsterType);
            
                if (approachedStack.Contains(monsterObject))
                {
                    while (approachedStack.Count > 0)
                    {
                        var detachedMonster = approachedStack.Pop();

                        Debug.Log($"몬스터({detachedMonster.GetInstanceID()}) 충돌 해제");
                        detachedMonster.GetComponent<Monster>().isCollided = false;
                    
                        if (detachedMonster.Equals(monsterObject))
                        {
                            break;
                        }
                    }

                    if (approachedStack.Count == 0)
                    {
                        ResetFirstApproach(monsterType);
                    }
                
                    CheckPush();
                }
            }
        }

        public bool IsFirstApproached(GameObject monsterObject)
        {
            var firstApproach = GetFirstApproach(monsterObject.GetComponent<Monster>().MonsterType);
            if (firstApproach != null)
            {
                return firstApproach.Equals(monsterObject);    
            }
            else
            {
                return false;
            }
        }
        
        private Stack<GameObject> GetApproachedStackWithType(MonsterType monsterType)
        {
            Stack<GameObject> approachStack = null;
            
            switch (monsterType)
            {
                case MonsterType.Normal:
                    approachStack = _approachedNormalStack;
                    break;
                case MonsterType.Boss:
                    approachStack = _approachedBossStack;
                    break;
                default:
                    Debug.LogError($"정의되지 않은 MonsterType입니다. : {monsterType}");
                    break;
            }

            return approachStack;
        }
        
        public GameObject GetLastApproachedMonsterWithType(MonsterType monsterType)
        {
            GameObject lastApproachMonster = null;
            
            switch (monsterType)
            {
                case MonsterType.Normal:
                    if (_approachedNormalStack.Count > 0)
                    {
                        lastApproachMonster = _approachedNormalStack.Peek();   
                    }
                    break;
                case MonsterType.Boss:
                    if (_approachedBossStack.Count > 0)
                    {
                        lastApproachMonster = _approachedBossStack.Peek();   
                    }
                    break;
                default:
                    Debug.LogError($"정의되지 않은 MonsterType입니다. : {monsterType}");
                    break;
            }
            return lastApproachMonster;
        }
        
        public void AddApproach(GameObject monsterObject)
        {
            var monster = monsterObject.GetComponent<Monster>();
            switch (monster.MonsterType)
            {
                case MonsterType.Normal:
                    if (_approachedNormalStack.Count == 0)
                    {
                        FirstApproachedNormal = monsterObject;
                    }
                    _approachedNormalStack.Push(monsterObject);
                    Debug.Log($"몬스터({monsterObject.GetInstanceID()}) 충돌 추가");
                    break;
                case MonsterType.Boss:
                    if (_approachedBossStack.Count == 0)
                    {
                        FirstApproachedBoss = monsterObject;
                    }
                    _approachedBossStack.Push(monsterObject);
                    Debug.Log($"보스({monsterObject.GetInstanceID()}) 충돌 추가");
                    break;
                default:
                    Debug.LogError($"정의되지 않은 MonsterType입니다. : {monster}");
                    break;
            }

            if (monster.IsPush == true)
            {
                IsPush = true;
            }
        }

        public void ChangeFirstApproach(GameObject monsterObject)
        {
            switch (monsterObject.GetComponent<Monster>().MonsterType)
            {
                case MonsterType.Normal:
                    FirstApproachedNormal = monsterObject;
                    break;
                case MonsterType.Boss:
                    FirstApproachedBoss = monsterObject;
                    break;
            }
        }

        public void ResetFirstApproach(MonsterType monsterType)
        {
            switch (monsterType)
            {
                case MonsterType.Normal:
                    FirstApproachedNormal = null;
                    break;
                case MonsterType.Boss:
                    FirstApproachedBoss = null;
                    break;
                default:
                    Debug.LogError($"정의되지 않은 MonsterType : {monsterType}");
                    break;
            }
        }

        public GameObject GetFirstApproach(MonsterType monsterType)
        {
            switch (monsterType)
            {
                case MonsterType.Normal:
                    return FirstApproachedNormal;
                case MonsterType.Boss:
                    return FirstApproachedBoss;
                default:
                    Debug.LogError($"정의되지 않은 MonsterType : {monsterType}");
                    return null;
            }
        }

        /// <summary>
        /// 몬스터가 플레이어를 미는 상태인지 확인한다.
        /// </summary>
        private void CheckPush()
        {
            foreach (var monster in _approachedNormalStack)
            {
                if (monster.GetComponent<Monster>().IsPush == true)
                {
                    IsPush = true;
                    return;
                }
            }

            foreach (var monster in _approachedBossStack)
            {
                if (monster.GetComponent<Monster>().IsPush == true)
                {
                    IsPush = true;
                    return;
                }
            }

            IsPush = false;
        }

        public GameObject[] GetAllApproachedMonsters()
        {
            var allApproachedMonsters = new List<GameObject>();
            allApproachedMonsters.AddRange(_approachedNormalStack.ToArray());
            allApproachedMonsters.AddRange(_approachedBossStack.ToArray());

            return allApproachedMonsters.ToArray();
        }

        public void ResetData()
        {
            IsPush = false;
            _approachedBossStack.Clear();
            _approachedNormalStack.Clear();
            FirstApproachedBoss = null;
            FirstApproachedNormal = null;
        }
    }
}