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
//        // シングルトンパターン（他スクリプトから呼び出しやすく）
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject); // シーンをまたいでも破棄されない
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

//    // 効果音を再生
//    public void PlaySE(string name)
//    {
//        if (seDict.TryGetValue(name, out AudioClip clip))
//        {
//            seAudioSource.PlayOneShot(clip);
//        }
//        else
//        {
//            Debug.LogWarning($"SE '{name}' が見つかりません");
//        }
//    }

//    // BGMを再生（ループ）
//    public void PlayBGM(string name)
//    {
//        if (bgmDict.TryGetValue(name, out AudioClip clip))
//        {
//            if (bgmAudioSource.clip == clip) return; // 同じBGMは再再生しない
//            bgmAudioSource.clip = clip;
//            bgmAudioSource.loop = true;
//            bgmAudioSource.Play();
//        }
//        else
//        {
//            Debug.LogWarning($"BGM '{name}' が見つかりません");
//        }
//    }

//    // BGM停止
//    public void StopBGM()
//    {
//        bgmAudioSource.Stop();
//    }
//}
////他のスクリプトから呼び出す時

////AudioManager.Instance.PlaySE("click"); // 効果音（click）を再生
////AudioManager.Instance.PlayBGM("main"); // BGM（main）を再生
////AudioManager.Instance.StopBGM();       // BGM停止