using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class feade : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Change_Button()
    {
        SceneManager.LoadScene("game");
    }

}
