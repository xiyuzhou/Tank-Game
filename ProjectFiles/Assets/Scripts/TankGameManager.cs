using Photon.Pun;
using System.Linq;
using UnityEngine;
using TMPro;

public class TankGameManager : MonoBehaviourPun
{
    [Header("Players")]
    public string playerPrefabPath;
    public Transform[] spawnPoints;
    public float respawnTime;
    public int alivePlayers;
    private int playersInGame;
    public TankPlayerInfo[] players;
    // instance
    public static TankGameManager instance;
    public bool ready = false;
    public TextMeshProUGUI winText;
    public GameObject ImageObject;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        players = new TankPlayerInfo[PhotonNetwork.PlayerList.Length];
        alivePlayers = players.Length;
        photonView.RPC("ImInGame", RpcTarget.AllBuffered);
        ready = true;
        ImageObject.SetActive(false);
    }
    [PunRPC]
    void ImInGame()
    {
        playersInGame++;
        if (PhotonNetwork.IsMasterClient && playersInGame == PhotonNetwork.PlayerList.Length)
            photonView.RPC("SpawnPlayer", RpcTarget.All);
    }
    [PunRPC]
    void SpawnPlayer()
    {
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabPath, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
        // initialize the player
        playerObj.GetComponent<TankPlayerInfo>().photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);

    }
    public TankPlayerInfo GetPlayer(int playerId)
    {
        foreach (TankPlayerInfo player in players)
        {
            if (player != null && player.id == playerId)
                return player;
        }
        return null;

    }
    public TankPlayerInfo GetPlayer(GameObject playerObj)
    {
        foreach (TankPlayerInfo player in players)
        {
            if (player != null && player.gameObject == playerObj)
                return player;
        }
        return null;

    }
    public void CheckWinCondition()
    {
        if (alivePlayers == 1)
            photonView.RPC("WinGame", RpcTarget.All, players.First(x => !x.isDead).id);
    }
    [PunRPC]
    void WinGame(int winningPlayer)
    {
        // set the UI win text
        ImageObject.SetActive(true);
        winText.text = GetPlayer(winningPlayer).photonPlayer.NickName + " Wins! ";
        //GameUIManager.instance.SetWinText(GetPlayer(winningPlayer).photonPlayer.NickName);
        Invoke("GoBackToMenu", 2);
    }
    void GoBackToMenu()
    {
        NetworkManager.instance.ChangeScene("Menu");
    }
}