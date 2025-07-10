using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Linq;
using System.Collections.Generic;

public class PlayFabStatSaver : MonoBehaviour
{
    /// <summary>
    /// ��� ���� �� ȣ��: ���ο� killCount�� ���� �ְ������� ������ ������ ����
    /// </summary>
    public void SaveHighScoreIfSuccessful(int killCount, bool isDefenseSuccess)
    {
        if (!isDefenseSuccess)
        {
            Debug.Log("��� ����, ��ŷ ��� ����");
            return;
        }

        // 1) ���� ������ ����� ���� ��谪 ��������
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest
        {
            StatisticNames = new List<string> { "ZomnicsKilled" }
        },
        result =>
        {
            // 2) ���� ��� ã��
            var stat = result.Statistics
                            .FirstOrDefault(s => s.StatisticName == "ZomnicsKilled");
            int currentHigh = stat != null ? stat.Value : 0;

            Debug.Log($"���� �ְ���: {currentHigh}, �� ����: {killCount}");

            // 3) �� ������ �� ������ ������Ʈ
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
                    updResult => Debug.Log($"�ְ��� ����: {killCount}"),
                    updError => Debug.LogError("���� ���� ����: " + updError.GenerateErrorReport())
                );
            }
            else
            {
                Debug.Log("�� ������ �ְ������� ���ų� ���Ƽ� �������� ����");
            }
        },
        error => Debug.LogError("�ְ��� ��ȸ ����: " + error.GenerateErrorReport()));
    }
}
