using System.Security.Cryptography;
using Unity.Jobs;
using UnityEngine;


using UnityEngine.UI;

public class VolumeSliderController : MonoBehaviour
{

    public Slider bgmVolumeSlider;
    public Slider seVolumeSlider;  
    void Start()
    {
        if (bgmVolumeSlider != null)
        {
            float savedBGMVolume = PlayerPrefs.GetFloat("SaveBGMVolume", 1f);//スライダーの初期値
            bgmVolumeSlider.value = savedBGMVolume;
            bgmVolumeSlider.onValueChanged.AddListener(SetBGMVolumeFromSlider);
            SetBGMVolumeFromSlider(savedBGMVolume);
        }
        if(seVolumeSlider !=null)
        {
            float savedSEVolume = PlayerPrefs.GetFloat("SavedSEVolume", 1f);//スライダーの初期値
            seVolumeSlider.value = savedSEVolume;
            seVolumeSlider.onValueChanged.AddListener(SetSEVolumeFromSlider);
            SetSEVolumeFromSlider(savedSEVolume);
        }
    }

    void SetBGMVolumeFromSlider(float volume)
    {
        if(AudioManager.Instance !=null)
        {
            AudioManager.Instance.SetBGMVolume(volume);
            PlayerPrefs.SetFloat("SavedBGMVolume", volume);
            PlayerPrefs.Save();
        }
    }

    void SetSEVolumeFromSlider(float volume)
    {
        if(AudioManager.Instance !=null)
        {
            AudioManager.Instance.SetSEVolume(volume);
            PlayerPrefs.SetFloat("SavedSEVolume", volume);
            PlayerPrefs.Save();
        }
    }
}
