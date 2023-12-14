using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TankPlayerInfo : MonoBehaviourPun
{
    [Header("Photon")]
    public int health = 5;
    public int id;
    public bool isDead;
    public int kills;
    public bool HPchange;
    public Player photonPlayer;
    private int curAttackerId;
    public bool photonViewIsMine = false;

    [PunRPC]
    public void Initialize(Player player)
    {
        id = player.ActorNumber;
        photonPlayer = player;
        TankGameManager.instance.players[id - 1] = this;

        // is this not our local player?
        if (!photonView.IsMine)
        {
            //
        }
        else
        {
        }

    }
    [PunRPC]
    public void OnHealChange(int changeValue)
    {
        if (isDead)
        {
            return;
        }
        health += changeValue;

        if (health <= 0)
        {
            Debug.Log("died");
            photonView.RPC("Die", RpcTarget.All);

        }
    }
    [PunRPC]
    void Die()
    {
        health = 0;
        isDead = true;
        TankGameManager.instance.alivePlayers--;
        // host will check win condition
        if (PhotonNetwork.IsMasterClient)
            TankGameManager.instance.CheckWinCondition();
        // is this our local player?
        if (photonView.IsMine)
        {
            if (curAttackerId != 0)
                TankGameManager.instance.GetPlayer(curAttackerId).photonView.RPC("AddKill", RpcTarget.All);
            // set the cam to spectator
            Destroy(this.gameObject);
            // disable the physics and hide the player
            //rig.isKinematic = true;
            transform.position = new Vector3(0, -50, 0);
        }
    }
}
