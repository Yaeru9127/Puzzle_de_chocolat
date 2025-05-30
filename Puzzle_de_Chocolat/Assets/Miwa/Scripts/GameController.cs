using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Button nextSceneButton;
    public FadeManager FadeManager;

     void Start()
    {
        if(nextSceneButton !=null)
        {
            nextSceneButton.onClick.AddListener(OnNextSceneButtonClicked);
        }

        if (FadeManager == null)
        {
            FadeManager = FindAnyObjectByType<FadeManager>();
        }

    }
    public void OnNextSceneButtonClicked()
    {
        if (FadeManager ! ==null && ! FadeManager.IsFading())
        {
            FadeManager.StartFadeoutAndLoadScene("game");
        }
    }
}
 
 
