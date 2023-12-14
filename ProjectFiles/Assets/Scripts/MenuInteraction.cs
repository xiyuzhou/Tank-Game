using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInteraction : MonoBehaviour
{
    private MenuInput menuInput;
    public MenuInput.PlayerMenuInputActions onMenu;

    public bool CurPageIndex;

    public TMP_InputField[] SignInPage;
    public TMP_InputField[] SignupPage;

    // Update is called once per frame
    void Awake()
    {
        CurPageIndex = true;
        menuInput = new MenuInput();
        onMenu = menuInput.PlayerMenuInput;
        onMenu.TabToNext.performed += ctx => TabSelectNext();
        onMenu.EnterConfirm.performed += ctx => EnterConfirm();
    }
    private void Start()
    {
        onMenu.Enable();
    }
    private void TabSelectNext()
    {
        var InputFieldList = CurPageIndex ? SignInPage : SignupPage;
        for (int i = 0; i < InputFieldList.Length; i++)
        {
            if (InputFieldList[i].isFocused)
            {
                int nextIndex = (i + 1) % InputFieldList.Length;
                InputFieldList[nextIndex].ActivateInputField();
                return;
            }
        }
        InputFieldList[0].ActivateInputField();
    }

    private void EnterConfirm()
    {
        if (CurPageIndex)
        {
            LoginRegister.instance.OnLoginButton();
        }
        else
        {
            LoginRegister.instance.OnRegister();
        }
    }
}
