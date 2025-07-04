using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEditor.Experimental.GraphView;


public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance { get; private set; }

    [SerializeField] private Image fadePanel;//fade‚·‚é‚Æ‚«‚ÉŽg‚¤UI
    [SerializeField] private float fadeDuration = 1.0f;//fade‚ÌŽ‘±ŽžŠÔ
    [SerializeField] private Ease fadeEase = Ease.Linear;



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
            Debug.LogWarning("FadePanel‚ªÝ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñ");
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
            Debug.LogWarning("FadePanel‚ªÝ’è‚³‚ê‚Ä‚Ü‚¹‚ñ");
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
            SceneManager.LoadScene(game);
        });
    }
}
