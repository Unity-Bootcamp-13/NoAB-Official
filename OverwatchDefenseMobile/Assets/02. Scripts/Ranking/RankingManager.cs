using System.Collections.Generic;
using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;

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
                List<RankingManager.PlayerResult> results = new List<RankingManager.PlayerResult>();

                foreach (var entry in result.Leaderboard)
                {
                    results.Add(new RankingManager.PlayerResult
                    {
                        Name = entry.DisplayName ?? "�͸�",
                        KillCount = entry.StatValue
                    });
                }

                ShowRanking(results);
            },
            error => Debug.LogError("��ŷ �ҷ����� ����: " + error.GenerateErrorReport()));
    }

    [System.Serializable]
    public class PlayerResult
    {
        public string Name;
        public int KillCount;
    }
}