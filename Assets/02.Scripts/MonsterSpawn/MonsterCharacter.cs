using UnityEngine;

public class MonsterCharacter : MonoBehaviour
{
    public Monster monster { get; private set; }
    private MonsterShaker shaker;
    private SPUM_Prefabs animHandler;

    public void Init(Monster monster)
    {
        this.monster = monster;
        shaker = GetComponent<MonsterShaker>();
        animHandler = GetComponentInChildren<SPUM_Prefabs>();

        animHandler.OverrideControllerInit();

        monster.DamagedAnimation += ShakeOnDamage;
    }

    private void OnEnable()
    {
        EventBus.OnMonsterDead += OnMonsterDead;
    }

    private void OnDisable()
    {
        EventBus.OnMonsterDead += OnMonsterDead;
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
        PlayDamaged();
    }

    private void OnMonsterDead(Monster monster)
    {
        if (this.monster == monster && monster.CurHp == 0)
        {
            PlayDeath();
        }
    }

    public void PlayIdle() => animHandler?.PlayAnimation(PlayerState.IDLE, 0);
    public void PlayMove() => animHandler?.PlayAnimation(PlayerState.MOVE, 0);
    public void PlayAttack() => animHandler?.PlayAnimation(PlayerState.ATTACK, 0);
    public void PlayDamaged() => animHandler?.PlayAnimation(PlayerState.DAMAGED, 0);
    public void PlayDeath() => animHandler?.PlayAnimation(PlayerState.DEATH, 0);
}
