﻿using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class UIView : MonoBehaviour
    {
        private IPanelUI _panelUI;
        [SerializeField]
        private bool isWindow;

        // 화면에 표시할 위치 조절용으로 사용
        public Vector2 position;
        private void Awake()
        {
            UINavigation.AddView(this);
            _panelUI = GetComponent<IPanelUI>();
        }

        private void Start()    
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            UINavigation.RemoveView(this);
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
            _panelUI.ShowPanelData();
            
            if (isWindow == false)
            {
                UINavigation.PeekStack()?.Hide();   
            }

            var resolution = Screen.currentResolution;
            Debug.Log(resolution);
            // this.transform.position = new Vector3(
            // position.x + (resolution.width / 2.0f),
            // position.y + (resolution.height / 2.0f));
            this.transform.position = UINavigation.baseTransform.position;
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
    }
}