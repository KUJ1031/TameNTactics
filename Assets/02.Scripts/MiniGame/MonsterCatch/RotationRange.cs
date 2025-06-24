[System.Serializable]
public struct RotationRange
{
    public float Min;
    public float Max;

    public RotationRange(float min, float max)
    {
        Min = min; Max = max;
    }

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

    private float NormalizeAngle180(float angle)
    {
        angle %= 360;
        if (angle > 180) angle -= 360;
        else if (angle < -180) angle += 360;
        return angle;
    }
}
