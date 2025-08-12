using UnityEngine;

public class Burn : StatusEffect
{
    public Burn(int duration) : base(StatusEffectType.Burn, duration){}

    // 화상, 턴이 시작될때 정해진 턴 만큼 최대체력의 10%만큼의 수치로 데미지 받음
    public override void OnTurnStart(Monster target)
    {
        int amount = Mathf.RoundToInt(target.MaxHp * 0.1f);
        target.TakeDamage(amount);
        duration--;
    }
}
