using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChatController : MonoBehaviour
{
    [SerializeField]
    private InputActionReference toggleConsoleAction;
    [SerializeField]
    private InputActionReference backAction;
    [SerializeField]
    private GameObject canvasChat;

    public void OnEnable()
    {
        toggleConsoleAction.action.Enable();
        backAction.action.Enable();
    }

    public void OnDisable()
    {
        toggleConsoleAction.action.Disable();
        backAction.action.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (toggleConsoleAction.action.IsPressed())
        {
            canvasChat.SetActive(true);
        }

        if (backAction.action.IsPressed())
        {
            canvasChat.SetActive(false);
        }
    }
}
