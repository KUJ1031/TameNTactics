using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuccessRangeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject successRangePrefab;
    [SerializeField] private GameObject backGroundPrefab;
    [SerializeField] private RectTransform parent;
    private Color successColor = Color.white;

    private void Start()
    {
        if (successRangePrefab == null) { Debug.Log("Ring에 successRangePrefab을 할당해주세요"); }
        if (backGroundPrefab == null) { Debug.Log("Ring에 backGroundPrefab을 할당해주세요"); }
        if (parent == null) { Debug.Log("Ring에 parent를 할당해주세요"); }
    }

    //성공범위 시각범위 생성
    public void SpawnRanges(List<RotationRange> ranges)
    {
        //기존 범위 제거.
        foreach (Transform child in parent)
            Destroy(child.gameObject);

        //배경생성.
        Instantiate(backGroundPrefab, parent);

        //새 범위 생성.
        foreach (RotationRange range in ranges)
        {
            GameObject go = Instantiate(successRangePrefab, parent);
            go.GetComponent<Image>().color = successColor;
            SuccessRangeVisual visual = go.GetComponent<SuccessRangeVisual>();
            visual.SetRange(range);
        }
    }

    public void SetSuccessColor(Color color)
    {
        successColor = color;
    }
}
