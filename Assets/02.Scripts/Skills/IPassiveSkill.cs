using System.Collections.Generic;

public interface IPassiveSkill
{
    void OnBattleStart(Monster self, List<Monster> monsters);
    void OnTurnEnd(Monster self);
    void OnDamaged(Monster self, int damage, int reflectDamage);
    bool TryEscape(Monster self, bool isGuaranteedEscape);
}
