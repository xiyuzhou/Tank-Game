using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LoginRegister : MonoBehaviour
{

    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;

    public TMP_InputField SignupName;
    public TMP_InputField SignupPassword1;
    public TMP_InputField SignupPassword2;

    public TextMeshProUGUI displayText;
    public TextMeshProUGUI displayText2;
    public UnityEvent onLoggedIn;
    [HideInInspector]
    public string playFabId;
    public static LoginRegister instance;

    public GameObject LoginPage;
    public GameObject RegisterPage;

    public MenuInteraction menuInteraction;
    void Awake() { 
        instance = this;
    }
    // Start is called before the first frame update
    public void OnRegister()
    {
        if (SignupPassword1.text == "") 
        {
            SetDisplayText(displayText2, "Empty password", Color.red);
            return;
        }
        if (SignupPassword1.text != SignupPassword2.text || SignupPassword1.text == "")
        {
            SetDisplayText(displayText2, "Password do not match", Color.red);
            return;
        }
        RegisterPlayFabUserRequest registerRequest = new RegisterPlayFabUserRequest
        {
            Username = SignupName.text,
            DisplayName = SignupName.text,
            Password = SignupPassword1.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest,
             result =>
             {
                 Debug.Log(result.PlayFabId);

                 SetDisplayText(displayText2, "Success! ID: " + result.PlayFabId + '\n'+ "Leading to sign in page..." , Color.green);

                 Invoke("SigninWithInfo", 1.5f);
             },
             error =>
             {
                 Debug.Log(error.ErrorMessage);
                 SetDisplayText(displayText2,error.ErrorMessage, Color.red);
             });
    }
    public void OnLoginButton()
    {
        LoginWithPlayFabRequest loginRequest = new LoginWithPlayFabRequest
        {
            Username = usernameInput.text,
            Password = passwordInput.text
        };
        PlayFabClientAPI.LoginWithPlayFab(loginRequest,
            result =>
            {
                SetDisplayText(displayText,"Logged in as: " + result.PlayFabId, Color.green);

                if (onLoggedIn != null)
                    onLoggedIn.Invoke();
                playFabId = result.PlayFabId;
                SceneManager.LoadScene("Menu");
            },
            error => SetDisplayText(displayText,error.ErrorMessage, Color.red)
        );

    }
    void SetDisplayText(TextMeshProUGUI TmpText,string text, Color color)
    {
        TmpText.text = text;
        TmpText.color = color;
    }
    public void OnCreateAccountBT()
    {
        LoginPage.SetActive(false);
        RegisterPage.SetActive(true);
        menuInteraction.CurPageIndex = false;
    }

    public void OnSignInBT()
    {
        LoginPage.SetActive(true);
        RegisterPage.SetActive(false);
        menuInteraction.CurPageIndex = true;
    }
    public void SigninWithInfo()
    {
        LoginPage.SetActive(true);
        RegisterPage.SetActive(false);
        menuInteraction.CurPageIndex = true;
        usernameInput.text = SignupName.text;
        passwordInput.text = SignupPassword1.text;
    }
}
