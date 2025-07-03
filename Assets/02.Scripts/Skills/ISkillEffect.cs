using System.Collections.Generic;

public interface ISkillEffect
{
    void Execute(Monster caster, List<Monster> targets);
}