using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [Header("Audio Mixer")]
    public AudioMixer gameAudioMixer; //�쐬����Audio Mixer��Inspector�Ŋ��蓖�Ă�
    [Header("BGM Settings")]
    public AudioSource bgmAudioSource;//BGM�Đ��p��Audio Source��Inspector�Ŋ��蓖�Ă�
    [Header("SE Settings")]
    public AudioSource seAudioSource;//SE�Đ��p��Audio Source��Inspector�Ŋ��蓖�Ă�
    public AudioClip[] bgmClips; 
    public AudioClip[] seClips;


    private void Awake()
    {
        if(Instance ==null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //BGM�̍Đ�
    public void PlayBGM (int index,bool loop  = true)
    {
        if (bgmAudioSource ==null || bgmClips ==null|| index>=bgmClips.Length)
        {
            return;
        }
        bgmAudioSource.clip = bgmClips[index];
        bgmAudioSource.loop = loop;
        bgmAudioSource.Play();

    }
    public void StopBGM()
    {
        if(bgmAudioSource !=null &&bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Stop();
        }
    }

    //SE���Đ�
    public void PlaySE(AudioClip clip)
    {
        if (clip == null) return;
        if(seAudioSource !=null)
        {
            seAudioSource.PlayOneShot(clip);
        }
        else if (bgmAudioSource !=null)
        {
            bgmAudioSource.PlayOneShot(clip);
        }
    }
   
    public void PlaySE(int index)
    {
        if (seClips == null || index >= seClips.Length || index < 0)
            return;
        AudioClip clipTOPlay = seClips[index];
        PlaySE(clipTOPlay);
    }
    //BGM�ESE�̃X���C�_�[�̒l
    public�@void SetBGMVolume(float volume)
    {
        if (gameAudioMixer !=null)
        {
            gameAudioMixer.SetFloat("BGMVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
        }
    }

    public void SetSEVolume(float volume)
    {
        if (gameAudioMixer != null)
        {
            gameAudioMixer.SetFloat("SEVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);

        }
    }
}
