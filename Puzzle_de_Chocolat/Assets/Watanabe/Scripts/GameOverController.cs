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
    private StageManager stage;

    public Image gameOverImage;
    public float fadeDuration = 1.5f;
    public float waitBeforeRetry = 3.0f;

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

        yield return new WaitForSeconds(waitBeforeRetry);
        SceneManager.LoadScene("RetryScene");
    }

    private void OnDestroy()
    {
        if (over == this) over = null;
    }
}
