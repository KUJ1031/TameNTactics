using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private RotatePoint rotatePoint;
    [SerializeField] private SuccessRangeSpawner spawner;

    [Header("성공 범위 설정")]
    [SerializeField] private List<RotationRange> ranges = new();

    private bool wasInSuccessZone = false;

    private void Start()
    {
        SetSuccessRanges(45f, 90f);
        rotatePoint.SetRanges(ranges);
        spawner.SpawnRanges(ranges);
    }

    private void Update()
    {
        bool current = rotatePoint.isInSuccessZone;

        if (current && !wasInSuccessZone)
        {
            //성공
        }
        else if (!current && wasInSuccessZone)
        {
            //실패
        }

        wasInSuccessZone = current;
    }

    //단일범위지정
    public void SetSuccessRanges(float min, float max)
    {
        ranges.Clear();
        ranges.Add(new RotationRange(min, max));
    }
    //여러 범위지정
    public void SetSuccessRanges(List<RotationRange> newRanges)
    {
        ranges = newRanges;
    }
}
