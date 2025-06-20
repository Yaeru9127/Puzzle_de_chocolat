using UnityEngine;
using UnityEngine.UI; // UI要素（Slider）を使うために必要

public class VolumeSliderController : MonoBehaviour
{
    public Slider bgmVolumeSlider; // BGM用UIスライダーをInspectorで割り当てる
    public Slider seVolumeSlider; // SFX用UIスライダーをInspectorで割り当てる

    void Start()
    {
        // スライダーの初期値を設定し、UIからの変更イベントを購読
        // PlayerPrefsから以前の値をロードするか、デフォルト値（0.75f）を使用
        if (bgmVolumeSlider != null)
        {
            float savedBGMVolume = PlayerPrefs.GetFloat("SavedBGMVolume", 0.75f);
            bgmVolumeSlider.value = savedBGMVolume;
            bgmVolumeSlider.onValueChanged.AddListener(SetBGMVolumeFromSlider);
            SetBGMVolumeFromSlider(savedBGMVolume); // 初期ロード時にAudioManagerに設定
        }

        if (seVolumeSlider != null)
        {
            float savedSEVolume = PlayerPrefs.GetFloat("SavedSEVolume", 0.75f);
            seVolumeSlider.value = savedSEVolume;
            seVolumeSlider.onValueChanged.AddListener(SetSEVolumeFromSlider);
            SetSEVolumeFromSlider(savedSEVolume); // 初期ロード時にAudioManagerに設定
        }
    }

    // BGMスライダーの値が変更されたときにAudioManagerのBGM音量設定を呼び出す
    void SetBGMVolumeFromSlider(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetBGMVolume(volume);
            PlayerPrefs.SetFloat("SavedBGMVolume", volume); // 設定を保存
        }
    }

    // SFXスライダーの値が変更されたときにAudioManagerのSFX音量設定を呼び出す
    void SetSEVolumeFromSlider(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSEVolume(volume);
            PlayerPrefs.SetFloat("SavedSEVolume", volume); // 設定を保存
        }
    }
}
