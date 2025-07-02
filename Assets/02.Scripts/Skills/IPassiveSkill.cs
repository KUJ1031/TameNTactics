using System.Collections.Generic;

public interface IPassiveSkill
{
    void OnBattleStart(Monster self, List<Monster> monsters);
    void OnTurnEnd(Monster self);
    void OnDamaged(Monster self, int damage, Monster actor);
    bool TryEscape(Monster self, ref bool isGuaranteedEscape);
}
