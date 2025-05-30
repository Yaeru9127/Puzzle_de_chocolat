using UnityEngine;

public class SweetsMove : MonoBehaviour
{
    private InputSystem_Actions actions;
    private InputSystem_Manager inputmanager;
    private GameObject nowMass;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputmanager = this.gameObject.transform.GetComponent<InputSystem_Manager>();

        actions = inputmanager.GetActions();
        inputmanager.PlayerOn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
