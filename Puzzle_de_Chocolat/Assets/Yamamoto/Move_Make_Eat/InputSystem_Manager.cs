using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputSystem_Manager : MonoBehaviour
{
    public static InputSystem_Manager manager { get; private set; }

    private InputSystem_Actions actions;

    private void Awake()
    {
        if (manager == null) manager = this;
        else if (manager != null) Destroy(this.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //‰Šú‰»
        DontDestroyOnLoad(this.gameObject);
        GetActions();
    }

    /// <summary>
    /// InputSystem_Actions‚ğæ“¾‚·‚éŠÖ”
    /// </summary>
    /// <returns></returns>
    public InputSystem_Actions GetActions()
    {
        if (actions == null) actions = new InputSystem_Actions();

        return actions;
    }

    /// <summary>
    /// GamePad
    /// </summary>
    public void GamePadOn()
    {
        actions.GamePad.Enable();
    }

    public void GamePadOff()
    {
        actions.GamePad.Disable();
    }

    /// <summary>
    /// Mouse
    /// </summary>
    public void MouseOn()
    {
        actions.Mouse.Enable();
    }

    public void MouseOff()
    {
        actions.Mouse.Disable();
    }


    /// <summary>
    /// Player‚ÌƒIƒ“ƒIƒt
    /// </summary>
    public void PlayerOn()
    {
        actions.Player.Enable();
    }

    public void PlayerOff()
    {
        actions.Player.Disable();
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
