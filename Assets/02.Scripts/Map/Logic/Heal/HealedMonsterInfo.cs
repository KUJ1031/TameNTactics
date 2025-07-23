using UnityEngine;
using UnityEngine.UI;

public class HealedMonsterInfo
{
    public string name;
    public int healedAmount;
    public int curHp;
    public int maxHp;
    public Sprite Image;

    public HealedMonsterInfo(string name, int healed, int cur, int max, Sprite image)
    {
        this.name = name;
        this.healedAmount = healed;
        this.curHp = cur;
        this.maxHp = max;
        this.Image = image;
    }
}
