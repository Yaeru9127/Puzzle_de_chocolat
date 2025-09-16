using UnityEngine;
using UnityEngine.SceneManagement;

public class skip : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            SceneManager.LoadScene("StageSelect");
        }
    }
}