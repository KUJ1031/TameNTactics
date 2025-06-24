using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntrySlot : MonoBehaviour
{
    public Image monsterImage;

    public void SetMonster(MonsterData monster)
    {
        monsterImage.sprite = monster.monsterImage;
        monsterImage.enabled = true;
    }

    public void ClearSlot()
    {
        monsterImage.sprite = null;
        monsterImage.enabled = false;
    }
}
