using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    public GaugeController gaugeController; // Inspector�Őݒ�

    void Update()
    {
        // R�L�[�܂��̓R���g���[���[�́��iJoystickButton2�j�őI�𒆃I�u�W�F�N�g��j��
        if ((Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton2))
            && ClickToDestroy.SelectedObject != null)
        {
            Destroy(ClickToDestroy.SelectedObject);
            gaugeController.OnObjectDestroyed();
            ClickToDestroy.ClearSelection();
        }
    }
}
