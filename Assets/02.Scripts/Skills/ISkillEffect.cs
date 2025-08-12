using System.Collections;
using System.Collections.Generic;

public interface ISkillEffect
{
    IEnumerator Execute(Monster caster, List<Monster> targets);
}