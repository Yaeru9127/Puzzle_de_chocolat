using UnityEngine;

public class InputSystem_Manager : MonoBehaviour
{
    InputSystem_Actions actions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    /// <summary>
    /// InputSystem_ActionsÇéÊìæÇ∑ÇÈä÷êî
    /// </summary>
    /// <returns></returns>
    public InputSystem_Actions GetActions()
    {
        if (actions == null)
        {
            actions = new InputSystem_Actions();
        }

        return actions;
    }

    /// <summary>
    /// UI
    /// </summary>
    public void UIClickOn()
    {
        actions.UI.Click.Enable();
    }

    public void UIClickOff()
    {
        actions.UI.Click.Disable();
    }

    public void UIPointOn()
    {
        actions.UI.Point.Enable();
    }

    public void UIPointOff()
    {
        actions.UI.Point.Disable();
    }

    /// <summary>
    /// PlayerÇÃÉIÉìÉIÉt
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
