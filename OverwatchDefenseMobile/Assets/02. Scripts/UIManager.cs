using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI playTimer;
    [SerializeField] private Slider baseHpBar;
    [SerializeField] private Cassidy cassidy;
    [SerializeField] private TextMeshProUGUI peacekeeperCurrentBullet;
    [SerializeField] private Slider ultimateGauge;
    [SerializeField] private TextMeshProUGUI ultimatePercent;
    [SerializeField] private Button combatRollButton;
    [SerializeField] private Slider combatRollSlider;
    [SerializeField] private TextMeshProUGUI combatRollText;
    [SerializeField] private Button flashbangButton;
    [SerializeField] private Slider flashbangSlider;
    [SerializeField] private TextMeshProUGUI flashbangText;
    // cassidy ult deadeye
    [SerializeField] private CassidyUlt cassidyUlt;
    [SerializeField] private Image ultPannelImage;
    [SerializeField] private ParticleSystem dustParticle;

    private float combatRollTimer;
    private bool setCombatRollTimer;
    private float flashbangTimer;
    private bool setFlashbangTimer;

    private void Awake()
    {
        // baseHp
        baseHpBar.maxValue = BaseHp.maxHp;

        // ultimate
        ultimateGauge.maxValue = cassidy.deadeye.maxUltimatePoint;

        // combatRoll
        combatRollSlider.maxValue = cassidy.combatRoll.skillCoolTime;

        // flashbang
        flashbangSlider.maxValue = cassidy.flashbang.skillCoolTime;
    }

    private void Update()
    {
        // playTimer 
        int minute = Mathf.FloorToInt(gameManager.PlayTime / 60);
        int second = Mathf.FloorToInt(gameManager.PlayTime % 60);

        playTimer.text = $"{minute} : {second: 00}";

        // baseHp
        baseHpBar.value = BaseHp.currentHp;

        // peacekeeper
        peacekeeperCurrentBullet.text = cassidy.peacekeeperCurrentBulletCount.ToString();

        // ultimate
        ultimateGauge.value = CassidyUlt.currentUltPoint;
        ultimatePercent.text = Mathf.FloorToInt((int)CassidyUlt.currentUltPoint * 100 / (int)cassidy.deadeye.maxUltimatePoint).ToString();

        // combatRoll
        if (cassidy.combatRoll.isSkillPossible == false)
        {
            if (setCombatRollTimer)
            {
                combatRollTimer -= Time.deltaTime;
                combatRollSlider.value += Time.deltaTime;
                combatRollText.text = ((int)combatRollTimer).ToString();
            }
            else
            {
                combatRollText.gameObject.SetActive(true);
                combatRollTimer = cassidy.combatRoll.skillCoolTime;
                setCombatRollTimer = true;
            }
        }
        else
        {
            combatRollText.gameObject.SetActive(false);
            setCombatRollTimer = false;
            combatRollSlider.value = 0;
        }

        // flashbang
        if (cassidy.flashbang.isSkillPossible == false)
        {
            if (setFlashbangTimer)
            {
                flashbangTimer -= Time.deltaTime;
                flashbangSlider.value += Time.deltaTime;
                flashbangText.text = ((int)flashbangTimer).ToString();
            }
            else
            {
                flashbangText.gameObject.SetActive(true);
                flashbangTimer = cassidy.flashbang.skillCoolTime;
                setFlashbangTimer = true;
            }
        }
        else
        {
            flashbangText.gameObject.SetActive(false);
            flashbangSlider.value = 0;
            setFlashbangTimer = false;
        }

        // deadeye
        if (cassidyUlt.IsUltActive)
        {
            ultPannelImage.gameObject.SetActive(true);
            
            ultPannelImage.CrossFadeAlpha(0.7f, 1f, false);
            dustParticle.Play();
        }
        else
        {
            ultPannelImage.gameObject.SetActive(false);
            ultPannelImage.canvasRenderer.SetAlpha(0f);
            dustParticle.Stop();
            dustParticle.Clear();
        }
    }
}