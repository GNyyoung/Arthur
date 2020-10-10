﻿﻿using System.Collections;
using System.Collections.Generic;
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
        
        public GameObject FirstApproachedMonster { private get; set; }
        private Stack<GameObject> _approachedNormalStack = new Stack<GameObject>();
        private Stack<GameObject> _approachedBossStack = new Stack<GameObject>();
        public bool IsPush { get; set; } = false;

        public bool IsExistCollide()
        {
            if (FirstApproachedMonster != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 매개변수로 받은 몬스터가 플레이어와 마지막으로 충돌했는지를 반환
        /// </summary>
        public bool IsLastApproachedMonster(GameObject monsterObject)
        {
            var monster = monsterObject.GetComponent<Monster>();
            Stack<GameObject> approachStack = GetApproachedStackWithType(monster.MonsterType);
            
            if (approachStack.Count > 0 &&
                approachStack.Peek().Equals(monsterObject))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 매개변수로 받은 몬스터를 스택에서 정리함.
        /// </summary>
        public void UpdateApproachStatus(GameObject monsterObject)
        {
            if (FirstApproachedMonster != null)
            {
                if (FirstApproachedMonster.Equals(monsterObject) == true)
                {
                    FirstApproachedMonster = null;
                    _approachedNormalStack = new Stack<GameObject>();
                }
                else
                {
                    // 스택 가운데에 있는 몬스터들은 충돌 확인 때 사용하지 않으므로 스택에서 제거하지 않다가, 
                    // 스택 마지막 몬스터가 된 경우에는 제거해준다.
                    if (_approachedNormalStack.Peek().Equals(monsterObject) == true)
                    {
                        do
                        {
                            _approachedNormalStack.Pop();
                        } while (_approachedNormalStack.Count > 0 &&
                                 _approachedNormalStack.Peek().activeSelf == false);
                    }
                }    
            }
        }

        public void AddApproach(GameObject monsterObject)
        {
            var approachStack = GetApproachedStackWithType(monsterObject.GetComponent<Monster>().MonsterType);
            approachStack.Push(monsterObject);
        }

        public bool IsFirstApproached(GameObject monsterObject)
        {
            if (FirstApproachedMonster != null)
            {
                return FirstApproachedMonster.Equals(monsterObject);    
            }
            else
            {
                return false;
            }
        }

        private Stack<GameObject> GetApproachedStackWithType(string monsterType)
        {
            Stack<GameObject> approachStack = null;
            
            switch (monsterType)
            {
                case "Normal":
                    approachStack = _approachedNormalStack;
                    break;
                case "Boss":
                    approachStack = _approachedBossStack;
                    break;
                default:
                    Debug.LogError($"정의되지 않은 MonsterType입니다. : {monsterType}");
                    break;
            }

            return approachStack;
        }
    }
}