using System;
using UnityEngine;

public class MonsterCharacter : MonoBehaviour
{
    public Monster monster { get; private set; }
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void Init(Monster monster)
    {
        this.monster = monster;
        spriteRenderer.sprite = monster.monsterData.monsterImage;
    }
}
