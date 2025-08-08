using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    protected override bool IsDontDestroy => true;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Data List")]
    [SerializeField] private List<AudioData> bgmDataList;
    [SerializeField] private List<AudioData> sfxDataList;
    private float bgmVolume = 1f;
    private float sfxVolume = 1f;
    private Coroutine fadeCoroutine;

    private Dictionary<string, AudioData> bgmDict;
    private Dictionary<string, AudioData> sfxDict;

    [SerializeField] private int sfxPoolSize = 10;
    private List<AudioSource> sfxSources = new List<AudioSource>();

    private string currentPlayingBGMName;

    protected override void Awake()
    {
        base.Awake();
        bgmDict = bgmDataList.ToDictionary(data => data.clipName, data => data);
        sfxDict = sfxDataList.ToDictionary(data => data.clipName, data => data);
        InitializeSFXPool();
    }

    //SFX사운드풀링
    private void InitializeSFXPool()
    {
        for (int i = 0; i < sfxPoolSize; i++)
        {
            var sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
            sfxSources.Add(sfxSource);
        }
    }

    #region Play
    public void PlaySFX(string name)
    {
        if (TryGetAudio(name, SoundType.SFX, out AudioData data))
        {
            AudioSource source = GetAvailableSFXSource();
            if (source != null && data.clip != null)
            {
                source.PlayOneShot(data.clip, data.volume * sfxVolume);
            }
        }
    }
    private AudioSource GetAvailableSFXSource()
    {
        foreach (var source in sfxSources)
        {
            if (!source.isPlaying)
                return source;
        }
        return sfxSources.Count > 0 ? sfxSources[0] : null;
    }

    public void PlayBGM(string name)
    {
        if (currentPlayingBGMName == name && bgmSource.isPlaying)
        {
            return;
        }

        if (TryGetAudio(name, SoundType.BGM, out AudioData data))
        {
            bgmSource.clip = data.clip;
            bgmSource.loop = data.loop;
            bgmSource.volume = data.volume * bgmVolume;
            bgmSource.Play();
            currentPlayingBGMName = name;
        }
    }

    public void FadeInBGM(string name, float duration)
    {

        if (currentPlayingBGMName == name && bgmSource.isPlaying)
        {
            return;
        }

        if (TryGetAudio(name, SoundType.BGM, out AudioData data))
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeInRoutine(data, duration));
            currentPlayingBGMName = name;
        }
    }

    /// <summary>
    /// 다음 BGM과 duration의 시간동안 페이드 인 아웃으로 연결합니다.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="duration"></param>
    public void CrossFadeBGM(string name, float duration)
    {
        if (currentPlayingBGMName == name)
        {
            return;
        }

        if (TryGetAudio(name, SoundType.BGM, out AudioData nextData))
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(CrossFadeRoutine(nextData, duration));
            currentPlayingBGMName = name;
        }
    }
    #endregion

    #region Stop
    public void StopBGM()
    {
        bgmSource.Stop();
        currentPlayingBGMName = null;
    }

    public void StopAllSFX()
    {
        foreach (var source in sfxSources)
        {
            source.Stop();
        }
    }

    public void FadeOutBGM(float duration)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeOutRoutine(duration));
    }
    #endregion

    #region Volume
    public void SetMasterVolume(float volume)
    {
        bgmVolume = volume;
        sfxVolume = volume;

        bgmSource.volume = bgmVolume;
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        bgmSource.volume = bgmVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }
    #endregion

    #region Mute
    public void MuteBGM(bool mute)
    {
        bgmSource.mute = mute;
    }

    public void MuteSFX(bool mute)
    {
        foreach (var source in sfxSources)
        {
            source.mute = mute;
        }
    }
    #endregion

    #region Coroutines
    private IEnumerator FadeInRoutine(AudioData data, float duration)
    {
        bgmSource.clip = data.clip;
        bgmSource.loop = data.loop;
        bgmSource.volume = 0f;
        bgmSource.Play();

        float targetVolume = data.volume * bgmVolume;
        float time = 0f;

        while (time < duration)
        {
            bgmSource.volume = Mathf.Lerp(0f, targetVolume, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        bgmSource.volume = targetVolume;
    }

    private IEnumerator FadeOutRoutine(float duration)
    {
        float startVolume = bgmSource.volume;
        float time = 0f;

        while (time < duration)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        bgmSource.Stop();
        bgmSource.volume = startVolume;
        currentPlayingBGMName = null;
    }

    private IEnumerator CrossFadeRoutine(AudioData nextData, float duration)
    {
        float startVolume = bgmSource.volume;
        float time = 0f;

        if (bgmSource.isPlaying)
        {
            while (time < duration)
            {
                bgmSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
                time += Time.unscaledDeltaTime;
                yield return null;
            }
            bgmSource.Stop(); // 페이드 아웃 완료 후 정지
        }
        else // 현재 BGM이 재생 중이 아니면 바로 다음 BGM으로
        {
            yield return null; // 한 프레임 대기
        }

        // 두 번째 부분: 새 BGM 페이드 인
        bgmSource.clip = nextData.clip;
        bgmSource.loop = nextData.loop;
        bgmSource.volume = 0f; // 0에서 시작
        bgmSource.Play();

        float targetVolume = nextData.volume * bgmVolume;
        time = 0f;

        while (time < duration)
        {
            bgmSource.volume = Mathf.Lerp(0f, targetVolume, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        bgmSource.volume = targetVolume;
    }
    #endregion

    /// <summary>
    /// 이름으로 오디오데이터를 가지고옵니다.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    private bool TryGetAudio(string name, SoundType type, out AudioData data)
    {
        switch (type)
        {
            case SoundType.SFX:
                if (sfxDict.TryGetValue(name, out data)) return true;
                break;
            case SoundType.BGM:
                if (bgmDict.TryGetValue(name, out data)) return true;
                break;
        }
        Debug.LogWarning($"[AudioManager] Audio not found: {name} ({type})");
        data = null;
        return false;
    }

    #region OnSceneLoaded
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
        string nextBGMName = null;
        float fadeInDuration = 5f; // 기본 페이드 인 시간

        switch (scene.name)
        {
            case "StartScene":
                nextBGMName = "StartScene";
                fadeInDuration = 10f;
                break;
            case "MainMapPuzzleTestScene":
                string stage = PlayerManager.Instance.player.playerLastStage;
                switch (stage)
                {
                    case "시작의 땅":
                    case "초보 사냥터":
                    case "위험한 쉼터":
                    case "잊혀진 공간":
                        nextBGMName = "Forest";
                        break;
                    case "불길한 다리":
                    case "파멸의 성 입구":
                    case "파멸의 성":
                    case "결전의 장소":
                        nextBGMName = "Castle";
                        break;
                    case "한적한 마을":
                        nextBGMName = "Village";
                        break;
                }
                break;
            case "BattleScene":
                nextBGMName = "Battle";
                fadeInDuration = 0.5f;
                break;
            default:
                StopBGM();
                break;
        }

        if (!string.IsNullOrEmpty(nextBGMName) && currentPlayingBGMName != nextBGMName)
        {
            if (bgmSource.isPlaying)
            {
                CrossFadeBGM(nextBGMName, fadeInDuration);
            }
            else
            {
                FadeInBGM(nextBGMName, fadeInDuration);
            }
        }
        else if (string.IsNullOrEmpty(nextBGMName))
        {
            StopBGM();
        }
    }
    #endregion
}
