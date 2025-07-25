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

    protected override void Awake()
    {
        base.Awake();
        bgmDict = bgmDataList.ToDictionary(data => data.clipName, data => data);
        sfxDict = sfxDataList.ToDictionary(data => data.clipName, data => data);
        InitializeSFXPool();
    }
 


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

        // 모두 재생 중이면 첫 번째를 재사용 (필요 시 정책 수정 가능)
        return sfxSources.Count > 0 ? sfxSources[0] : null;
    }
    public void PlayBGM(string name)
    {
        if (TryGetAudio(name, SoundType.BGM, out AudioData data))
        {
            bgmSource.clip = data.clip;
            bgmSource.loop = data.loop;
            bgmSource.volume = data.volume * bgmVolume;
            bgmSource.Play();
        }
    }

    public void FadeInBGM(string name, float duration)
    {
        if (TryGetAudio(name, SoundType.BGM, out AudioData data))
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeInRoutine(data, duration));
        }
    }

    public void CrossFadeBGM(string name, float duration)
    {
        if (TryGetAudio(name, SoundType.BGM, out AudioData nextData))
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(CrossFadeRoutine(nextData, duration));
        }
    }
    #endregion

    #region Stop
    public void StopBGM()
    {
        bgmSource.Stop();
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
        sfxSource.mute = mute;
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
    }

    private IEnumerator CrossFadeRoutine(AudioData nextData, float duration)
    {
        float startVolume = bgmSource.volume;
        float time = 0f;

        while (time < duration)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        bgmSource.clip = nextData.clip;
        bgmSource.loop = nextData.loop;
        bgmSource.volume = 0f;
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

        //if (bgmSource.isPlaying)
        //{
        // FadeOutBGM(1f);
        //}

        switch (scene.name)
        {
            case "StartScene":
                FadeInBGM("StartScene", 1f);
                break;
            case "MainMapScene":
                FadeInBGM("Village", 1f);
                break;
            case "BattleScene":
                FadeInBGM("Battle", 1f);
                break;
            default:
                StopBGM();
                break;
        }
    }
}
