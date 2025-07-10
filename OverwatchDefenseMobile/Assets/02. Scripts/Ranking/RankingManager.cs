using System.Collections.Generic;
using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RankingManager : MonoBehaviour
{
    [SerializeField] private RankingUI rankingUI;

    private void Start()
    {
        FetchTop20Ranking();
    }

    public void ShowRanking(List<PlayerResult> results)
    {
        rankingUI.Show(results);
    }

    public void FetchTop20Ranking()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "ZomnicsKilled",
            StartPosition = 0,
            MaxResultsCount = 20
        };

        PlayFabClientAPI.GetLeaderboard(request,
            result =>
            {
                List<PlayerResult> results = new List<PlayerResult>();

                foreach (var entry in result.Leaderboard)
                {
                    results.Add(new PlayerResult
                    {
                        Name = entry.DisplayName ?? "익명",
                        KillCount = entry.StatValue
                    });
                }

                ShowRanking(results);
            },
            error => Debug.LogError("랭킹 불러오기 실패: " + error.GenerateErrorReport()));
    }

    [System.Serializable]
    public class PlayerResult
    {
        public string Name;
        public int KillCount;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("StartScene");
    }
}