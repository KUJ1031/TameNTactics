using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : Singleton<CameraController>
{
    public CinemachineVirtualCamera CurrentVCam { get; private set; }
    private CinemachineBasicMultiChannelPerlin perlin;

    private List<CinemachineVirtualCamera> allCameras = new();
    private Dictionary<string, CinemachineVirtualCamera> cameraDict = new();

    private float shakeTime;
    private float totalShakeTime;
    private float startAmplitude;
    private float startFrequency;

    private Coroutine zoomCoroutine;
    private float originalOrthoSize;
    private bool isZooming = false;

    private Coroutine rotationCoroutine;

    protected override bool IsDontDestroy => true;

    private void Awake()
    {
        base.Awake(); // Singleton Awake
        CacheAllVirtualCameras();
    }

    private void Update()
    {
        if (shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;

            float t = 1f - (shakeTime / totalShakeTime);
            perlin.m_AmplitudeGain = Mathf.Lerp(startAmplitude, 0, t);
            perlin.m_FrequencyGain = Mathf.Lerp(startFrequency, 0, t);

            if (shakeTime <= 0f)
            {
                ShakeStop();
            }
        }
        RefreshCameraList();
    }

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
                CurrentVCam = GameObject.Find("PlayerCamera")?.GetComponent<CinemachineVirtualCamera>();
                Transform playerTransform = GameObject.Find("Player")?.transform;
                SetTarget(playerTransform);
                break;

            case "BattleScene":
                CurrentVCam = GameObject.Find("Virtual Camera")?.GetComponent<CinemachineVirtualCamera>();
                originalOrthoSize = 5f;
                break;
        }

        if (CurrentVCam != null)
        {
            perlin = CurrentVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            originalOrthoSize = CurrentVCam.m_Lens.OrthographicSize;
        }
        else
        {
            Debug.LogWarning("해당 씬에서 사용할 카메라를 찾을 수 없습니다.");
        }
    }

    /// <summary>
    /// 카메라의 타겟을 변경합니다.
    /// </summary>
    public void SetTarget(Transform target)
    {
        if (CurrentVCam != null && target != null)
        {
            CurrentVCam.Follow = target;
            CurrentVCam.LookAt = target;
        }
    }

    #region Shake
    /// <summary>
    /// 카메라 흔들림을 시작합니다.
    /// </summary>
    public void Shake(float time, float amplitude = 1f, float frequency = 1f)
    {
        if (shakeTime > time) return;

        shakeTime = time;
        totalShakeTime = time;
        startAmplitude = amplitude;
        startFrequency = frequency;

        if (perlin == null && CurrentVCam != null)
            perlin = CurrentVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (perlin != null)
        {
            perlin.m_AmplitudeGain = amplitude;
            perlin.m_FrequencyGain = frequency;
        }
    }

    /// <summary>
    /// 카메라 흔들림을 중단합니다.
    /// </summary>
    public void ShakeStop()
    {
        shakeTime = 0;
        if (perlin != null)
        {
            perlin.m_AmplitudeGain = 0;
            perlin.m_FrequencyGain = 0;
        }
    }
    #endregion

    #region Zoom
    /// <summary>
    /// 카메라를 특정 OrthographicSize로 줌합니다.
    /// </summary>
    public void Zoom(float targetSize, float duration)
    {
        if (CurrentVCam == null) return;

        if (!isZooming)
            originalOrthoSize = CurrentVCam.m_Lens.OrthographicSize;

        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);

        zoomCoroutine = StartCoroutine(ZoomCoroutine(targetSize, duration));
    }

    /// <summary>
    /// 카메라 줌을 원래 크기로 되돌립니다.
    /// </summary>
    public void ResetZoom(float duration = 0.3f)
    {
        if (CurrentVCam == null) return;

        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);

        zoomCoroutine = StartCoroutine(ZoomCoroutine(originalOrthoSize, duration));
    }

    public IEnumerator ZoomCoroutine(float targetSize, float duration)
    {
        isZooming = true;

        float startSize = CurrentVCam.m_Lens.OrthographicSize;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            CurrentVCam.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, t);
            yield return null;
        }

        CurrentVCam.m_Lens.OrthographicSize = targetSize;
        zoomCoroutine = null;
        isZooming = false;
    }
    #endregion

    #region Rotation


    public void RotateTo(float targetRotation, float duration)
    {
        if (CurrentVCam == null) return;

        if (rotationCoroutine != null)
            StopCoroutine(rotationCoroutine);

        rotationCoroutine = StartCoroutine(RotateCoroutine(targetRotation, duration));
    }

    public void ResetRotation(float duration)
    {
        RotateTo(0f, duration);
    }

    private IEnumerator RotateCoroutine(float targetRotation, float duration)
    {
        float startRotation = CurrentVCam.m_Lens.Dutch;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            float current = Mathf.Lerp(startRotation, targetRotation, t);
            CurrentVCam.m_Lens.Dutch = Mathf.Clamp(current, -180f, 180f);
            yield return null;
        }

        CurrentVCam.m_Lens.Dutch = Mathf.Clamp(targetRotation, -180f, 180f);
        rotationCoroutine = null;
    }
    #endregion
    public void Kick(float kickSize = -2f, float totalDuration = 1f, Action onComplete = null)
    {
        if (CurrentVCam == null) return;
        float kickTarget = CurrentVCam.m_Lens.OrthographicSize + kickSize;
        float halfDuration = totalDuration * 0.5f;


        float ran = UnityEngine.Random.Range(20f, 40f);
        float r = UnityEngine.Random.Range(0, 2);
        if (r == 1) { ran *= -1; }
        RotateTo(ran, totalDuration);

        // 줌 아웃
        Zoom(0.5f, halfDuration / 2);
        Zoom(kickTarget, halfDuration);

        // 복구 예약
        StartCoroutine(ResetZoomDelayed(0.4f, onComplete));
    }

    private IEnumerator ResetZoomDelayed(float resetDelay, Action onComplete)
    {
        yield return new WaitForSeconds(0.01f);
        ResetZoom(resetDelay);
        ResetRotation(resetDelay);
        yield return new WaitForSeconds(0.01f);
        onComplete?.Invoke();
    }

    public void RefreshCameraList()
    {
        allCameras.Clear();
        cameraDict.Clear();

        var foundCams = FindObjectsOfType<CinemachineVirtualCamera>(true);
        foreach (var cam in foundCams)
        {
            allCameras.Add(cam);
            cameraDict[cam.name] = cam;
        }
    }


    private void CacheAllVirtualCameras()
    {
        allCameras.Clear();
        cameraDict.Clear();

        var cams = GetComponentsInChildren<CinemachineVirtualCamera>(true); // 폴더 안에 있는 카메라도 포함
        foreach (var cam in cams)
        {
            allCameras.Add(cam);
            cameraDict[cam.name] = cam;
        }
    }

    /// <summary>
    /// 이름으로 카메라 전환
    /// </summary>
    public void SwitchTo(string cameraName, bool forceClearTarget = false, bool canPlayerMove = false)
    {
        if (cameraDict.TryGetValue(cameraName, out var cam))
        {
            SwitchTo(cam, forceClearTarget, canPlayerMove);
        }
        else
        {
            Debug.LogWarning($"카메라 이름 '{cameraName}'을 찾을 수 없습니다.");
        }
    }


    /// <summary>
    /// 인덱스로 카메라 전환
    /// </summary>
    public void SwitchTo(int index)
    {
        if (index >= 0 && index < allCameras.Count)
        {
            SwitchTo(allCameras[index]);
        }
        else
        {
            Debug.LogWarning($"카메라 인덱스 {index}가 범위를 벗어났습니다.");
        }
    }

    /// <summary>
    /// 실제 카메라 전환 처리
    /// </summary>
    private void SwitchTo(CinemachineVirtualCamera targetCam, bool forceClearTarget = false, bool canPlayerMove = false)
    {
        if (targetCam == null) return;

        // 플레이어 입력 잠금
        if (PlayerManager.Instance?.playerController != null)
            PlayerManager.Instance.playerController.isInputBlocked = true;

        Transform currentTarget = CurrentVCam?.Follow;

        foreach (var cam in allCameras)
            cam.Priority = 0;

        targetCam.Priority = 10;
        CurrentVCam = targetCam;

        if (forceClearTarget)
        {
            CurrentVCam.Follow = null;
            CurrentVCam.LookAt = null;
        }
        else if (currentTarget != null)
        {
            CurrentVCam.Follow = currentTarget;
            CurrentVCam.LookAt = currentTarget;
        }

        perlin = CurrentVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        originalOrthoSize = CurrentVCam.m_Lens.OrthographicSize;

        if (!canPlayerMove)
        {
            StartCoroutine(UnblockPlayerInputAfterDelay(3f));
        }
    }

    private IEnumerator UnblockPlayerInputAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayerManager.Instance.playerController.isInputBlocked = false;
    }



}
