using UnityEngine;

public class ReloadCountDisplay : MonoBehaviour
{
    void Start()
    {
        if (ReloadCountManager.Instance != null)
        {
            int count = ReloadCountManager.Instance.ReloadCount;
            Debug.Log("【リロード回数】現在のリロード回数は: " + count);
        }
        else
        {
            Debug.LogWarning("ReloadCountManager が見つかりませんでした！");
        }
    }
}
