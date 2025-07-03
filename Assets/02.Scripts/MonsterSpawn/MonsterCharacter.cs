using System;
using UnityEngine;

public class MonsterCharacter : MonoBehaviour
{
    public Monster monster { get; private set; }

    public void Init(Monster monster)
    {
        this.monster = monster;
    }
}
