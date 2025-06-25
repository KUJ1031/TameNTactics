using UnityEngine;
using UnityEngine.UI;

public class SuccessRangeVisual : MonoBehaviour
{
    [SerializeField] private Image image;

    //주어진 범위(range)만큼 이미지 셋팅
    public void SetRange(RotationRange range)
    {
        float min = Normalize360(range.Min);
        float max = Normalize360(range.Max);
        float delta = (max >= min) ? max - min : 360f - (min - max);

        image.fillAmount = delta / 360f;
        transform.localEulerAngles = new Vector3(0, 0, max);
    }

    private float Normalize360(float angle)
    {
        angle %= 360;
        return angle < 0 ? angle + 360 : angle;
    }
}
