using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.InputSystem;


public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance { get; private set; }

    [SerializeField] private Image fadePanel;//fadeするときに使うUI
    [SerializeField] private float fadeDuration = 1.0f;//fadeの持続時間
    [SerializeField] private Ease fadeEase = Ease.Linear;

    private InputSystem_Actions action;
    private InputSystem_Manager manager;
    private StageManager stage;
    private CursorController cc;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        if (fadePanel != null)
        {
            Color panelColor = fadePanel.color;
            panelColor.a = 0f;
            fadePanel.color = panelColor;
            fadePanel.gameObject.SetActive(true);
        }

        //action = manager.GetActions();
        stage = StageManager.stage;
        cc = CursorController.cc;

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        FadeIn();
    }

    public void FadeIn(System.Action onComplete = null)
    {
        if (fadePanel == null)
        {
            Debug.LogWarning("FadePanelが設定されていません");
            onComplete?.Invoke();
            return;
        }

        if (!fadePanel.gameObject.activeSelf)
        {
            fadePanel.gameObject.SetActive(true);
        }

        fadePanel.DOFade(0f, fadeDuration).SetEase(fadeEase).OnComplete(() =>
        {
            fadePanel.gameObject.SetActive(false);
            onComplete?.Invoke();
        });

    }

    public void FadeOut(System.Action onComplete = null)
    {
        if (fadePanel == null)
        {
            Debug.LogWarning("FadePanelが設定されてません");
            onComplete?.Invoke();
            return;
        }

        if (!fadePanel.gameObject.activeSelf)
        {
            fadePanel.gameObject.SetActive(true);
        }

        fadePanel.DOFade(1f, fadeDuration).SetEase(fadeEase).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    public void FadegameLodScene(string game)
    {
        FadeOut(() =>
        {
            if (game == "StageSelect") stage.stagenum = 0;
            else if (game == "Stage01") stage.stagenum = 1;

            //manager.GamePadOff();
            //manager.MouseOff();
            //manager.PlayerOff();

            SceneManager.LoadScene(game);
        });
    }

    public void EndGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
