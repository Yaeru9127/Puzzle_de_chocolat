using UnityEngine;

public class InputSystem_Manager : MonoBehaviour
{
    InputSystem_Actions actions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    /// <summary>
    /// InputSystem_Actions���擾����֐�
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
    /// InputSystem�̃I���I�t
    /// </summary>
    public void UIOn()
    {
        actions.UI.Enable();
    }

    public void UIOff()
    {
        actions.UI.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
