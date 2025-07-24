using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using System.Collections;

public class ItemSelectPopup : MonoBehaviour
{
    [SerializeField] private Transform imageContainer;
    [SerializeField] private GameObject monsterImageButtonPrefab;
    private Monster selectedMonster;

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
            if (monster.CurHp >= monster.MaxHp)
            {
                continue;
            }

            GameObject go = Instantiate(monsterImageButtonPrefab, imageContainer);
            go.GetComponent<Image>().sprite = monster.monsterData.monsterImage;

            Image hpBar = go.transform.Find("HpBar").GetComponent<Image>();
            if (hpBar != null)
            {
                float hpRatio = (float)monster.CurHp / monster.MaxHp;
                hpBar.fillAmount = hpRatio;
            }

            TextMeshProUGUI hpText = go.transform.Find("HpText")?.GetComponent<TextMeshProUGUI>();
            if (hpText != null)
            {
                hpText.text = $"{monster.CurHp} / {monster.MaxHp}";
            }

            Button btn = go.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                selectedMonster = monster;
                OnMonsterSelected(monster);
            });
        }
    }



    public void OnMonsterSelected(Monster monster)
    {
        StartCoroutine(UseItemCoroutine(monster));
    }

    private IEnumerator UseItemCoroutine(Monster monster)
    {
        foreach (var effect in usingItem.data.itemEffects)
        {
            if (effect.type == ItemEffectType.curHp)
            {
                monster.Heal_Potion(effect.value);
                yield return AnimateHeal(monster, effect.value, 1f);
                Debug.Log($"[회복] {monster.monsterName} 체력 +{effect.value} → {monster.CurHp}/{monster.MaxHp}");
            }
            else if (effect.type == ItemEffectType.allMonsterCurHp)
            {
                foreach (var m in PlayerManager.Instance.player.ownedMonsters)
                {
                    if (m.CurHp < m.MaxHp)
                    {
                        m.Heal_Potion(effect.value);
                        yield return AnimateHeal(m, effect.value, 0.5f);
                        Debug.Log($"[회복] {m.monsterName} 체력 +{effect.value} → {m.CurHp}/{m.MaxHp}");
                    }
                }
            }
        }

        usingItem.quantity--;
        if (usingItem.quantity <= 0)
            PlayerManager.Instance.player.items.Remove(usingItem);

        FindObjectOfType<InventoryUI>()?.Refresh();

        PopulateMonsterButtons(); // 체력이 찬 몬스터는 버튼에서 제외하기 위해 다시 갱신

        // 0.2초 후 팝업 닫기
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }




    private IEnumerator AnimateHeal(Monster monster, int healAmount, float duration = 0.5f)
    {
        // 몬스터 UI 버튼 찾기 (이미지 컨테이너 내부에서 몬스터와 연결된 버튼 찾기)
        Transform monsterButton = null;
        foreach (Transform child in imageContainer)
        {
            Button btn = child.GetComponent<Button>();
            if (btn == null) continue;

            // btn.onClick의 리스너가 monster와 연결된지 확인하는 로직은 복잡하니, 
            // 아래처럼 몬스터 이미지를 비교하거나 버튼에 몬스터 참조를 별도 저장하는 방법 권장
            Image img = child.GetComponent<Image>();
            if (img != null && monster.monsterData.monsterImage == img.sprite)
            {
                monsterButton = child;
                break;
            }
        }

        if (monsterButton == null)
            yield break;

        Image hpBar = monsterButton.Find("HpBar")?.GetComponent<Image>();
        TextMeshProUGUI hpText = monsterButton.Find("HpText")?.GetComponent<TextMeshProUGUI>();

        if (hpBar == null || hpText == null)
            yield break;

        float startFill = hpBar.fillAmount;
        float endFill = Mathf.Clamp01((float)(monster.CurHp) / monster.MaxHp);

        int startHp = Mathf.RoundToInt(startFill * monster.MaxHp);
        int endHp = monster.CurHp;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            hpBar.fillAmount = Mathf.Lerp(startFill, endFill, t);
            int currentHp = Mathf.RoundToInt(Mathf.Lerp(startHp, endHp, t));
            hpText.text = $"{currentHp} / {monster.MaxHp}";

            yield return null;
        }

        hpBar.fillAmount = endFill;
        hpText.text = $"{monster.CurHp} / {monster.MaxHp}";
    }


}
