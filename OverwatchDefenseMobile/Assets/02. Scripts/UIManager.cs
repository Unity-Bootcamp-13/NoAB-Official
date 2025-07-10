using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI playTimer;
    [SerializeField] private Slider baseHpBar;
    [SerializeField] private Cassidy cassidy;
    [SerializeField] private TextMeshProUGUI zomnicKillCount;

    // possibleTurn effect
    [SerializeField] private GameObject combatrollPossibleTurnEffect;
    [SerializeField] private GameObject flashbangPossibleTurnEffect;
    [SerializeField] private GameObject ultPossibleTurnEffect;
    [SerializeField] private AudioSource skillPossibleTurnSound;
    [SerializeField] private AudioSource ultPossibleTurnSound;    
       
    // peacekeeper
    [SerializeField] private TextMeshProUGUI peacekeeperCurrentBullet;
    
    // combatroll
    [SerializeField] private Button combatRollButton;
    [SerializeField] private Slider combatRollSlider;
    [SerializeField] private TextMeshProUGUI combatRollText;
    // flashbang
    [SerializeField] private Button flashbangButton;
    [SerializeField] private Slider flashbangSlider;
    [SerializeField] private TextMeshProUGUI flashbangText;
    // cassidy ult deadeye
    [SerializeField] private Slider ultimateGauge;
    [SerializeField] private TextMeshProUGUI ultimatePercent;
    [SerializeField] private CassidyUlt cassidyUlt;
    [SerializeField] private Image ultPannelImage;
    [SerializeField] private ParticleSystem dustParticle;
    
    [SerializeField] private GameObject ultimatePossible;
    [SerializeField] private GameObject ultEffect;

    private bool hasTriggeredCombatEffect = false;
    private bool hasTriggeredFlashbangEffect = false;

    private float combatRollTimer;
    private bool setCombatRollTimer;
    private float flashbangTimer;
    private bool setFlashbangTimer;
    private bool rotateUltEffect;

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

        // zomnicKillCount
        zomnicKillCount.text = $"Á»´Ð {GameManager.ZomnicKillCount}Å³";

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
                combatRollText.text = Mathf.CeilToInt(combatRollTimer).ToString();

                if (combatRollTimer <= 0f && !hasTriggeredCombatEffect)
                {
                    StartCoroutine(C_SkillPossibleTurnEffect(combatrollPossibleTurnEffect, skillPossibleTurnSound));
                    hasTriggeredCombatEffect = true;
                }
            }
            else
            {                
                combatRollText.gameObject.SetActive(true);
                combatRollTimer = cassidy.combatRoll.skillCoolTime;
                setCombatRollTimer = true;
                hasTriggeredCombatEffect = false;
            }
        }
        else
        {
            combatRollText.gameObject.SetActive(false);          
            combatRollSlider.value = 0;
            setCombatRollTimer = false;
            hasTriggeredCombatEffect = false;
        }

        // flashbang
        if (cassidy.flashbang.isSkillPossible == false)
        {
            if (setFlashbangTimer)
            {
                flashbangTimer -= Time.deltaTime;
                flashbangSlider.value += Time.deltaTime;
                flashbangText.text = Mathf.CeilToInt(flashbangTimer).ToString();
                
                if (flashbangTimer <= 0.2f && !hasTriggeredFlashbangEffect)
                {
                    StartCoroutine(C_SkillPossibleTurnEffect(flashbangPossibleTurnEffect, skillPossibleTurnSound));
                    hasTriggeredFlashbangEffect = true;
                }
            }
            else
            {
                flashbangText.gameObject.SetActive(true);
                flashbangTimer = cassidy.flashbang.skillCoolTime;
                setFlashbangTimer = true;
                hasTriggeredFlashbangEffect = false;
            }
        }
        else
        {
            flashbangText.gameObject.SetActive(false);
            flashbangSlider.value = 0;
            setFlashbangTimer = false;
            hasTriggeredFlashbangEffect = false;
        }

        // deadeye
        bool ultPossible = cassidyUlt.IsUltimatePossible;
        ultimateGauge.gameObject.SetActive(!ultPossible);
        ultimatePossible.SetActive(ultPossible);                       
         
        if (ultPossible && rotateUltEffect == false)
            StartCoroutine(C_UltPossibleTurnEffect());

        if (ultPossible && !rotateUltEffect)
        {
            rotateUltEffect = true;
            StartCoroutine(C_RotateUltEffect());
        }

        if (!ultPossible)
        {
            StopCoroutine(C_RotateUltEffect());
        }

        if (cassidyUlt.IsUltActive)
        {
            ultPannelImage.gameObject.SetActive(true);

            ultPannelImage.CrossFadeAlpha(0.7f, 1f, false);
            dustParticle.Play();
            rotateUltEffect = false;
        }
        else
        {
            ultPannelImage.gameObject.SetActive(false);
            ultPannelImage.canvasRenderer.SetAlpha(0f);
            dustParticle.Stop();
            dustParticle.Clear();
        }
    }

    private IEnumerator C_SkillPossibleTurnEffect(GameObject skillEffect, AudioSource turnSound)
    {
        skillEffect.SetActive(true);
        turnSound.Play();

        yield return new WaitForSeconds(turnSound.clip.length);

        skillEffect.SetActive(false);
    }

    private IEnumerator C_UltPossibleTurnEffect()
    {        
        ultPossibleTurnEffect.SetActive(true);
        ultPossibleTurnSound.volume *= 5;
        ultPossibleTurnSound.Play();

        yield return new WaitForSeconds(ultPossibleTurnSound.clip.length);

        ultPossibleTurnEffect.SetActive(false);
    }

    private IEnumerator C_RotateUltEffect()
    {
        while (cassidyUlt.IsUltimatePossible)
        {
            ultEffect.SetActive(true);
            int randomRot = Random.Range(0, 360);
            ultEffect.transform.localRotation = Quaternion.Euler(0, 0, randomRot);
            yield return new WaitForSeconds(0.5f);

            ultEffect.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}