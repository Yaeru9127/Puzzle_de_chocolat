using UnityEngine;
using UnityEngine.InputSystem;

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
    /// UI
    /// </summary>
    public void UIOn()
    {
        actions.UI.Enable();
    }

    public void UIOff()
    {
        actions.UI.Disable();
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

    private void OnDestroy()
    {
        actions.Disable();

        //シーンを跨ぐときにメモリから消す
        if (manager == this) manager = null;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
