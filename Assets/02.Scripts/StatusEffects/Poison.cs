using UnityEngine;

public class Poison : StatusEffect
{
    public Poison(int duration) : base(StatusEffectType.Poison, duration) {}
    
    // 독, 턴이 시작될때 정해진 턴 만큼 최대체력의 10%만큼의 수치로 데미지 받음
    public override void OnTurnStart(Monster target)
    {
        int amount = Mathf.RoundToInt(target.MaxHp * 0.1f);
        target.TakeDamage(amount);
        duration--;
    }
}
