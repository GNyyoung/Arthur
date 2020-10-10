using System;
using System.Collections.Generic;
using System.Globalization;
using DefaultNamespace.Main;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class InventoryPanelUI : MonoBehaviour, IPanelUI, IInstanceReceiver
    {
        private InventoryPanelUI(){}
        private static InventoryPanelUI _instance;

        public static InventoryPanelUI Instance
        {
            get
            {
                if (_instance == null)
                {
                    var instances = FindObjectsOfType<InventoryPanelUI>();
                    if (instances.Length == 0)
                    {
                        var newInstance = UINavigation.GetView("Inventory")?.gameObject.AddComponent<InventoryPanelUI>();
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

        private PlayerInfo _playerInfo;
        private SwordInfo[] _swordInfos;
        private List<GameObject> _swordItemList;
        
        public Text nameText;
        public Text levelText;
        public Image levelIngredientSwordImage;
        public Image levelIngredientGoldImage;
        public Text damageText;
        public Text cooldownText;
        public Text durabilityText;
        public Text activeSkillText;
        public Text drawSkillText;
        public GameObject itemPrefab;
        public GameObject itemScrollViewContent;
        public Toggle[] equipToggles;

        private void Awake()
        {
            InstanceProvider.Add(this);
        }

        public void UpdateData()
        {
            ResetInfoInterface();
            _swordItemList = new List<GameObject>();
            
            _swordInfos = _playerInfo.GetOwnedSwords();
            for (int j = 0; j < itemScrollViewContent.transform.childCount; j++)
            {
                itemScrollViewContent.transform.GetChild(j).gameObject.SetActive(false);
            }
            
            for (int i = 0; i < _swordInfos.Length; i++)
            {
                int itemObjectIndex = i;
                GameObject item;
                if (i < itemScrollViewContent.transform.childCount)
                {
                    item = itemScrollViewContent.transform.GetChild(i).gameObject;
                    item.SetActive(true);
                }
                else
                {
                    item = Instantiate(itemPrefab, itemScrollViewContent.transform);
                }
                
                var buttonClickedEvent = new Button.ButtonClickedEvent();
                buttonClickedEvent.AddListener(() =>
                {
                    ResetInfoInterface();
                    ShowSwordInfo(itemObjectIndex);
                    UpdateEquipButtons(itemObjectIndex);
                });
                item.GetComponent<Button>().onClick = buttonClickedEvent;
                
                item.GetComponent<SwordItem>().SetData(_swordInfos[itemObjectIndex], itemObjectIndex);
                var swordImage = item.GetComponent<Image>(); 
                swordImage.sprite =
                    Resources.Load<Sprite>($"Sprites/Sword/{_swordInfos[itemObjectIndex].ImageName}");
                swordImage.preserveAspect = true;
                _swordItemList.Add(item);
            }
        }

        public void SetInstance(object obj)
        {
            if (obj.GetType() == typeof(PlayerInfo))
            {
                _playerInfo = obj as PlayerInfo;
            }
        }

        public void ResetInfoInterface()
        {
            nameText.text = null;
            levelText.text = null;
            damageText.text = null;
            cooldownText.text = null;
            RemoveTogglesEvent();
            ResetToggles();
            
        }
        public void ShowSwordInfo(int index)
        {
            var swordInfo = _swordInfos[index];
            nameText.text = swordInfo.Name;
            levelText.text = swordInfo.Level.ToString();
            damageText.text = swordInfo.Damage.ToString();
            cooldownText.text = swordInfo.Cooldown.ToString(CultureInfo.InvariantCulture);
            Debug.Log(swordInfo.Name);
            Debug.Log(levelText.text);
        }

        private void UpdateEquipButtons(int itemObjectIndex)
        {
            Debug.Log($"업데이트함 : {itemObjectIndex}");
            
            // 이미 장착된 검이라면 토글에 표시해줌
            var equippedIndex = _playerInfo.FindEquipSwordIndex(itemObjectIndex);
            if (equippedIndex >= 0)
            {
                equipToggles[equippedIndex].isOn = true;
            }
            
            // 토글에 리스너 새로 연결
            for (int i = 0; i < equipToggles.Length; i++)
            {
                int equipNumberIndex = i;
                var toggleEvent = new Toggle.ToggleEvent();
                toggleEvent.AddListener((isEquip) =>
                {
                    if (isEquip == true)
                    {
                        _playerInfo.EquipSword(itemObjectIndex, equipNumberIndex);
                        MoveItemOrder(_swordItemList[itemObjectIndex], equipNumberIndex);
                    }
                    else
                    {
                        _playerInfo.UnEquipSword(equipNumberIndex);
                        MoveItemOrder(null, equipNumberIndex);
                    }
                    
                });
                equipToggles[i].onValueChanged = toggleEvent;
            }
        }

        private void RemoveTogglesEvent()
        {
            var emptyEvent = new Toggle.ToggleEvent();
            foreach (var toggle in equipToggles)
            {
                toggle.onValueChanged = emptyEvent;
            }
        }
        private void ResetToggles()
        {
            for (int i = 0; i < equipToggles.Length; i++)
            {
                equipToggles[i].isOn = false;
            }
        }

        /// <summary>
        /// 이미 장착된 아이템을 인벤토리 가장 앞에 배치한다.
        /// </summary>
        public void SortEquippedSword()
        {
            var equippedItems = new GameObject[_playerInfo.EquipSwordIndexList.Count];
            var equipSwordIndexList = _playerInfo.EquipSwordIndexList.ToArray();
            
            foreach (var itemObject in _swordItemList)
            {
                var swordItem = itemObject.GetComponent<SwordItem>();
                for (int i = 0; i < equipSwordIndexList.Length ; i++)
                {
                    if (swordItem.ItemIndex == equipSwordIndexList[i])
                    {
                        equippedItems[i] = itemObject;
                    }
                }
            }

            for (int i = equipSwordIndexList.Length - 1; i >= 0; i--)
            {
                if (equippedItems[i] != null)
                {
                    equippedItems[i].transform.SetAsFirstSibling();
                }
            }
        }
        
        /// <summary>
        /// 아이템의 배치 순서를 바꿈
        /// </summary>
        /// <param name="itemObject">옮길 아이템</param>
        /// <param name="order">옮길 위치</param>
        private void MoveItemOrder(GameObject itemObject, int order)
        {
            // 원래 order에 있는 아이템을 다른 위치로 이동시킴.
            //임시 코드.
            itemScrollViewContent.transform.GetChild(order).SetAsLastSibling();
            //코드 끝.
            
            Debug.Log(itemObject);
            // item Object를 order로 이동시킴.
            if (itemObject != null)
            {
                itemObject.transform.SetSiblingIndex(order);   
            }
        }
    }
}