using System.Collections.Generic;
using UnityEngine;

public class RotatePoint : MonoBehaviour
{
    [SerializeField] private RectTransform point; // 회전시킬 대상
    [SerializeField] private float rotateSpeed = 1f; // 한 바퀴 도는 데 걸리는 시간(초)

    [field: SerializeField] public bool isInSuccessZone { get; private set; }

    private float rotationSpeedDegree; // 초당 회전 각도
    private List<RotationRange> successRanges = new();

    private void Start()
    {
        UpdateRotationSpeed();
    }

    private void Update()
    {
        if (point == null || rotateSpeed == 0f)
            return;

        Vector3 curRotation = point.localEulerAngles;
        curRotation.z -= rotationSpeedDegree * Time.deltaTime; // 반시계 회전
        point.localEulerAngles = curRotation;
        
        CheckSuccessZone();
    }
    //성공범위 설정
    public void SetRanges(List<RotationRange> ranges)
    {
        successRanges = ranges;
    }

    //회전속도 계산
    private void UpdateRotationSpeed()
    {
        rotationSpeedDegree = (rotateSpeed > 0f) ? 360f / rotateSpeed : 0f;
    }

    //성공 범위에 있는지 확인
    private void CheckSuccessZone()
    {
        isInSuccessZone = false;
        float currentZ = point.localEulerAngles.z;

        foreach (var range in successRanges)
        {
            if (range.Contains(currentZ))
            {
                isInSuccessZone = true;
                break;
            }
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateRotationSpeed(); // 에디터에서 rotateSpeed 바꿨을 때 즉시 반영
    }
#endif
}
