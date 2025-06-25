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

        SetSuccessRanges(90);

        rotatePoint.SetRanges(ranges);
        spawner.SpawnRanges(ranges);
    }

    private void Update()
    {
        bool current = rotatePoint.isInSuccessZone;

        if (current && !wasInSuccessZone)
        {
            Debug.Log("성공");
        }
        else if (!current && wasInSuccessZone)
        {
            Debug.Log("실패");
        }

        wasInSuccessZone = current;

        if (Input.GetKey(KeyCode.Space))//추후 인풋 변경
        {
            rotatePoint.SetRotateSpeed(0);
            //rotatePoint.isInSuccessZone 값을 전달(성공/실패)
        }
    }

    //랜덤 범위지정
    public void SetSuccessRanges(float value)
    {
        if (value <= 0) { Debug.Log("SetSuccessRanges의 value가 0이하 입니다."); }

        float min = Random.Range(-180, 180);
        float max = min + value;
        ranges.Add(new RotationRange(min, max));
    }
    //여러 범위지정
    //public void SetSuccessRanges(List<RotationRange> newRanges)
    //{
    //    ranges = newRanges;
    //}
}
