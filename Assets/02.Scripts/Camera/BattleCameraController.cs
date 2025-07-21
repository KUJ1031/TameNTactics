using Cinemachine;
using UnityEngine;

public class BattleCameraController : Singleton<BattleCameraController>
{
    [SerializeField] private CinemachineBrain brain;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlin;

    private float shakeTime;
    private float totalShakeTime;
    private float startAmplitude;
    private float startFrequency;

    protected override void Awake()
    {
        base.Awake();
        if (virtualCamera != null)
            perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
        Shake(1f, 3f, 2f);
    }

    private void Update()
    {
        if (shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;

            float t = 1f - (shakeTime / totalShakeTime); // 0 → 1
            // 부드러운 감쇠
            perlin.m_AmplitudeGain = Mathf.Lerp(startAmplitude, 0, t);
            perlin.m_FrequencyGain = Mathf.Lerp(startFrequency, 0, t);

            if (shakeTime <= 0f)
            {
                ShakeStop();
            }
        }
    }

    public void Shake(float time, float amplitude = 1f, float frequency = 1f)
    {
        if (shakeTime > time) return;

        shakeTime = time;
        totalShakeTime = time;
        startAmplitude = amplitude;
        startFrequency = frequency;

        if (perlin == null)
            perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        perlin.m_AmplitudeGain = amplitude;
        perlin.m_FrequencyGain = frequency;
    }

    public void ShakeStop()
    {
        shakeTime = 0;
        if (perlin != null)
        {
            perlin.m_AmplitudeGain = 0;
            perlin.m_FrequencyGain = 0;
        }
    }
}
