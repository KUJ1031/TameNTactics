using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct RotationRange
{
    public float Min; //범위 최소값
    public float Max; //범위 최대값
    public RotationRange(float min, float max)
    {
        Min = min; Max = max;
    }

    //각도체크
    public bool Contains(float angle)
    {
        angle = NormalizeAngle180(angle);
        float normalizedMin = NormalizeAngle180(Min);
        float normalizedMax = NormalizeAngle180(Max);
        if (normalizedMin <= normalizedMax)
        {
            return angle >= normalizedMin && angle <= normalizedMax;
        }
        else
        {
            return angle >= normalizedMin || angle <= normalizedMax;
        }

    }
    private float NormalizeAngle180(float angle)
    {
        angle %= 360; // 0 ~ 360 범위로 먼저 만듬
        if (angle > 180)
        {
            angle -= 360; // 180 초과면 360을 빼서 -180 ~ 180 범위로
        }
        else if (angle < -180)
        {
            angle += 360; // -180 미만이면 360을 더해서 -180 ~ 180 범위로
        }
        return angle;
    }
}
public class RotatePoint : MonoBehaviour
{
    [SerializeField] private RectTransform point; //돌릴 객체
    public float rotateSpeed = 1; //한바퀴 도는데 걸리는 시간
    private float rotationSpeedDegree; //초당 회전 각도
    [field: SerializeField] public bool isInSuccessZone { get; private set; }//성공여부
    [SerializeField] private List<RotationRange> successRanges = new List<RotationRange>();//성공범위 리스트

    private void Update()
    {
        if (point == null)
        {
            Debug.Log("회전할 Point가 할당되지 않았습니다. Inspector창을 확인해주세요");
            return;
        }
        if (rotateSpeed == 0)//0일경우 멈춤
        {
            return;
        }

        rotationSpeedDegree = 360f / rotateSpeed;

        Vector3 curRotation = point.localEulerAngles;
        curRotation.z -= rotationSpeedDegree * Time.deltaTime;
        point.localEulerAngles = curRotation;
        CheckSuccessZone();
    }

    private void CheckSuccessZone()
    {
        isInSuccessZone = false;
        float curRotation = point.localEulerAngles.z;
        foreach (RotationRange range in successRanges)
        {
            if (range.Contains(curRotation))
            {
                isInSuccessZone = true;
                break;
            }
        }
        if (isInSuccessZone)
        {
            Debug.Log("성공");
        }
        else
        {
            Debug.Log("실패");
        }
    }
}
