using UnityEngine;

public enum SoundType
{
    BGM,
    SFX
}

[CreateAssetMenu(menuName = "Audio/AudioData", fileName = "NewAudioData")]
public class AudioData : ScriptableObject
{
    public string clipName;     // 이름으로 재생할 수 있도록
    public AudioClip clip;      // 실제 클립
    public SoundType type;      // BGM / SFX
    [Range(0f, 1f)]
    public float volume = 1f;   // 클립 개별 볼륨
    public bool loop = false;   // 루프 여부 (BGM 등)
}