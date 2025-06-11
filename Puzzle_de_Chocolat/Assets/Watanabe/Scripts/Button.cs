using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Button : MonoBehaviour
{
    public FadeUI fadePanel; // Inspector �Ŋ��蓖�Ă�

    public void GoToGameScene()
    {
        StartCoroutine(FadeAndLoadScene("gauge"));
    }

    public void GoToStageSelect()
    {
        StartCoroutine(FadeAndLoadScene("StageSelect"));
    }

    public void GoToTitle()
    {
        StartCoroutine(FadeAndLoadScene("Title"));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        yield return fadePanel.FadeOut(); 
        SceneManager.LoadScene(sceneName);
    }
}
