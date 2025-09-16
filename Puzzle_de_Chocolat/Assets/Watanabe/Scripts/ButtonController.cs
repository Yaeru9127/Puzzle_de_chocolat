using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonController : MonoBehaviour
{
    private StageManager stage;

    public FadeUI fadePanel; // Inspector ‚ÅŠ„‚è“–‚Ä‚é

    private void Start()
    {
        stage = StageManager.stage;
    }

    public void GoToGameScene()
    {
        StartCoroutine(FadeAndLoadScene(stage.gamescene));
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
