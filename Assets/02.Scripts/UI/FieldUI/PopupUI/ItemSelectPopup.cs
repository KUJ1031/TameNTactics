using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ItemSelectPopup : MonoBehaviour
{
    [SerializeField] private Transform imageContainer;
    [SerializeField] private GameObject monsterImageButtonPrefab;

    private ItemInstance usingItem;

    public void Open(ItemInstance item)
    {
        usingItem = item;
        gameObject.SetActive(true);
        PopulateMonsterButtons();
    }

    private void PopulateMonsterButtons()
    {
        foreach (Transform child in imageContainer)
            Destroy(child.gameObject);

        foreach (var monster in PlayerManager.Instance.player.ownedMonsters)
        {
            GameObject go = Instantiate(monsterImageButtonPrefab, imageContainer);
            go.GetComponent<Image>().sprite = monster.monsterData.monsterImage;

            // 체력바 이미지 (Filled 타입)
            Image hpBar = go.transform.Find("HpBar").GetComponent<Image>();
            if (hpBar != null)
            {
                float hpRatio = (float)monster.CurHp / monster.MaxHp;
                hpBar.fillAmount = hpRatio;
            }

            // 체력 텍스트 (예: "35 / 100")
            TextMeshProUGUI hpText = go.transform.Find("HpText")?.GetComponent<TextMeshProUGUI>();
            if (hpText != null)
            {
                hpText.text = $"{monster.CurHp} / {monster.MaxHp}";
            }

            Button btn = go.GetComponent<Button>();
            btn.onClick.AddListener(() => OnMonsterSelected(monster));
        }
    }


    private void OnMonsterSelected(Monster monster)
    {
        foreach (var effect in usingItem.data.itemEffects)
        {
            if (effect.type == ItemEffectType.curHp)
            {
                monster.Heal_Potion(effect.value);
                Debug.Log($"[회복] {monster.monsterName} 체력 +{effect.value} → {monster.CurHp}/{monster.MaxHp}");
            }
        }

        usingItem.quantity--;
        if (usingItem.quantity <= 0)
            PlayerManager.Instance.player.items.Remove(usingItem);

        gameObject.SetActive(false);
        FindObjectOfType<InventoryUI>()?.Refresh();
    }

}
