using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    private InputSystem_Manager manager;
    private InputSystem_Actions action;
    private CursorController cc;
    private StageManager stage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc = CursorController.cc;
        stage = StageManager.stage;
        manager = InputSystem_Manager.manager;
        action = manager.GetActions();
        stage.phase = StageManager.Phase.Result;
        cc.instance.SetActive(true);
        manager.PlayerOff();

        if (cc.instance != null)
        {
            manager.MouseOff();
            manager.GamePadOn();
        }
        else
        {
            manager.GamePadOff();
            manager.MouseOn();
        }
    }

    public void LoadResultScene()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
