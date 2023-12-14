using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public string[] GameName = {"Roll Ball", "Tank", "Minesweeper" };

    public void LoadRoomScene()
    {
        SceneManager.LoadScene("FindRoom");
    }
    public void LoadRollingBall()
    {
        SceneManager.LoadScene("Rollball");
    }
}
