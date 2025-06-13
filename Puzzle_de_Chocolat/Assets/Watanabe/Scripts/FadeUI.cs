using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeUI : MonoBehaviour
{
    public float fadeDuration = 1f;
    private Image fadeImage;

    private void Awake()
    {
        fadeImage = GetComponent<Image>();
        fadeImage.color = new Color(0f, 0f, 0f, 1f); // 最初は黒（完全に不透明）
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeOut()
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = t / fadeDuration;
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;
    }

    public IEnumerator FadeIn()
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = 1f - (t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 0f;
        fadeImage.color = c;
    }
}
