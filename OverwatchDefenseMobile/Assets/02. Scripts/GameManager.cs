using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
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

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource VictorySound;
    [SerializeField] private AudioSource VictoryEffectSound;
    [SerializeField] private AudioSource DefeatSound;
    [SerializeField] private AudioSource DefeatEffectSound;
    [SerializeField] private AudioSource Remain10sSound;
    [SerializeField] private AudioSource Remain30sSound;


    internal float PlayTime = 150;
    private bool _isGameEnded = false;
    private bool _remain30 = false;
    private bool _remain10 = false;
    private AudioMixerSnapshot muteOn, muteOff;
    public static int ZomnicKillCount;
    public static bool LogError = false;
    

    private void Awake()
    {
        muteOn = audioMixer.FindSnapshot("MuteOn");
        muteOff = audioMixer.FindSnapshot("MuteOff");
    }

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
            LoadScene("StartScene");
        }
        else if (PlayTime <= 0 && !LogError)
        {
            _isGameEnded = true;
            FindFirstObjectByType<PlayFabStatSaver>().SaveHighScoreIfSuccessful(ZomnicKillCount, true);
            StartCoroutine(Victory());
            LoadScene("RankingScene");
        }
        else if (PlayTime <= 0 && LogError)
        {
            _isGameEnded = true;
            StartCoroutine(Victory());
            LoadScene("StartScene");
        }

        if (Cassidy.transform.position.y <= 28)
        {
            _isGameEnded = true;
            StartCoroutine(Defeat());
        }
    }

    private IEnumerator Victory()
    {
        ZomnicKillCount = 0;
        VictoryPanel.SetActive(true);
        muteOn.TransitionTo(0f);

        VictoryEffectSound.Play();
        yield return new WaitForSeconds(1.5f);
        VictoryText.gameObject.SetActive(true);
        VictorySound.Play();

        yield return new WaitForSeconds(VictorySound.clip.length);
        VictoryEffect.SetActive(false);

        yield return new WaitForSeconds(2);        
        muteOff.TransitionTo(0f);
    }

    private IEnumerator Defeat()
    {
        ZomnicKillCount = 0;
        DefeatPanel.SetActive(true);
        muteOn.TransitionTo(0f);

        DefeatEffectSound.Play();
        yield return new WaitForSeconds(1.5f);
        DefeatText.gameObject.SetActive(true);
        DefeatSound.Play();

        yield return new WaitForSeconds(DefeatSound.clip.length);
        DefeatEffect.SetActive(false);

        yield return new WaitForSeconds(2);
        muteOff.TransitionTo(0f);
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
