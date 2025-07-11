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
    /// InputSystem_Actions���擾����֐�
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
    /// Player�̃I���I�t
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

        //�V�[�����ׂ��Ƃ��Ƀ������������
        if (manager == this) manager = null;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
