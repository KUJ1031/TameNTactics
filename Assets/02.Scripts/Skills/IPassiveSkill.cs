using System.Collections.Generic;

public interface IPassiveSkill
{
    void OnBattleStart(Monster self, List<Monster> monsters); // 배틀 시작시 발동
    void OnTurnEnd(Monster self); // 턴 종료시 발동
    void OnDamaged(Monster self, int damage, Monster actor); // 데미지 입었을시 발동
    bool TryEscape(Monster self, ref bool isGuaranteedEscape); // 도망가기 선택시 발동
}
