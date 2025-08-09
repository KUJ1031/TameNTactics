using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EndingManager : MonoBehaviour
{
    public Image EntryMonster1;
    public Image EntryMonster2;
    public Image EntryMonster3;
    public Image EntryMonster4;
    public Image EntryMonster5;

    public Image[] entryMonsetersimages;

    private void Start()
    {
        Time.timeScale = 1f; // 게임 시간 정지
    }

    public void output()
    {
        for (int i = 0; i < PlayerManager.Instance.player.entryMonsters.Count; i++)
        {
            if (PlayerManager.Instance.player.entryMonsters[i].monsterData != null)
            {
                entryMonsetersimages[i].sprite = PlayerManager.Instance.player.entryMonsters[i].monsterData.monsterImage;
            }
            else
            {
                entryMonsetersimages[i].enabled = false;
            }
        }
    }
}
