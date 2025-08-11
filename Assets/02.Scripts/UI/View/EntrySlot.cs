using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntrySlot : MonoBehaviour
{
    // 슬롯 배경 이미지 (Entry 슬롯 테두리 등)
    public Image entrySlotImage;

    // 몬스터의 아이콘 이미지 (실제로 보여질 몬스터 이미지)
    public Image monsterImage;

    // 체력바 오브젝트 (활성/비활성 용도)
    [SerializeField] private GameObject hpBarObject;

    // 체력바 내부 이미지 (fillAmount로 체력 비율 표현)
    [SerializeField] private Image hpFillImage; // Fill type = Horizontal

    /// <summary>
    /// 몬스터 정보를 이 슬롯에 세팅합니다.
    /// 몬스터가 battleEntry에 포함되어 있으면 체력바를 보여주고, 이미지 크기를 키웁니다.
    /// </summary>
    public void SetMonster(Monster monster)
    {
        // 몬스터 이미지 설정 및 표시
        monsterImage.sprite = monster.monsterData.monsterImage;
        monsterImage.enabled = true;

        // 슬롯 배경 이미지의 RectTransform 가져오기 (크기 조절용)
        RectTransform rt = entrySlotImage.rectTransform;

        // 해당 몬스터가 전투 출전 명단에 포함된 경우
        if (PlayerManager.Instance.player.battleEntry.Any(b => b.monsterData.monsterNumber == monster.monsterData.monsterNumber))
        {
            // 체력바 표시
            hpBarObject.SetActive(true);

            // 체력 비율 계산 후 Fill 이미지에 적용
            float hpRatio = (float)monster.CurHp / monster.MaxHp;
            hpFillImage.fillAmount = hpRatio;

            // 슬롯 이미지 크기 크게 설정
            rt.sizeDelta = new Vector2(100f, 100f);
        }
        else
        {
            // 체력바 숨김
            hpBarObject.SetActive(false);

            // 슬롯 이미지 크기 작게 설정
            rt.sizeDelta = new Vector2(50f, 50f);
        }
    }

    /// <summary>
    /// 슬롯을 비웁니다. 이미지와 체력바를 숨깁니다.
    /// </summary>
    public void ClearSlot()
    {
        monsterImage.sprite = null;
        monsterImage.enabled = false;
        hpBarObject?.SetActive(false); // 안전하게 체력바도 끔
    }
}
