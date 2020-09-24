using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class UIView : MonoBehaviour
    {
        private IPanelUI _panelUI;
        [SerializeField]
        private bool isWindow;

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

        public void Show()
        {
            this.gameObject.SetActive(true);
            _panelUI.UpdateData();
            
            if (isWindow == false)
                {UINavigation.PeekStack()?.Hide();   
            }
            
            var resolution = new Resolution();
            var rect = GetComponent<RectTransform>().rect;
            this.transform.position = new Vector3(
                position.x + ((rect.width - resolution.width) / 2.0f),
                position.y + ((rect.height - resolution.height) / 2.0f));
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
    }
}