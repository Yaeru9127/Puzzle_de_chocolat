using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic; // Dictionaryを使うために必要
using UnityEngine.SceneManagement; // シーンのロードイベントを監視するために必要

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Mixer")]
    public AudioMixer gameAudioMixer;

    [Header("BGM Settings")]
    public AudioSource bgmAudioSource;
    public AudioClip[] bgmClips;

    // ★追加: シーン名とBGMインデックスを紐付けるための設定
    [System.Serializable] // Unity Inspectorで表示可能にする
    public class SceneBGMEntry
    {
        public string sceneName; // シーンの名前 (Build Settingsに登録された名前と一致)
        public int bgmIndex;     // bgmClips配列のインデックス
        public bool loopBGM = true; // そのBGMをループさせるか
    }
    public SceneBGMEntry[] sceneBGMEntries; // シーンごとのBGM設定リスト

    [Header("SE Settings")]
    public AudioSource seAudioSource;
    public AudioClip[] seClips;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return; // 既存のインスタンスがある場合は処理を終了
        }

        // ★追加: シーンがロードされた時のイベントを購読
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // ★追加: シーンがロードされた時のイベント購読を解除 (DontDestroyOnLoadを使う場合は重要)
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // ★追加: シーンがロードされたときに呼び出されるメソッド
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ロードされたシーン名に基づいてBGMを再生
        PlayBGMForScene(scene.name);
    }


    public void PlayBGM(int index, bool loop = true)
    {
        if (bgmAudioSource == null || bgmClips == null || index >= bgmClips.Length || index < 0)
        {
            Debug.LogWarning("BGM AudioSource, bgmClips, または指定されたBGMインデックスが無効です。");
            return;
        }

        // 現在のBGMと同じであれば再生し直さない
        if (bgmAudioSource.clip == bgmClips[index] && bgmAudioSource.isPlaying)
        {
            return;
        }

        bgmAudioSource.clip = bgmClips[index];
        bgmAudioSource.loop = loop;
        bgmAudioSource.Play();
    }

    // ★追加: シーン名に基づいてBGMを再生するプライベートメソッド
    private void PlayBGMForScene(string sceneName)
    {
        foreach (var entry in sceneBGMEntries)
        {
            if (entry.sceneName == sceneName)
            {
                PlayBGM(entry.bgmIndex, entry.loopBGM);
                return; // 該当するBGMが見つかったら終了
            }
        }
        // 該当するシーンのBGM設定がなければBGMを停止
        StopBGM();
        Debug.Log("シーン「" + sceneName + "」に対応するBGM設定が見つかりませんでした。BGMを停止します。");
    }

    public void StopBGM()
    {
        if (bgmAudioSource != null && bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Stop();
        }
    }

    public void PlaySEAudioClip(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("再生しようとしたSEのAudioClipがnullです。");
            return;
        }
        if (seAudioSource != null)
        {
            seAudioSource.PlayOneShot(clip);
        }
        else if (bgmAudioSource != null)
        {
            Debug.LogWarning("SE AudioSourceが設定されていません。BGM AudioSourceでSEを再生します。", this);
            bgmAudioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("SEを再生するためのAudioSourceがありません。", this);
        }
    }

    /*public void PlaySE(int index)
    {
        if (seClips == null || index >= seClips.Length || index < 0)
        {
            Debug.LogWarning("seClips配列または指定されたSEインデックスが無効です。インデックス: " + index, this);
            return;
        }
        AudioClip clipToPlay = seClips[index];
        PlaySE(clipToPlay);
    }*/

    public void PlaySE(string clipName)
    {
        if (string.IsNullOrEmpty(clipName))
        {
            Debug.LogWarning("再生しようとしたSEのクリップ名が空またはnullです。");
            return;
        }

        AudioClip foundClip = null;
        if (seClips != null)
        {
            foreach (AudioClip clip in seClips)
            {
                if (clip != null && clip.name == clipName)
                {
                    foundClip = clip;
                    break;
                }
            }
        }

        if (foundClip != null)
        {
            PlaySEAudioClip(foundClip);
        }
        else
        {
            Debug.LogWarning("指定された名前のSEクリップが見つかりませんでした: " + clipName, this);
        }
    }

    public void SetBGMVolume(float volume)
    {
        if (gameAudioMixer != null)
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
