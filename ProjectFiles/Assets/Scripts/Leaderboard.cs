using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class LeaderBoard : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject leaderboardCanvas;
    public GameObject[] leaderboardEntries;
    public string leaderboardName;
    public int maxCount;
    private List<GameObject> playerList = new List<GameObject>();


    void Awake()
    {
        foreach (var entry in leaderboardEntries)
        {
            entry.SetActive(false);
        }
    }
    private void Start()
    {
        DisplayLeaderboard();
    }
    public void DisplayLeaderboard()
    {
        GetLeaderboardRequest getLeaderboardRequest = new GetLeaderboardRequest
        {
            StatisticName = leaderboardName,
            MaxResultsCount = maxCount
        };
        PlayFabClientAPI.GetLeaderboard(getLeaderboardRequest,
        result => UpdateLeaderboardUI(result.Leaderboard),
        error => Debug.Log(error.ErrorMessage)
        );
    }
    void UpdateLeaderboardUI(List<PlayerLeaderboardEntry> leaderboard)
    {
        for (int x = 0; x < leaderboardEntries.Length; x++)
        {
            leaderboardEntries[x].SetActive(x < leaderboard.Count);
            if (x >= leaderboard.Count) continue;
            leaderboardEntries[x].transform.Find("Name").GetComponent<TextMeshProUGUI>()
                .text = (leaderboard[x].Position + 1) + ". " + leaderboard[x].DisplayName;
            leaderboardEntries[x].transform.Find("Score").GetComponent<TextMeshProUGUI>()
                .text = (-(float)leaderboard[x].StatValue * 0.001f).ToString("F2");
        }

    }
    public void SetLeaderboardEntry(int newScore)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
            Statistics = new List<StatisticUpdate>
                    {
                        new StatisticUpdate { StatisticName = "FastestTime", Value = newScore },
                    }
        },
                result => { Debug.Log("User statistics updated"); },
                error => { Debug.LogError(error.GenerateErrorReport()); }
        );
    }

}
