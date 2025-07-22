using UnityEngine;

public class MonsterCharacter : MonoBehaviour
{
    public Monster monster { get; private set; }
    [SerializeField] private SpriteRenderer spriteRenderer;
    private MonsterShaker shaker;

    public void Init(Monster monster)
    {
        this.monster = monster;
        spriteRenderer.sprite = monster.monsterData.monsterImage;
        shaker = GetComponent<MonsterShaker>();

        monster.DamagedAnimation += ShakeOnDamage;
    }

    private void OnDestroy()
    {
        if (monster != null)
        {
            monster.DamagedAnimation -= ShakeOnDamage;
        }
    }

    private void ShakeOnDamage(Monster monster)
    {
        shaker?.TriggerShake();
    }
}
