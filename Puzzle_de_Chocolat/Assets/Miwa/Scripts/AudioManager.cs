using UnityEngine;
using UnityEngine.Audio; // Audio Mixer���g�����߂ɕK�v

public class AudioManager : MonoBehaviour
{
    // �V���O���g���C���X�^���X
    public static AudioManager Instance { get; private set; }

    [Header("Audio Mixer")]
    public AudioMixer gameAudioMixer; // �쐬����Audio Mixer��Inspector�Ŋ��蓖�Ă�

    [Header("BGM Settings")]
    public AudioSource bgmAudioSource; // BGM�Đ��p��Audio Source��Inspector�Ŋ��蓖�Ă�
    public AudioClip[] bgmClips; // BGM�f�ނ̔z���Inspector�Ŋ��蓖�Ă�

    [Header("SFX Settings")]
    public AudioSource sfxAudioSource; // SE�Đ��p��Audio Source��Inspector�Ŋ��蓖�Ă�
    // �܂��́APlayOneShot()��bgmAudioSource�ő�p����ꍇ�͕s�v

    private void Awake()
    {
        // �V���O���g���p�^�[���̎���
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �V�[���J�ڂ��Ă��j������Ȃ��悤�ɂ���
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �w�肳�ꂽ�C���f�b�N�X��BGM���Đ����܂��B
    /// </summary>
    public void PlayBGM(int index, bool loop = true)
    {
        if (bgmAudioSource == null || bgmClips == null || index < 0 || index >= bgmClips.Length)
        {
            Debug.LogWarning("BGM�ݒ肪�s�����Ă��邩�A�C���f�b�N�X�������ł��B");
            return;
        }

        bgmAudioSource.clip = bgmClips[index];
        bgmAudioSource.loop = loop;
        bgmAudioSource.Play();
    }

    /// <summary>
    /// BGM���~���܂��B
    /// </summary>
    public void StopBGM()
    {
        if (bgmAudioSource != null && bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Stop();
        }
    }

    /// <summary>
    /// �w�肳�ꂽAudioClip��SE���Đ����܂��B(����)
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        // SFX��p��AudioSource������΂�����g��
        if (sfxAudioSource != null)
        {
            sfxAudioSource.PlayOneShot(clip);
        }
        // �Ȃ����BGM�pAudioSource�ő�p����i�����Đ����ɒ��Ӂj
        else if (bgmAudioSource != null)
        {
            bgmAudioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("SE�Đ��p��AudioSource���ݒ肳��Ă��܂���B");
        }
    }

    /// <summary>
    /// BGM�̉��ʂ�ݒ肵�܂� (�X���C�_�[�̒l 0.0�`1.0 ��n��)�B
    /// </summary>
    public void SetBGMVolume(float volume)
    {
        if (gameAudioMixer != null)
        {
            // Audio Mixer�̉��ʂ͑ΐ��X�P�[���Ȃ̂ŕϊ����K�v
            gameAudioMixer.SetFloat("BGMVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
        }
    }

    /// <summary>
    /// SFX�̉��ʂ�ݒ肵�܂� (�X���C�_�[�̒l 0.0�`1.0 ��n��)�B
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        if (gameAudioMixer != null)
        {
            gameAudioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
        }
    }
}

