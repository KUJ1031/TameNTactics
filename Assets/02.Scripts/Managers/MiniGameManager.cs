using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private RotatePoint rotatePoint;
    [SerializeField] private SuccessRangeSpawner spawner;

    [Header("성공 범위")]
    [SerializeField] private List<RotationRange> ranges = new();

    private void Start()
    {
        StartMiniGame(10);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))//추후 인풋 변경
        {
            rotatePoint.SetRotateSpeed(0);
            //Debug.Log(rotatePoint.isInSuccessZone);
            //rotatePoint.isInSuccessZone 값을 전달(성공/실패)
        }
    }

    //랜덤 범위지정
    private void SetSuccessRanges(float value)
    {
        if (value <= 0) { Debug.Log("SetSuccessRanges의 value가 0이하 입니다."); }

        float min = Random.Range(-180, 180);
        float max = min + value;
        ranges.Add(new RotationRange(min, max));
    }

    //범위 0~100
    public void StartMiniGame(float percent)
    {
        ranges.Clear();
        float p = percent / 100f * 360f;
        SetSuccessRanges(p);

        rotatePoint.SetRanges(ranges);
        spawner.SpawnRanges(ranges);
    }
}
