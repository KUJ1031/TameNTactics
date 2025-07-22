using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : Singleton<CameraController>
{
    private CinemachineBrain brain;
    public CinemachineVirtualCamera CurrentVCam { get; private set; }
    private CinemachineBasicMultiChannelPerlin perlin;

    private float shakeTime;
    private float totalShakeTime;
    private float startAmplitude;
    private float startFrequency;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMapScene":
                CurrentVCam = GameObject.Find("PlayerCamera")?.GetComponent<Cinemachine.CinemachineVirtualCamera>();
                Transform playerTransform = GameObject.Find("Player")?.transform;
                CurrentVCam.Follow = playerTransform;
                CurrentVCam.LookAt = playerTransform;
                break;
            case "BattleScene":
                CurrentVCam = GameObject.Find("BattleCamera")?.GetComponent<Cinemachine.CinemachineVirtualCamera>();
                break;
        }

        if (CurrentVCam == null)
            Debug.LogWarning("해당 씬에서 사용할 카메라를 찾을 수 없습니다.");
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

    /// <summary>
    /// 화면을 흔듭니다 시간(초), 진폭, 주파수 를 받습니다
    /// </summary>
    /// <param name="time"></param>
    /// <param name="amplitude"></param>
    /// <param name="frequency"></param>
    public void Shake(float time, float amplitude = 1f, float frequency = 1f)
    {
        if (shakeTime > time) return;

        shakeTime = time;
        totalShakeTime = time;
        startAmplitude = amplitude;
        startFrequency = frequency;

        if (perlin == null)
            perlin = CurrentVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

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
