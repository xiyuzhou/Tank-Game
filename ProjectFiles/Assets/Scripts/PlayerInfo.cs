using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using TMPro;
using PlayFab.ClientModels;

public class PlayerInfo : MonoBehaviour
{
    [HideInInspector]
    public PlayerProfileModel profile;
    public TextMeshProUGUI LoginMessage;
    void Awake() { 
    }
    private void Start()
    {
        OnLoggedIn();
    }
    public void OnLoggedIn()
    {
        GetPlayerProfileRequest getProfileRequest = new GetPlayerProfileRequest
        {
            PlayFabId = LoginRegister.instance.playFabId,
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowDisplayName = true
            }
        };
        PlayFabClientAPI.GetPlayerProfile(getProfileRequest,
         result =>
         {
             profile = result.PlayerProfile;
             Debug.Log("Loaded in player: " + profile.DisplayName);
             LoginMessage.text = "Logged in as: "+ '\n' + result.PlayerProfile.DisplayName + '\n' + "PlayerID:" + result.PlayerProfile.PlayerId;
         },
         error => Debug.Log(error.ErrorMessage)
        );

    }
}
