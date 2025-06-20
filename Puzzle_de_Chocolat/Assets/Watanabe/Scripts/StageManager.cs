using UnityEngine;

public class StageManager : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayBGM("stage");
    }
}
