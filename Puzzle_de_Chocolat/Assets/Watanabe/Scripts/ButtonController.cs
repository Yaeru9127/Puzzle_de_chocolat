using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonController : MonoBehaviour
{
    public FadeUI fadePanel; // Inspector �Ŋ��蓖�Ă�

    public void GoToGameScene()
    {
        StartCoroutine(FadeAndLoadScene("Stage01"));
    }

    public void GoToStageSelect()
    {
        StartCoroutine(FadeAndLoadScene("StageSelect"));
    }

    public void GoToTitle()
    {
        StartCoroutine(FadeAndLoadScene("TitleScene"));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        yield return fadePanel.FadeOut(); 
        SceneManager.LoadScene(sceneName);
    }
}
