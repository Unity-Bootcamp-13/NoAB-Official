using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class RankingUI : MonoBehaviour
{
    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private Transform contentParent;

    public void Show(List<RankingManager.PlayerResult> sortedList)
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        for (int i = 0; i < sortedList.Count; i++)
        {
            GameObject entry = Instantiate(entryPrefab, contentParent);
            var texts = entry.GetComponentsInChildren<TextMeshProUGUI>();

            texts[0].text = $"{i + 1}";
            texts[1].text = sortedList[i].Name;
            texts[2].text = $"Á»´Ð {sortedList[i].KillCount}Å³";
        }
    }
}