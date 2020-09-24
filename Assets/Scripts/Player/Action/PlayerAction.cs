using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class PlayerAction : MonoBehaviour, IPlayerAction
    {
        protected Player Player { get; private set; }

        // protected void Awake()
        // {
        //     Player = GetComponent<Player>();
        // }

        public virtual void InitializeAction(Player player)
        {
            Player = player;
        }

        public abstract void StartAction();
        public abstract void StopAction();
        public abstract PlayerStatus GetStatus();
    }    
}


