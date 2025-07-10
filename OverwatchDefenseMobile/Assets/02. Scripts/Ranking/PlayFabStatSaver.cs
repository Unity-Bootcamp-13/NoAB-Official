using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Linq;
using System.Collections.Generic;

public class PlayFabStatSaver : MonoBehaviour
{
    /// <summary>
    /// 방어 성공 시 호출: 새로운 killCount가 이전 최고점보다 높으면 서버에 저장
    /// </summary>
    public void SaveHighScoreIfSuccessful(int killCount, bool isDefenseSuccess)
    {
        if (!isDefenseSuccess)
        {
            Debug.Log("방어 실패, 랭킹 등록 생략");
            return;
        }

        // 1) 먼저 서버에 저장된 현재 통계값 가져오기
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest
        {
            StatisticNames = new List<string> { "ZomnicsKilled" }
        },
        result =>
        {
            // 2) 기존 기록 찾기
            var stat = result.Statistics
                            .FirstOrDefault(s => s.StatisticName == "ZomnicsKilled");
            int currentHigh = stat != null ? stat.Value : 0;

            Debug.Log($"기존 최고점: {currentHigh}, 새 점수: {killCount}");

            // 3) 새 점수가 더 높으면 업데이트
            if (killCount > currentHigh)
            {
                var updateRequest = new UpdatePlayerStatisticsRequest
                {
                    Statistics = new List<StatisticUpdate>
                    {
                        new StatisticUpdate
                        {
                            StatisticName = "ZomnicsKilled",
                            Value = killCount
                        }
                    }
                };
                PlayFabClientAPI.UpdatePlayerStatistics(updateRequest,
                    updResult => Debug.Log($"최고점 갱신: {killCount}"),
                    updError => Debug.LogError("점수 갱신 실패: " + updError.GenerateErrorReport())
                );
            }
            else
            {
                Debug.Log("새 점수가 최고점보다 낮거나 같아서 갱신하지 않음");
            }
        },
        error => Debug.LogError("최고점 조회 실패: " + error.GenerateErrorReport()));
    }
}
