using System;
using System.Collections.Generic;
using System.Globalization;
using DefaultNamespace.Main;
using Main;
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

        [SerializeField]
        private Image levelIngredientSwordImage = null;
        [SerializeField]
        private Image levelIngredientGoldImage = null;
        [SerializeField]
        private Text nameText = null;
        [SerializeField]
        private Text levelText = null;
        [SerializeField]
        private Text damageText = null;
        [SerializeField]
        private Text cooldownText = null;
        [SerializeField]
        private Text reachText = null;
        [SerializeField]
        private Text durabilityText = null;
        [SerializeField]
        private Text activeSkillText = null;
        [SerializeField]
        private Text drawSkillText = null;
        [SerializeField]
        private PlayerInfo _playerInfo = null;
        private SwordInfo[] _swordInfos;
        private List<GameObject> _swordItemList;
        private Sprite _itemCellImage;
        private Sprite _equippedCellImage;
        private int clickedItemIndex;
        
        public GameObject itemPrefab;
        public GameObject itemScrollViewContent;
        public Toggle[] equipToggles;
        public Button levelUpButton;
        public Text levelUpCostText;
        
        
        public string ClickedSwordName { get; private set; }
        
        
        public GameObject equippedItemObject;

        private void Awake()
        {
            InstanceProvider.Add(this);
            _itemCellImage = Resources.Load<Sprite>("Sprites/UI/Main/ItemCell");
            _equippedCellImage = Resources.Load<Sprite>("Sprites/UI/Main/EquippedCell");
        }

        /// <summary>
        /// 인벤토리 정보를 업데이트하고 보여줌
        /// </summary>
        public void ShowPanelData()
        {
            ResetInfoInterface();
            ResetAllItemCellImage();
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
                
                // 아이템 클릭 시 실행할 이벤트.
                var buttonClickedEvent = new Button.ButtonClickedEvent();
                buttonClickedEvent.AddListener(() =>
                {
                    ResetInfoInterface();
                    ShowSwordInfo(itemObjectIndex);
                    UpdateEquipButtons(itemObjectIndex);
                    UpdateLevelupButton(itemObjectIndex);
                    MainSound.Instance.OutputItemClickSound();
                    clickedItemIndex = itemObjectIndex;
                });
                item.GetComponent<Button>().onClick = buttonClickedEvent;
                
                SetItemSprite(item, itemObjectIndex);
                _swordItemList.Add(item);
            }
            
            SortEquippedSword();
            
            DetailInfoPanelUI.Instance.gameObject.SetActive(false);
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
        
        /// <summary>
        /// 검 클릭 시 화면에 정보를 띄움.
        /// </summary>
        /// <param name="index">무기 인덱스</param>
        public void ShowSwordInfo(int index)
        {
            var swordInfo = _swordInfos[index];
            nameText.text = TextGetter.GetText(swordInfo.Name, TextGetter.TextType.SwordName);
            levelText.text = $"Lv. {swordInfo.Level.ToString()}";
            damageText.text = swordInfo.Damage.ToString();
            cooldownText.text = swordInfo.AttackCooldown.ToString(CultureInfo.InvariantCulture);
            reachText.text = swordInfo.Length.ToString();
            durabilityText.text = swordInfo.Durability.ToString();
            activeSkillText.text = TextGetter.GetText(swordInfo.ActiveSkill, TextGetter.TextType.SkillName);
            drawSkillText.text = TextGetter.GetText(swordInfo.DrawSkill, TextGetter.TextType.SkillName);
            int level = _playerInfo.GetSword(index).Level;
            levelUpCostText.text = Data.Instance.GetLevelUpCost(level + 1).ToString();
            Debug.Log($"클릭한 무기 : {swordInfo.Name}");

            ClickedSwordName = swordInfo.Name;
        }

        /// <summary>
        /// 무기 장착 버튼을 현재 클릭한 무기에 맞춰 업데이트함.
        /// </summary>
        /// <param name="itemObjectIndex"></param>
        private void UpdateEquipButtons(int itemObjectIndex)
        {
            Debug.Log($"{itemObjectIndex}번째 무기 정보 표시");
            
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
                        MainSound.Instance.OutputEquipWeaponSound();
                    }
                    else
                    {
                        _playerInfo.UnEquipSword(equipNumberIndex);
                        MoveItemOrder(null, equipNumberIndex);
                        MainSound.Instance.OutputUnequipSwordSound();
                    }
                    
                });
                equipToggles[i].onValueChanged = toggleEvent;
            }
        }

        /// <summary>
        /// 현재 선택한 아이템을 레벨업하도록 버튼을 업데이트합니다.
        /// </summary>
        private void UpdateLevelupButton(int itemObjectIndex)
        {
            var clickedEvent = new Button.ButtonClickedEvent();
            clickedEvent.AddListener(() => _playerInfo.LevelUpWeapon(itemObjectIndex));
            levelUpButton.onClick = clickedEvent;
        }
        
        public void OnClickDetailInfo()
        {
            Debug.Log(_playerInfo.GetSword(clickedItemIndex).Name);
            DetailInfoPanelUI.Instance.gameObject.SetActive(true);
            DetailInfoPanelUI.Instance.ShowPanelData();
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

            Debug.Log(equippedItems.Length);
            for (int i = equipSwordIndexList.Length - 1; i >= 0; i--)
            {
                if (equippedItems[i] != null)
                {
                    Debug.Log($"{i}번째에 이미지 변경");
                    equippedItems[i].transform.SetAsFirstSibling();
                    equippedItems[i].GetComponent<Image>().sprite = _equippedCellImage;
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
            int unEquipOrder = order;
            for (int i = 0; i < order; i++)
            {
                if (_playerInfo.EquipSwordIndexList[i] == -1)
                {
                    unEquipOrder -= 1;
                }
            }
            var unequipItemTransform = itemScrollViewContent.transform.GetChild(unEquipOrder);
            bool isMove = true;
            foreach (var equipIndex in _playerInfo.EquipSwordIndexList)
            {
                if (unequipItemTransform.GetComponent<SwordItem>().ItemIndex == equipIndex)
                {
                    isMove = false;
                    break;
                }    
            }
            if (isMove == true)
            {
                unequipItemTransform.SetAsLastSibling();
                unequipItemTransform.GetComponent<Image>().sprite = _itemCellImage;   
            }
            
            // 원래 order에 있는 아이템을 다른 위치로 이동시킴.
            //임시 코드. 인벤토리 맨 마지막으로 가게 해놨음.
            // var unequipItemTransform = itemScrollViewContent.transform.GetChild(order); 
            // unequipItemTransform.SetAsLastSibling();
            // unequipItemTransform.GetComponent<Image>().sprite = _itemCellImage;
            //코드 끝.
            
            Debug.Log(itemObject);
            // item Object를 order로 이동시킴.
            if (itemObject != null)
            {
                int placeOrder = order;
                for (int i = 0; i < order; i++)
                {
                    if (_playerInfo.EquipSwordIndexList[i] == -1)
                    {
                        placeOrder -= 1;
                    }
                }
                itemObject.transform.SetSiblingIndex(placeOrder);
                itemObject.GetComponent<Image>().sprite = _equippedCellImage;
            }
        }

        private void ResetAllItemCellImage()
        {
            for (int i = 0; i < itemScrollViewContent.transform.childCount; i++)
            {
                var item = itemScrollViewContent.transform.GetChild(i).gameObject;
                item.GetComponent<Image>().sprite = _itemCellImage;
            }
        }

        /// <summary>
        /// 인벤토리에 띄울 아이템 이미지를 붙입니다.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="itemObjectIndex"></param>
        public void SetItemSprite(GameObject item, int itemObjectIndex)
        {
            var swordItemComponent = item.GetComponent<SwordItem>(); 
            swordItemComponent.SetData(_swordInfos[itemObjectIndex], itemObjectIndex); 
            Sprite itemSprite = Resources.Load<Sprite>($"Sprites/Sword/{_swordInfos[itemObjectIndex].ImageName}");
            swordItemComponent.itemImage.sprite = itemSprite;
            var itemImageRectTransform = swordItemComponent.itemImage.GetComponent<RectTransform>();
            float height = itemSprite.textureRect.height / itemSprite.pixelsPerUnit * 100;
            Debug.Log($"{height}, {item.GetComponent<RectTransform>().rect.width / itemImageRectTransform.localScale.y}");
            if (height < 
                item.GetComponent<RectTransform>().rect.width / itemImageRectTransform.localScale.y)
            {
                itemImageRectTransform.anchorMax = Vector2.one * 0.5f;
                itemImageRectTransform.anchorMin = Vector2.one * 0.5f;
                itemImageRectTransform.pivot = Vector2.one * 0.5f;
                itemImageRectTransform.anchoredPosition = Vector2.zero;
            }
            else
            {
                itemImageRectTransform.anchorMax = Vector2.one;
                itemImageRectTransform.anchorMin = Vector2.one;
                itemImageRectTransform.pivot = new Vector2(0.5f, 1.05f);
                itemImageRectTransform.anchoredPosition = Vector2.zero;
            }
            swordItemComponent.itemImage.preserveAspect = true;
            swordItemComponent.itemImage.SetNativeSize();
        }
    }
}