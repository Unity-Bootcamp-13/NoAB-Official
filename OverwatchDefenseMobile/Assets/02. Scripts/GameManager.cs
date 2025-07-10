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
    [SerializeField] GameObject Cassidy;

    [SerializeField] AudioSource VictorySound;
    [SerializeField] AudioSource DefeatSound;
    [SerializeField] AudioSource Remain10sSound;
    [SerializeField] AudioSource Remain30sSound;

    internal float PlayTime = 150;
    private bool _isGameEnded = false;
    private bool _remain30 = false;
    private bool _remain10 = false;

    private void Update()
    {
        if (_isGameEnded) return;

        PlayTime -= Time.deltaTime;
        if (PlayTime < 0) PlayTime = 0;

        if (!_remain30 && PlayTime <= 31)
        {
            _remain30 = true;
            Remain30sSound.Play();
        }

        if (!_remain10 && PlayTime <= 11)
        {
            _remain10 = true;
            Remain10sSound.Play();
        }

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

        if (Cassidy.transform.position.y <= 28)
        {
            _isGameEnded = true;
            StartCoroutine(Defeat());
        }
    }

    private IEnumerator Victory()
    {
        VictoryPanel.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        VictoryText.gameObject.SetActive(true);
        VictorySound.Play();

        yield return new WaitForSeconds(VictorySound.clip.length);
        VictoryEffect.SetActive(false);

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("StartScene");
    }

    private IEnumerator Defeat()
    {
        DefeatPanel.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        DefeatText.gameObject.SetActive(true);
        DefeatSound.Play();

        yield return new WaitForSeconds(DefeatSound.clip.length);
        DefeatEffect.SetActive(false);

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("StartScene");
    }
}
