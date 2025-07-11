using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic; // Dictionary���g�����߂ɕK�v
using UnityEngine.SceneManagement; // �V�[���̃��[�h�C�x���g���Ď����邽�߂ɕK�v

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Mixer")]
    public AudioMixer gameAudioMixer;

    [Header("BGM Settings")]
    public AudioSource bgmAudioSource;
    public AudioClip[] bgmClips;

    // ���ǉ�: �V�[������BGM�C���f�b�N�X��R�t���邽�߂̐ݒ�
    [System.Serializable] // Unity Inspector�ŕ\���\�ɂ���
    public class SceneBGMEntry
    {
        public string sceneName; // �V�[���̖��O (Build Settings�ɓo�^���ꂽ���O�ƈ�v)
        public int bgmIndex;     // bgmClips�z��̃C���f�b�N�X
        public bool loopBGM = true; // ����BGM�����[�v�����邩
    }
    public SceneBGMEntry[] sceneBGMEntries; // �V�[�����Ƃ�BGM�ݒ胊�X�g

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
            return; // �����̃C���X�^���X������ꍇ�͏������I��
        }

        // ���ǉ�: �V�[�������[�h���ꂽ���̃C�x���g���w��
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // ���ǉ�: �V�[�������[�h���ꂽ���̃C�x���g�w�ǂ����� (DontDestroyOnLoad���g���ꍇ�͏d�v)
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // ���ǉ�: �V�[�������[�h���ꂽ�Ƃ��ɌĂяo����郁�\�b�h
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���[�h���ꂽ�V�[�����Ɋ�Â���BGM���Đ�
        PlayBGMForScene(scene.name);
    }


    public void PlayBGM(int index, bool loop = true)
    {
        if (bgmAudioSource == null || bgmClips == null || index >= bgmClips.Length || index < 0)
        {
            Debug.LogWarning("BGM AudioSource, bgmClips, �܂��͎w�肳�ꂽBGM�C���f�b�N�X�������ł��B");
            return;
        }

        // ���݂�BGM�Ɠ����ł���΍Đ��������Ȃ�
        if (bgmAudioSource.clip == bgmClips[index] && bgmAudioSource.isPlaying)
        {
            return;
        }

        bgmAudioSource.clip = bgmClips[index];
        bgmAudioSource.loop = loop;
        bgmAudioSource.Play();
    }

    // ���ǉ�: �V�[�����Ɋ�Â���BGM���Đ�����v���C�x�[�g���\�b�h
    private void PlayBGMForScene(string sceneName)
    {
        foreach (var entry in sceneBGMEntries)
        {
            if (entry.sceneName == sceneName)
            {
                PlayBGM(entry.bgmIndex, entry.loopBGM);
                return; // �Y������BGM������������I��
            }
        }
        // �Y������V�[����BGM�ݒ肪�Ȃ����BGM���~
        StopBGM();
        Debug.Log("�V�[���u" + sceneName + "�v�ɑΉ�����BGM�ݒ肪������܂���ł����BBGM���~���܂��B");
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
            Debug.LogWarning("�Đ����悤�Ƃ���SE��AudioClip��null�ł��B");
            return;
        }
        if (seAudioSource != null)
        {
            seAudioSource.PlayOneShot(clip);
        }
        else if (bgmAudioSource != null)
        {
            Debug.LogWarning("SE AudioSource���ݒ肳��Ă��܂���BBGM AudioSource��SE���Đ����܂��B", this);
            bgmAudioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("SE���Đ����邽�߂�AudioSource������܂���B", this);
        }
    }

    /*public void PlaySE(int index)
    {
        if (seClips == null || index >= seClips.Length || index < 0)
        {
            Debug.LogWarning("seClips�z��܂��͎w�肳�ꂽSE�C���f�b�N�X�������ł��B�C���f�b�N�X: " + index, this);
            return;
        }
        AudioClip clipToPlay = seClips[index];
        PlaySE(clipToPlay);
    }*/

    public void PlaySE(string clipName)
    {
        if (string.IsNullOrEmpty(clipName))
        {
            Debug.LogWarning("�Đ����悤�Ƃ���SE�̃N���b�v������܂���null�ł��B");
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
            Debug.LogWarning("�w�肳�ꂽ���O��SE�N���b�v��������܂���ł���: " + clipName, this);
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
