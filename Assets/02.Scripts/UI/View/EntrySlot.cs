using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntrySlot : MonoBehaviour
{
    public Image monsterImage;

    public void SetMonster(Monster monster)
    {
        monsterImage.sprite = monster.monsterData.monsterImage;
        monsterImage.enabled = true;
        //만약 몬스터가 battleEntry에 있다면 이미지를 크게
        RectTransform rt = monsterImage.rectTransform;

        if (PlayerManager.Instance.player.battleEntry.Contains(monster))
        {
            rt.sizeDelta = new Vector2(100f, 100f); // 정상 크기
        }
        else
        {
            rt.sizeDelta = new Vector2(50f, 50f);   // 작게 표시
        }
    }

    public void ClearSlot()
    {
        monsterImage.sprite = null;
        monsterImage.enabled = false;
    }

    

}
