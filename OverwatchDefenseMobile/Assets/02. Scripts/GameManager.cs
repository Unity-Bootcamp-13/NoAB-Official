using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject VictoryPanel;
    [SerializeField] GameObject VictoryEffect;
    [SerializeField] TextMeshProUGUI VictoryText;
    [SerializeField] GameObject DefeatPanel;
    [SerializeField] GameObject DefeatEffect;
    [SerializeField] TextMeshProUGUI DefeatText;

    internal float PlayTime = 10;
    private bool _isGameEnded = false;

    private void Update()
    {
        if (_isGameEnded) return;

        PlayTime -= Time.deltaTime;
        if (PlayTime < 0) PlayTime = 0;

        if (BaseHp.GameOver)
        {
            _isGameEnded = true;
            StartCoroutine(Defeat());
        }
        else if (PlayTime <= 0)
        {
            _isGameEnded = true;
            StartCoroutine(Victory());
        }
    }

    private IEnumerator Victory()
    {
        VictoryPanel.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        VictoryText.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.8f);
        VictoryEffect.SetActive(false);

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("StartScene");
    }

    private IEnumerator Defeat()
    {
        DefeatPanel.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        DefeatText.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.8f);
        DefeatEffect.SetActive(false);

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("StartScene");
    }
}
