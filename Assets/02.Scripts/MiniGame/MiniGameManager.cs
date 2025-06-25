using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private RotatePoint rotatePoint;
    [SerializeField] private SuccessRangeSpawner spawner;

    [Header("성공 범위")]
    [SerializeField] private List<RotationRange> ranges = new();

    private bool wasInSuccessZone = false;

    private void Start()
    {
        ranges.Clear();
        //아래를 기준으로 시계 방향으로 0~360
        ranges.Add(new RotationRange(10, 30));
        ranges.Add(new RotationRange(40, 80));
        ranges.Add(new RotationRange(340, 350));

        rotatePoint.SetRanges(ranges);
        spawner.SpawnRanges(ranges);
    }

    private void Update()
    {
        bool current = rotatePoint.isInSuccessZone;

        if (current && !wasInSuccessZone)
        {
            Debug.Log("성공");
            //성공
        }
        else if (!current && wasInSuccessZone)
        {
            Debug.Log("실패");
            //실패
        }

        wasInSuccessZone = current;

        if (Input.GetKey(KeyCode.A))
        {

        }
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
