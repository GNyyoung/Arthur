using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class DetailInfoPanelUI : MonoBehaviour, IPanelUI
    {
        private DetailInfoPanelUI(){}
        private static DetailInfoPanelUI _instance;

        public static DetailInfoPanelUI Instance
        {
            get
            {
                if (_instance == null)
                {
                    var instances = FindObjectsOfType<DetailInfoPanelUI>();
                    if (instances.Length == 0)
                    {
                        var newInstance = UINavigation.GetView("DetailInfo")?.gameObject.AddComponent<DetailInfoPanelUI>();
                        _instance = newInstance;
                    }
                    else if (instances.Length >= 1)
                    {
                        for (int i = 1; i > instances.Length; i++)
                        {
                            Destroy(instances[i]);
                        }

                        _instance = instances[0];
                    }
                }

                return _instance;
            }
        }

        [SerializeField] private Text nameText = null;
        [SerializeField] private Text swordDesc = null;
        [SerializeField] private Text activeSkillName = null;
        [SerializeField] private Text activeSkillDesc = null;
        [SerializeField] private Text drawSkillName = null;
        [SerializeField] private Text drawSkillDesc = null;
        [SerializeField] private Button closeButton = null;
        [SerializeField] private Image swordImage = null;


        private void Awake()
        {
            Debug.Log("등록");
        }

        public void ShowPanelData()
        {
            var sword = Data.Instance.GetSword(InventoryPanelUI.Instance.ClickedSwordName);
            nameText.text = TextGetter.GetText(sword.Name, TextGetter.TextType.SwordName);
            swordDesc.text = TextGetter.GetText(sword.Name, TextGetter.TextType.SwordDesc);
            activeSkillName.text = TextGetter.GetText(sword.ActiveSkill, TextGetter.TextType.SkillName);
            activeSkillDesc.text = TextGetter.GetText(sword.ActiveSkill, TextGetter.TextType.SkillDesc);
            drawSkillName.text = TextGetter.GetText(sword.DrawSkill, TextGetter.TextType.SkillName);
            drawSkillDesc.text = TextGetter.GetText(sword.DrawSkill, TextGetter.TextType.SkillDesc);
            swordImage.sprite = Resources.Load<Sprite>(
                $"Sprites/Sword/{Data.Instance.GetSword(InventoryPanelUI.Instance.ClickedSwordName).ImagePath}");
        }

        public void OnclickCloseButton()
        {
            gameObject.SetActive(false);
        }
    }
}