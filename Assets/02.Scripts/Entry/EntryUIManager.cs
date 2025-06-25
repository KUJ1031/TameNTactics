using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 출전 몬스터 UI 패널의 표시/숨김을 제어하는 클래스
/// - 버튼 등에서 이 클래스의 메서드를 호출하여 UI On/Off 가능
/// </summary>
public class EntryUIManager : MonoBehaviour
{
    [Header("출전 패널")]
    public GameObject EntryPanel; // 실제로 활성화/비활성화할 UI 패널 오브젝트

    /// <summary>
    /// 출전 패널을 화면에 표시
    /// </summary>
    public void SetDisplayEntry()
    {
        if (EntryPanel != null)
        {
            EntryPanel.SetActive(true); // 패널을 켠다
        }
    }

    /// <summary>
    /// 출전 패널을 화면에서 숨김
    /// </summary>
    public void HideDisplayEntry()
    {
        if (EntryPanel != null)
        {
            EntryPanel.SetActive(false); // 패널을 끈다
        }
    }
}
