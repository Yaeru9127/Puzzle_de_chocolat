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
        //初期化
        DontDestroyOnLoad(this.gameObject);
        GetActions();
        PlayerOff();
    }

    /// <summary>
    /// InputSystem_Actionsを取得する関数
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
    /// Playerのオンオフ
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
        SceneManager.LoadScene("Stage01");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
