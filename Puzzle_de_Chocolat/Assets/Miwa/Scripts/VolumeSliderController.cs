using UnityEngine;
using UnityEngine.UI; // UI�v�f�iSlider�j���g�����߂ɕK�v

public class VolumeSliderController : MonoBehaviour
{
    public Slider bgmVolumeSlider; // BGM�pUI�X���C�_�[��Inspector�Ŋ��蓖�Ă�
    public Slider seVolumeSlider; // SFX�pUI�X���C�_�[��Inspector�Ŋ��蓖�Ă�

    void Start()
    {
        // �X���C�_�[�̏����l��ݒ肵�AUI����̕ύX�C�x���g���w��
        // PlayerPrefs����ȑO�̒l�����[�h���邩�A�f�t�H���g�l�i0.75f�j���g�p
        if (bgmVolumeSlider != null)
        {
            float savedBGMVolume = PlayerPrefs.GetFloat("SavedBGMVolume", 0.75f);
            bgmVolumeSlider.value = savedBGMVolume;
            bgmVolumeSlider.onValueChanged.AddListener(SetBGMVolumeFromSlider);
            SetBGMVolumeFromSlider(savedBGMVolume); // �������[�h����AudioManager�ɐݒ�
        }

        if (seVolumeSlider != null)
        {
            float savedSEVolume = PlayerPrefs.GetFloat("SavedSEVolume", 0.75f);
            seVolumeSlider.value = savedSEVolume;
            seVolumeSlider.onValueChanged.AddListener(SetSEVolumeFromSlider);
            SetSEVolumeFromSlider(savedSEVolume); // �������[�h����AudioManager�ɐݒ�
        }
    }

    // BGM�X���C�_�[�̒l���ύX���ꂽ�Ƃ���AudioManager��BGM���ʐݒ���Ăяo��
    void SetBGMVolumeFromSlider(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetBGMVolume(volume);
            PlayerPrefs.SetFloat("SavedBGMVolume", volume); // �ݒ��ۑ�
        }
    }

    // SFX�X���C�_�[�̒l���ύX���ꂽ�Ƃ���AudioManager��SFX���ʐݒ���Ăяo��
    void SetSEVolumeFromSlider(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSEVolume(volume);
            PlayerPrefs.SetFloat("SavedSEVolume", volume); // �ݒ��ۑ�
        }
    }
}
