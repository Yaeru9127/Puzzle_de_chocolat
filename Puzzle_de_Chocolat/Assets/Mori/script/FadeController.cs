using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public Image fadePanel;             // �t�F�[�h�p��UI�p�l���iImage�j
    public float fadeDuration = 1.0f;   // �t�F�[�h�̊����ɂ����鎞��

    private void Start()
    {
        // ������Ԃł͓����ɂ��Ă���
        fadePanel.color = new Color(0f, 0f, 0f, 0f);
    }

    public void FadeOutAndLoadScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeOut(string sceneName)
    {
        // �t�F�[�h�A�E�g�J�n
        float elapsedTime = 0f;
        Color startColor = fadePanel.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        // �t�F�[�h�A�E�g
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadePanel.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            yield return null;
        }

        // ���S�Ƀt�F�[�h�A�E�g������V�[�������[�h
        fadePanel.color = endColor;
        SceneManager.LoadScene(sceneName);
    }
}