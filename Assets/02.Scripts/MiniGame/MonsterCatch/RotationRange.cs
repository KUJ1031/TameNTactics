[System.Serializable]
public struct RotationRange
{
    public float Min;
    public float Max;

    public RotationRange(float min, float max)
    {
        Min = min; Max = max;
    }


    //angle의 값이 Min~Max사이에 있는지 판별
    public bool Contains(float angle)
    {
        angle = NormalizeAngle180(angle);
        float normalizedMin = NormalizeAngle180(Min);
        float normalizedMax = NormalizeAngle180(Max);

        if (normalizedMin <= normalizedMax)
            return angle >= normalizedMin && angle <= normalizedMax;
        else
            return angle >= normalizedMin || angle <= normalizedMax;
    }

    //각도를 -180 ~ 180으로 정규화.
    private float NormalizeAngle180(float angle)
    {
        angle %= 360;
        if (angle > 180) angle -= 360;
        else if (angle < -180) angle += 360;
        return angle;
    }
}
