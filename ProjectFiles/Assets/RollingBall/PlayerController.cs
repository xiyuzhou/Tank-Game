using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    public bool multiplayer;
    public GameObject playButton;
    public float highestTime = 0;
    public float speed;
    private Rigidbody rig;
    private float startTime;
    private float timeTaken;
    public float turnSpeed = 5f;

    private int collectablesPicked = 0;
    public int maxCollectables = 10;
    private bool isPlaying;
    public TextMeshProUGUI curTimeText;
    private bool isPlayed = false;
    public TextMeshProUGUI buttonText;
    public LeaderBoard leaderBoard;
    public TextMeshProUGUI curSessionBoard;

    private Vector3 playerInput;
    void Awake()
    {
        rig = GetComponent<Rigidbody>();
        rig.useGravity = false;
    }
    void Update()
    {
        if (!isPlaying)
        {
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
            transform.Rotate(Vector3.forward, turnSpeed / 2 * Time.deltaTime);
            return;
        }
          
        float x = Input.GetAxis("Horizontal") * speed;
        float z = Input.GetAxis("Vertical") * speed;
        playerInput = new Vector3(x, rig.velocity.y, z);

        curTimeText.text = (Time.time - startTime).ToString("F2");

    }

    private void FixedUpdate()
    {
        rig.AddForce(playerInput * speed);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            collectablesPicked++;
            Destroy(other.gameObject);
            if (collectablesPicked == maxCollectables)
                End();
        }
    }
    public void Begin()
    {
        rig.useGravity = true;
        if (isPlayed)
            SceneManager.LoadScene("Rollball");
        startTime = Time.time;
        isPlaying = true;
        playButton.SetActive(false);
    }
    void End()
    {
        timeTaken = Time.time - startTime;
        isPlaying = false;
        leaderBoard.SetLeaderboardEntry(-Mathf.RoundToInt(timeTaken * 1000.0f));
        playButton.SetActive(true);
        buttonText.text = "Replay";
        isPlayed = true;
        if (multiplayer)
        {
            UpdateHighestCurrentSession(timeTaken);
        }
        Invoke("UpdateLeaderBoard", 1.5f);
    }
    public void UpdateHighestCurrentSession(float time) {
        if (highestTime > time || highestTime == 0)
        {
            highestTime = time;
        }
        photonView.RPC("UpdateHighestCurrentSessionUI", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName);
    }
    [PunRPC]
    public void UpdateHighestCurrentSessionUI(string name)
    {
        curSessionBoard.text = name + highestTime.ToString();
    }
    private void UpdateLeaderBoard()
    {
        leaderBoard.DisplayLeaderboard();
    }


}
