using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks; 

public class GameOverController : MonoBehaviour
{
    public static GameOverController over { get; private set; }

    private InputSystem_Actions actions;
    private InputSystem_Manager manager;
    private CursorController cc;
    private StageManager stage;

    public Image gameOverImage;          // GameOver表示用UI
    public float fadeDuration = 1.5f;    // フェードイン時間
    public float waitBeforeRetry = 3.0f; // リトライ前の待機時間

    private bool isGameOver = false;

    private void Awake()
    {
        if (over == null) over = this;
        else if (over != null) Destroy(this.gameObject);
    }

    void Start()
    {
        if (gameOverImage != null)
        {
            // 透明にして非表示
            gameOverImage.color = new Color(1, 1, 1, 0);
            gameOverImage.gameObject.SetActive(false);
        }

        manager = InputSystem_Manager.manager;
        actions = manager.GetActions();
        cc = CursorController.cc;
        stage = StageManager.stage;
    }

    // GameOver演出を開始
    public void ShowGameOver()
    {
        if (isGameOver) return; // すでに実行中なら無視

        manager.PlayerOff(); // プレイヤー操作無効
        isGameOver = true;

        if (gameOverImage != null)
        {
            gameOverImage.gameObject.SetActive(true);
            stage.phase = StageManager.Phase.Result;

            // カーソル表示
            if (cc.instance != null)
                cc.instance.SetActive(true);

            FadeInAndRetry().Forget(); // 非同期で開始
        }
    }

    // フェードインして一定時間後にリトライシーンへ
    private async UniTaskVoid FadeInAndRetry()
    {
        float elapsed = 0f;
        Color color = gameOverImage.color;
        color.a = 0f;

        // フェードイン処理
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            color.a = alpha;
            gameOverImage.color = color;
            await UniTask.Yield(); // フレームを待つ
        }

        // フル表示
        color.a = 1f;
        gameOverImage.color = color;

        // 一定時間待機
        await UniTask.Delay(System.TimeSpan.FromSeconds(waitBeforeRetry));

        // リトライシーンへ
        SceneManager.LoadScene("RetryScene");
    }

    private void OnDestroy()
    {
        // シングルトン解除（次のシーンで再生成可能にする）
        if (over == this) over = null;
    }
}
