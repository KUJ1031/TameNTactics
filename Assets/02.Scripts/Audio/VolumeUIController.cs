using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class VolumeUIController : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI bgmText;
    [SerializeField] private TextMeshProUGUI sfxText;

    private const string BGMVolumeKey = "BGMVolume";
    private const string SFXVolumeKey = "SFXVolume";

    private void Start()
    {
        // 저장된 값 불러오기, 없으면 1f 사용
        float savedBGM = PlayerPrefs.GetFloat(BGMVolumeKey, 1f);
        float savedSFX = PlayerPrefs.GetFloat(SFXVolumeKey, 1f);

        bgmSlider.value = savedBGM;
        sfxSlider.value = savedSFX;

        // AudioManager에 초기 볼륨 설정
        AudioManager.Instance.SetBGMVolume(savedBGM);
        AudioManager.Instance.SetSFXVolume(savedSFX);

        // 이벤트 등록
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    private void OnBGMVolumeChanged(float value)
    {
        AudioManager.Instance.SetBGMVolume(value);
        PlayerPrefs.SetFloat(BGMVolumeKey, value);

        bgmText.text = Math.Round(value*100f).ToString()+"%";
    }

    private void OnSFXVolumeChanged(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
        PlayerPrefs.SetFloat(SFXVolumeKey, value);
        sfxText.text = Math.Round(value * 100f).ToString() + "%";
    }
}
