using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    public GaugeController gaugeController; // Inspectorで設定

    void Update()
    {
        // Rキーまたはコントローラーの□（JoystickButton2）で選択中オブジェクトを破壊
        if ((Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton2))
            && ClickToDestroy.SelectedObject != null)
        {
            Destroy(ClickToDestroy.SelectedObject);
            gaugeController.OnObjectDestroyed();
            ClickToDestroy.ClearSelection();
        }
    }
}
