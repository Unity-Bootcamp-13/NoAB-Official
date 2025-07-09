using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class Cassidy : Character
{    
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private ProjectileManager projectileManager;
    [SerializeField] private CassidyUlt cassidyUlt;
    [SerializeField] AudioSource rollingSound;
    [SerializeField] AudioSource reloadSound;

    internal int peacekeeperCurrentBulletCount;
    internal static bool isRolling = false;

    [Header("Peacekeeper")]
    public ProjectileSettings peacekeeperBullet = new ProjectileSettings
    {
        id = 10001,
        speed = 500,
        lifetime = 2f,
        damage = 100,
        useGravity = false,
        collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative
    };

    public SkillSettings peacekeeper = new SkillSettings
    {
        skillCoolTime = 0.5f,
        bulletReloadTime = 1.5f,
        bulletInitCount = 6,
        isSkillPossible = true
    };

    [Header("Flashbang")]
    public ProjectileSettings flashbangBullet = new ProjectileSettings
    {
        id = 10002,
        speed = 30f,
        lifetime = 1f,
        damage = 50,
        useGravity = true,
        collisionDetectionMode = CollisionDetectionMode.Discrete
    };

    public SkillSettings flashbang = new SkillSettings
    {
        skillCoolTime = 10,
        skillDuration = 1,
        isSkillPossible = true
    };

    [Header("Combat Roll")]
    public SkillSettings combatRoll = new SkillSettings
    {
        skillCoolTime = 6f,
        isSkillPossible = true
    };

    [Header("Deadeye")]
    public UltimateSettings deadeye = new UltimateSettings
    { 
        maxUltimatePoint = 1800,
        pointPerDamage =  1,
        pointPerSecond = 5,
        damagePerSecond = 150,
        isUltimatePossible = true,
    };


    private void Awake()
    {
        peacekeeperCurrentBulletCount = peacekeeper.bulletInitCount;
        cassidyUlt.InjectUltimateSettings(deadeye);
    }


    private void Update()
    {       
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            NormalAttack();
        }

        // 스킬 1 - 섬광탄
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (isRolling)
                return;
            Skill_Flashbang(); 
        }

        // 스킬 2 - 구르기
        if (Keyboard.current.shiftKey.wasPressedThisFrame)
        {
           Skill_CombatRoll();
        }

        // 궁극기
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            cassidyUlt.OnFirstUltimateButtonInput();
        }

        if (Keyboard.current.qKey.wasReleasedThisFrame)
        {
            peacekeeperCurrentBulletCount = peacekeeper.bulletInitCount;
            cassidyUlt.OnSecondUltimateButtonInput();
        }

        // if (peacekeeper.isSkillPossible)
        //     NormalAttack();
    }


    public void OnPointerDown()
    {
        cassidyUlt.OnFirstUltimateButtonInput();
    }


    public void OnPointerUp()
    {
        peacekeeperCurrentBulletCount = peacekeeper.bulletInitCount;
        cassidyUlt.OnSecondUltimateButtonInput();        
    }


    public override void NormalAttack()
    {
        if (!peacekeeper.isSkillPossible)
            return;

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            if (hit.collider.CompareTag("Zomnic"))
            {
                StartCoroutine(C_NormalAtk(hit));
            }
        }
    }


    public IEnumerator C_NormalAtk(RaycastHit hit)
    {
        if (!peacekeeper.isSkillPossible)
            yield break;

        projectileManager.FireProjectile(Camera.main.transform.position + Camera.main.transform.forward, Camera.main.transform.forward, peacekeeperBullet);
        peacekeeper.isSkillPossible = false;
        peacekeeperCurrentBulletCount--;

        Debug.Log("총알 발사");
        Zomnic zomnic = hit.collider.GetComponent<Zomnic>();
        zomnic.TakeDamage(peacekeeperBullet.damage);

        if (peacekeeperCurrentBulletCount <= 0)
        {
            Reload();
            yield break;
        }
        else
            yield return new WaitForSeconds(peacekeeper.skillCoolTime);

        peacekeeper.isSkillPossible = true;
    }


    public void Reload()
    {
        StartCoroutine(C_PeacekeeperReload());
    }


    public IEnumerator C_PeacekeeperReload()
    {
        reloadSound.Play();
        peacekeeper.isSkillPossible = false;
        Debug.Log("재장전");
        yield return new WaitForSeconds(peacekeeper.bulletReloadTime);
        peacekeeperCurrentBulletCount = peacekeeper.bulletInitCount;
        Debug.Log("재장전 완료");
        peacekeeper.isSkillPossible = true;
    }     
  

    public void Skill_Flashbang()
    {
        if (isRolling)
            return;

        if (cassidyUlt.IsUltActive)
            return;

        StartCoroutine(C_flashbang());
    }


    private IEnumerator C_flashbang()
    {
        if (!flashbang.isSkillPossible)
        {
            Debug.Log("섬광탄 사용불가");
            yield break;
        }
        projectileManager.FireProjectile(Camera.main.transform.position, (Camera.main.transform.forward + Camera.main.transform.up * 0.5f).normalized, flashbangBullet);
        flashbang.isSkillPossible = false;

        yield return new WaitForSeconds(flashbang.skillCoolTime);
        flashbang.isSkillPossible = true;
    }


    public void Skill_CombatRoll()
    {
        if (cassidyUlt.IsUltActive)
            return;

        StartCoroutine(C_Rolling(playerMovement.MoveDir));
    }


    private IEnumerator C_Rolling(Vector3 inputVector)
    {
        if (!combatRoll.isSkillPossible)
        {
            Debug.Log("구르기 사용불가");
            yield break;
        }

        combatRoll.isSkillPossible = false;

        Transform cam = Camera.main.transform;
        cam.eulerAngles = new Vector3(0f, cam.eulerAngles.y, cam.eulerAngles.z);

        Vector3 rolling_dir;

        rollingSound.Play();

        isRolling = true;

        if (inputVector.sqrMagnitude > 0f)
            rolling_dir = inputVector.normalized;
        else
        {
            rolling_dir = cam.forward;
            rolling_dir.y = 0f;
            rolling_dir.Normalize();
        }

        float rollDistance = 6f;
        float rollDuration = 0.5f;
        Vector3 rollVelocity = rolling_dir * (rollDistance / rollDuration);
        float halfTime = rollDuration * 0.5f;

        Quaternion startRot = cam.localRotation;
        Quaternion maxRot = Quaternion.Euler(30f, 0f, 0f);

        float elapsed = 0f;
        while (elapsed < rollDuration)
        {
            characterController.Move(rollVelocity * Time.deltaTime);

            if (elapsed < halfTime)
                cam.localRotation = Quaternion.Lerp(startRot, maxRot, elapsed / halfTime);
            else
                cam.localRotation = Quaternion.Lerp(maxRot, startRot, (elapsed - halfTime) / halfTime);

            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.localRotation = startRot;
        isRolling = false;

        yield return new WaitForSeconds(combatRoll.skillCoolTime);
        combatRoll.isSkillPossible = true;        
    }
}