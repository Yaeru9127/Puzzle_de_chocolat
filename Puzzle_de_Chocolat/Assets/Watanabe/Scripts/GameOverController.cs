using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

public class GameOverController : MonoBehaviour
{
    public static GameOverController over { get; private set; }

    private InputSystem_Actions actions;
    private InputSystem_Manager manager;
    private CursorController cc;
    private StageManager stage;

    public Image gameOverImage; // Game Over の UI Image
    public float fadeDuration = 1.5f; // フェードにかかる時間
    public float waitBeforeRetry = 3.0f; // リトライまでの待機時間

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
            gameOverImage.color = new Color(1, 1, 1, 0);
            gameOverImage.gameObject.SetActive(false);
        }

        manager = InputSystem_Manager.manager;
        actions = manager.GetActions();
        cc = CursorController.cc;
        stage = StageManager.stage;
    }

    public void ShowGameOver()
    {
        if (isGameOver) return;

        manager.PlayerOff();

        isGameOver = true;

        if (gameOverImage != null)
        {
            gameOverImage.gameObject.SetActive(true);
            stage.phase = StageManager.Phase.Result;
            if (cc.instance != null) cc.instance.SetActive(true);
            StartCoroutine(FadeInAndRetry());
        }
    }

    private IEnumerator FadeInAndRetry()
    {
        float elapsed = 0f;
        Color color = gameOverImage.color;
        color.a = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            color.a = alpha;
            gameOverImage.color = color;
            yield return null;
        }

        color.a = 1f;
        gameOverImage.color = color;

        // フェード完了後、3秒待ってシーン遷移
        yield return new WaitForSeconds(waitBeforeRetry);
        SceneManager.LoadScene("RetryScene"); // Retry シーン名に応じて変更
    }

    private void OnDestroy()
    {
        //シーンを跨ぐときにメモリから消す
        if (over == this) over = null;
    }
}
