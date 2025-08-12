using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC_Info : MonoBehaviour
{
    private TextMeshPro infoText;
    private MonsterCharacter monsterCharacter;
    private NPCInteraction npc;

    private bool isPlayerTouching = false;

    private void Awake()
    {
        infoText = GetComponentInChildren<TextMeshPro>();
        monsterCharacter = GetComponentInParent<MonsterCharacter>();
        if (transform.parent != null)
        {
            foreach (Transform sibling in transform.parent)
            {
                if (sibling == transform) continue; // 자기 자신은 스킵

                npc = sibling.GetComponent<NPCInteraction>();
                if (npc != null) break;
            }
        }
    }

    private void Start()
    {
        if (infoText != null)
        {
            if (monsterCharacter != null)
            {
                infoText.text = $"<color=#D93333>Lv. {monsterCharacter.monster.Level} {monsterCharacter.monster.monsterName}</color>";
            }
            else if (npc != null)
            {
                infoText.text = $"<color=#3380E6>{npc.npcName}</color>";
            }
        }
    }
}
