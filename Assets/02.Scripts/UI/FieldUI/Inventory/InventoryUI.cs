using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class InventoryUI : FieldMenuBaseUI
{
    [SerializeField] private Transform slotParent;         // 슬롯들을 넣을 부모 오브젝트
    [SerializeField] private GameObject slotPrefab;        // 슬롯 프리팹
    [SerializeField] private Button useButton, equipButton, unequipButton, dropButton;
    [SerializeField] private Image ItemImage;
    [SerializeField] private TextMeshProUGUI ItemInfoText;

    private ItemInstance selectedItem;

    public List<ItemInstance> items;

    [SerializeField] private Button consumableButton;
    [SerializeField] private Button equipableButton;
    [SerializeField] private Button gestureButton;

    private ItemType? currentFilter = null;

    [SerializeField] private ItemSelectPopup itemSelectPopup; // 로고 오브젝트
    [SerializeField] private GameObject warringPopup;
    [SerializeField] private TextMeshProUGUI warringPopupText;
    [SerializeField] private Button warringPopupExitButton;
    [SerializeField] private TextMeshProUGUI alertText;

    [SerializeField] private Button closeMenuButton;
    private void Awake()
    {
        closeMenuButton.onClick.AddListener(OnClickCloseMenuButton);
    }

    public void OnClickCloseMenuButton()
    {
        FieldUIManager.Instance.CloseAllUI();
    }

    private void Start()
    {
        useButton.onClick.AddListener(OnUse);
        equipButton.onClick.AddListener(OnEquip);
        unequipButton.onClick.AddListener(OnUnEquip);
        dropButton.onClick.AddListener(OnDrop);

        consumableButton.onClick.AddListener(() => ApplyFilter(ItemType.consumable));
        equipableButton.onClick.AddListener(() => ApplyFilter(ItemType.equipment));
        gestureButton.onClick.AddListener(() => ApplyFilter(ItemType.gesture));

        warringPopupExitButton.onClick.AddListener(CloseWarringPopup);

        Refresh();

        ApplyFilter(ItemType.consumable);
    }

    private void OnEnable()
    {
        items = PlayerManager.Instance.player.items;
        ApplyFilter(ItemType.consumable);
    }

    public void Refresh(ItemInstance retainSelectedItem = null)
    {
        // 기존 슬롯 제거
        for (int i = 0; i < slotParent.childCount; i++)
            Destroy(slotParent.GetChild(i).gameObject);

        int createdSlotCount = 0;

        foreach (var item in items)
        {
            // 현재 필터 조건에 맞지 않으면 건너뜀
            if (currentFilter.HasValue && item.data.type != currentFilter.Value)
                continue;

            // 슬롯 생성
            GameObject slotObj = Instantiate(slotPrefab, slotParent);
            InventorySlotUI slotUI = slotObj.GetComponent<InventorySlotUI>();
            slotUI.Init(item, this);

            // 장착 여부 확인 후 설정
            item.isEquipped = PlayerManager.Instance.player.playerEquipment.Any(e => e.data.itemId == item.data.itemId);

            createdSlotCount++;
        }

        // 필터에 해당하는 아이템이 하나도 없을 경우
        if (currentFilter.HasValue && createdSlotCount == 0)
        {
            string typeText = "";

            switch (currentFilter.Value)
            {
                case ItemType.equipment:
                    typeText = "장착";
                    break;
                case ItemType.consumable:
                    typeText = "소비";
                    break;
                case ItemType.gesture:
                    typeText = "제스처";
                    break;
            }
            alertText.gameObject.SetActive(true);
            alertText.text = $"보유중인 {typeText} 아이템이 없습니다!";
        }
        else alertText.gameObject.SetActive(false);

        // 선택 항목 및 버튼 UI 갱신
        selectedItem = retainSelectedItem;
        UpdateButtons();
    }



    public void SelectItem(ItemInstance item)
    {
        selectedItem = item;
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        if (selectedItem == null)
        {
            useButton.interactable = false;
            equipButton.interactable = false;
            unequipButton.interactable = false;
            dropButton.interactable = false;
            return;
        }

        bool isConsumable = selectedItem.data.type == ItemType.consumable;
        bool isEquipment = selectedItem.data.type == ItemType.equipment;
        bool isGesture = selectedItem.data.type == ItemType.gesture;

        useButton.interactable = isConsumable;
        equipButton.interactable = isEquipment && !selectedItem.isEquipped;
        unequipButton.interactable = isEquipment && selectedItem.isEquipped;
        dropButton.interactable = true;

        useButton.gameObject.SetActive(isConsumable);
        equipButton.gameObject.SetActive(isEquipment && !selectedItem.isEquipped);
        unequipButton.gameObject.SetActive(isEquipment && selectedItem.isEquipped);
        dropButton.gameObject.SetActive(!isGesture);
    }

    private void OnUse()
    {
        if (selectedItem == null) return;

        if (selectedItem.data.itemEffects.Exists(e => e.type == ItemEffectType.curHp))
        {
            if (PlayerManager.Instance.player.ownedMonsters.All(m => m.CurHp >= m.MaxHp))
            {
                warringPopup.gameObject.SetActive(true);
                warringPopupText.text = $"[사용 불가] 모든 몬스터가 최대 체력입니다.";
                return;
            }
            // 대상 몬스터 선택 창 띄우기
            itemSelectPopup.gameObject.SetActive(true);
            itemSelectPopup.Open(selectedItem);
            return;
        }

        if (selectedItem.data.itemEffects.Exists(e => e.type == ItemEffectType.allMonsterCurHp))
        {
            if (PlayerManager.Instance.player.ownedMonsters.All(m => m.CurHp >= m.MaxHp))
            {
                warringPopup.gameObject.SetActive(true);
                warringPopupText.text = $"[사용 불가] 모든 몬스터가 최대 체력입니다.";
                return;
            }
            itemSelectPopup.gameObject.SetActive(true);
            itemSelectPopup.Open(selectedItem);
            itemSelectPopup.OnMonsterSelected(PlayerManager.Instance.player.ownedMonsters[0]);
            return;
        }

        // 즉시 효과 처리용 (예: 골드 증가 등)
        foreach (var effect in selectedItem.data.itemEffects)
        {
            Debug.Log($"[사용] {effect.type} +{effect.value}");
        }

        selectedItem.quantity--;
        if (selectedItem.quantity <= 0)
            items.Remove(selectedItem);

        Refresh();
    }


    private void OnEquip()
    {
        if (selectedItem == null) return;

        var equipment = PlayerManager.Instance.player.playerEquipment;

        // 중복 착용 체크
        if (equipment.Any(equip => equip.data.itemId == selectedItem.data.itemId))
        {
            Debug.LogWarning("[장착 실패] 이미 같은 장비가 장착되어 있습니다.");
            warringPopup.gameObject.SetActive(true);
            warringPopupText.text = $"[장착 실패] 이미 같은 장비가 장착되어 있습니다.";
            return;
        }

        // 1개만 착용 제한
        if (equipment.Count >= 1)
        {
            Debug.LogWarning("[장착 실패] 장비 아이템은 1개만 착용 가능합니다.");
            warringPopup.gameObject.SetActive(true);
            warringPopupText.text = $"[장착 실패] 장비 아이템은 1개만 착용 가능합니다.\n현재 장착 아이템 : <color=#FF4444>{equipment[0].data.itemName}</color>";
            return;
        }

        selectedItem.isEquipped = true;
        equipment.Add(selectedItem);

        // 능력치 반영
        foreach (var effect in selectedItem.data.itemEffects)
        {
            foreach (var m in PlayerManager.Instance.player.ownedMonsters)
            {
                switch (effect.type)
                {
                    case ItemEffectType.attack:
                        m.AttackUp(effect.value); break;
                    case ItemEffectType.defense:
                        m.DefenseUp(effect.value); break;
                    case ItemEffectType.speed:
                        m.SpeedUp(effect.value); break;
                    case ItemEffectType.criticalChance:
                        m.CriticalChanceUp(effect.value); break;
                }
            }
        }

        Refresh(selectedItem);
        SelectItem(selectedItem);
    }


    private void OnUnEquip()
    {
        if (selectedItem == null) return;

        var player = PlayerManager.Instance.player;

        // itemId 기준으로 장착된 아이템 제거
        int removedCount = player.playerEquipment.RemoveAll(equip => equip.data.itemId == selectedItem.data.itemId);

        if (removedCount > 0)
        {
            selectedItem.isEquipped = false;

            // 능력치 감소 적용
            foreach (var effect in selectedItem.data.itemEffects)
            {
                foreach (var m in player.ownedMonsters)
                {
                    switch (effect.type)
                    {
                        case ItemEffectType.attack:
                            m.AttackDown(effect.value);
                            Debug.Log($"[해제] {m.monsterName} 공격력 -{effect.value}");
                            break;
                        case ItemEffectType.defense:
                            m.DefenseDown(effect.value);
                            Debug.Log($"[해제] {m.monsterName} 방어력 -{effect.value}");
                            break;
                        case ItemEffectType.speed:
                            m.SpeedDown(effect.value);
                            Debug.Log($"[해제] {m.monsterName} 속도 -{effect.value}");
                            break;
                        case ItemEffectType.criticalChance:
                            m.CriticalChanceDown(effect.value);
                            Debug.Log($"[해제] {m.monsterName} 치명타 확률 -{effect.value}%");
                            break;
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("[해제 실패] 장착 목록에서 해당 아이템을 찾을 수 없습니다.");
            warringPopup.gameObject.SetActive(true);
            warringPopupText.text = $"[해제 실패] 장착 목록에서 해당 아이템을 찾을 수 없습니다.";
        }

        Refresh(selectedItem);
        SelectItem(selectedItem);
    }




    private void OnDrop()
    {
        if (selectedItem == null) return;

        items.Remove(selectedItem);
        Debug.Log($"{selectedItem.data.itemName} 버림");

        Refresh();
    }

    public void DisplayItemInfo(ItemInstance item)
    {
        if (item == null) return;

        ItemImage.sprite = item.data.itemImage;
        ItemInfoText.text = $"{item.data.itemName}\n{item.data.description}\n가격: {item.data.goldValue} G";
    }

    private void ApplyFilter(ItemType type)
    {
        currentFilter = type;
        Refresh(); // 필터 적용 후 다시 렌더링
    }

    public void CloseWarringPopup()
    {
        warringPopup.gameObject.SetActive(false);
    }

}
