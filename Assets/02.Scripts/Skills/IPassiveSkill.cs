using System.Collections.Generic;

public interface IPassiveSkill
{
    void OnBattleStart(Monster self, List<Monster> monsters); // 배틀 시작시 발동
    void OnTurnEnd(Monster self); // 턴 종료시 발동
    int OnDamaged(Monster self, int damage, Monster actor); // 데미지 입었을시 발동
    void OnAllyDeath(Monster self); // 같은 팀이 죽었을때 발동
    void OnAttack(Monster attacker, int damage, Monster target, SkillData skill, float effectiveness); // 데미지를 주었을때 발동
}
