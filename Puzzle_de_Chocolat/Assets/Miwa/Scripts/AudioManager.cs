using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [Header("Audio Mixer")]
    public AudioMixer gameAudioMixer; //作成したAudio MixerをInspectorで割り当てる
    [Header("BGM Settings")]
    public AudioSource bgmAudioSource;//BGM再生用のAudio SourceおInspectorで割り当てる
    [Header("SE Settings")]
    public AudioSource seAudioSource;//SE再生用のAudio SourceをInspectorで割り当てる
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
    //BGMの再生
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

    //SEを再生
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
    //BGM・SEのスライダーの値
    public　void SetBGMVolume(float volume)
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
