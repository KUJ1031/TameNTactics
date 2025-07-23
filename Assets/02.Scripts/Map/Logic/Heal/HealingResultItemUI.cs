using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealingResultItemUI : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text healText;
    public TMP_Text hpText;
    public Image Image;


    public void Setup(HealedMonsterInfo info)
    {
        nameText.text = info.name;
        healText.text = $"+{info.healedAmount} HP";
        hpText.text = $"{info.curHp} / {info.maxHp}";
        Image.sprite = info.Image;
    }
}
