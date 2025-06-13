using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem_Manager : MonoBehaviour
{
    public static InputSystem_Manager manager { get; private set; }

    private InputSystem_Actions actions;

    private void Awake()
    {
        if (manager == null) manager = this;
        else Destroy(this.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
