public interface IPassiveSkill
{
    void OnBattleStart(Monster self);
    void OnTurnStart(Monster self);
    void OnTurnEnd(Monster self);
    void OnDamaged(Monster self, Monster attacker);
    void OnAttack(Monster self, Monster target);
}
