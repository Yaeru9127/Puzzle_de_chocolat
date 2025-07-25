using UnityEngine;

public class MyGameHandler : MonoBehaviour
{
    public Shake shake;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (shake != null)
            {
                shake.TriggerShake();
                Debug.Log("UIÇ™óhÇÍÇ‹ÇµÇΩÅI");
            }
        }
    }
}