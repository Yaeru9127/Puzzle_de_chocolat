using UnityEngine;
using UnityEngine.Audio; // Audio Mixerを使うために必要

public class AudioManager : MonoBehaviour
{
    // シングルトンインスタンス
    public static AudioManager Instance { get; private set; }

    [Header("Audio Mixer")]
    public AudioMixer gameAudioMixer; // 作成したAudio MixerをInspectorで割り当てる

    [Header("BGM Settings")]
    public AudioSource bgmAudioSource; // BGM再生用のAudio SourceをInspectorで割り当てる
    public AudioClip[] bgmClips; // BGM素材の配列をInspectorで割り当てる

    [Header("SFX Settings")]
    public AudioSource sfxAudioSource; // SE再生用のAudio SourceをInspectorで割り当てる
    // または、PlayOneShot()をbgmAudioSourceで代用する場合は不要

    private void Awake()
    {
        // シングルトンパターンの実装
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン遷移しても破棄されないようにする
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 指定されたインデックスのBGMを再生します。
    /// </summary>
    public void PlayBGM(int index, bool loop = true)
    {
        if (bgmAudioSource == null || bgmClips == null || index < 0 || index >= bgmClips.Length)
        {
            Debug.LogWarning("BGM設定が不足しているか、インデックスが無効です。");
            return;
        }

        bgmAudioSource.clip = bgmClips[index];
        bgmAudioSource.loop = loop;
        bgmAudioSource.Play();
    }

    /// <summary>
    /// BGMを停止します。
    /// </summary>
    public void StopBGM()
    {
        if (bgmAudioSource != null && bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Stop();
        }
    }

    /// <summary>
    /// 指定されたAudioClipのSEを再生します。(推奨)
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        // SFX専用のAudioSourceがあればそれを使う
        if (sfxAudioSource != null)
        {
            sfxAudioSource.PlayOneShot(clip);
        }
        // なければBGM用AudioSourceで代用する（同時再生数に注意）
        else if (bgmAudioSource != null)
        {
            bgmAudioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("SE再生用のAudioSourceが設定されていません。");
        }
    }

    /// <summary>
    /// BGMの音量を設定します (スライダーの値 0.0～1.0 を渡す)。
    /// </summary>
    public void SetBGMVolume(float volume)
    {
        if (gameAudioMixer != null)
        {
            // Audio Mixerの音量は対数スケールなので変換が必要
            gameAudioMixer.SetFloat("BGMVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
        }
    }

    /// <summary>
    /// SFXの音量を設定します (スライダーの値 0.0～1.0 を渡す)。
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        if (gameAudioMixer != null)
        {
            gameAudioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
        }
    }
}

