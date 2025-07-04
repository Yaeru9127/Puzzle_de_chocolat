using UnityEngine;

public class ReloadCountManager : MonoBehaviour
{
    public static ReloadCountManager Instance { get; private set; }

    public int ReloadCount { get; private set; } = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // アプリ起動時にリロード回数を0で初期化
        ReloadCount = 0;
    }

    // リロード回数を1増やす
    public void IncrementReloadCount()
    {
        ReloadCount++;
    }

    // リロード回数をリセット（必要なら任意のタイミングで呼び出し可）
    public void ResetReloadCount()
    {
        ReloadCount = 0;
    }
}
