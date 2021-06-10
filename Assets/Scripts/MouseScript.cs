using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScript : MonoBehaviour
{
    public GameObject menu;
    public GameControls controls;
    PlayerMovement player;
    bool _menuFlag = false;


    private void Awake()
    {
        controls = new GameControls();
        controls.Menu.Enable();
        controls.Menu.Esc.performed += _ => OpenMenu();
    }

    void Start()
    {
        player = GetComponent<PlayerMovement>();
        Cursor.lockState = CursorLockMode.Locked;

    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void OpenMenu()
    {
        if (!_menuFlag) { 
            UnlockCursor();
            Time.timeScale = 0;
            player._stopFlag = true;
            menu.SetActive(true);
            _menuFlag = true;
        }
        else
        {
            LockCursor();
            Time.timeScale = 1;
            player._stopFlag = false;
            menu.SetActive(false);
            _menuFlag = false;
        }
    }
}
