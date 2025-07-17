using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;

    private Vector2 offset = new Vector2(-150f, 110f);

    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        transform.position = mousePos + offset;
    }

    public void ShowSkillTooltip(string name, string desc)
    {
        skillName.text = name;
        skillDescription.text = desc;
        gameObject.SetActive(true);
    }

    public void HideSkillTooltip()
    {
        gameObject.SetActive(false);
    }
}
