using System.Collections.Generic;
using UnityEngine;

public class SuccessRangeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject successRangePrefab;
    [SerializeField] private GameObject backGroundPrefab;
    [SerializeField] private RectTransform parent;

    private void Start()
    {
        if (successRangePrefab == null) { Debug.Log("Ring에 successRangePrefab을 할당해주세요"); }
        if (backGroundPrefab == null) { Debug.Log("Ring에 backGroundPrefab을 할당해주세요"); }
        if (parent == null) { Debug.Log("Ring에 parent를 할당해주세요"); }
    }
    public void SpawnRanges(List<RotationRange> ranges)
    {
        foreach (Transform child in parent)
            Destroy(child.gameObject);
        Instantiate(backGroundPrefab, parent);
        foreach (RotationRange range in ranges)
        {
            GameObject go = Instantiate(successRangePrefab, parent);
            SuccessRangeVisual visual = go.GetComponent<SuccessRangeVisual>();
            visual.SetRange(range);
        }
    }
}
