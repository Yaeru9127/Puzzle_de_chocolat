//using UnityEngine;
//using System.Collections.Generic;

//public class AudioManager : MonoBehaviour
//{
//    public static AudioManager Instance;

//    [Header("SE")]
//    public AudioSource seAudioSource;
//    public List<AudioClip> seClips;

//    [Header("BGM")]
//    public AudioSource bgmAudioSource;
//    public List<AudioClip> bgmClips;

//    private Dictionary<string, AudioClip> seDict = new Dictionary<string, AudioClip>();
//    private Dictionary<string, AudioClip> bgmDict = new Dictionary<string, AudioClip>();

//    private void Awake()
//    {
//        // �V���O���g���p�^�[���i���X�N���v�g����Ăяo���₷���j
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject); // �V�[�����܂����ł��j������Ȃ�
//            InitializeDictionaries();
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    private void InitializeDictionaries()
//    {
//        foreach (AudioClip clip in seClips)
//        {
//            seDict[clip.name] = clip;
//        }

//        foreach (AudioClip clip in bgmClips)
//        {
//            bgmDict[clip.name] = clip;
//        }
//    }

//    // ���ʉ����Đ�
//    public void PlaySE(string name)
//    {
//        if (seDict.TryGetValue(name, out AudioClip clip))
//        {
//            seAudioSource.PlayOneShot(clip);
//        }
//        else
//        {
//            Debug.LogWarning($"SE '{name}' ��������܂���");
//        }
//    }

//    // BGM���Đ��i���[�v�j
//    public void PlayBGM(string name)
//    {
//        if (bgmDict.TryGetValue(name, out AudioClip clip))
//        {
//            if (bgmAudioSource.clip == clip) return; // ����BGM�͍čĐ����Ȃ�
//            bgmAudioSource.clip = clip;
//            bgmAudioSource.loop = true;
//            bgmAudioSource.Play();
//        }
//        else
//        {
//            Debug.LogWarning($"BGM '{name}' ��������܂���");
//        }
//    }

//    // BGM��~
//    public void StopBGM()
//    {
//        bgmAudioSource.Stop();
//    }
//}
////���̃X�N���v�g����Ăяo����

////AudioManager.Instance.PlaySE("click"); // ���ʉ��iclick�j���Đ�
////AudioManager.Instance.PlayBGM("main"); // BGM�imain�j���Đ�
////AudioManager.Instance.StopBGM();       // BGM��~