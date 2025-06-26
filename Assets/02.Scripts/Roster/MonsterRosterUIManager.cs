using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRosterUIManager : MonoBehaviour
{
    [Header("출전 패널")]
    public GameObject RosterPanel; // 실제로 활성화/비활성화할 UI 패널 오브젝트

    /// <summary>
    /// 출전 패널을 화면에 표시
    /// </summary>
    public void SetDisplayRoster()
    {
        if (RosterPanel != null)
        {
            RosterPanel.SetActive(true); // 패널을 켠다
        }
    }

    /// <summary>
    /// 출전 패널을 화면에서 숨김
    /// </summary>
    public void HideDisplayRoster()
    {
        if (RosterPanel != null)
        {
            RosterPanel.SetActive(false); // 패널을 끈다
        }
    }
}
