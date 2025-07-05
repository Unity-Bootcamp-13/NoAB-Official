using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class CassidyTest: Character
{    
    [SerializeField] private PlayerMovementTest playerMovement;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private ProjectileManager projectileManager;
    [SerializeField] private CassidyUlt cassidyUlt;

    private int peacekeeperCurrentBulletCount;
    private bool _buttonDown;


    [Header("Peacekeeper")]
    ProjectileSettings peacekeeperBullet = new ProjectileSettings
    {
        id = 10001,
        speed = 500,
        lifetime = 2f,
        damage = 100,
        useGravity = false,
        collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative
    };

    SkillSettings peacekeeper = new SkillSettings
    {
        skillCoolTime = 0.5f,
        bulletReloadTime = 1.5f,
        bulletInitCount = 6,
        isSkillPossible = true
    };

    [Header("Flashbang")]
    ProjectileSettings flashbangBullet = new ProjectileSettings
    {
        id = 10002,
        speed = 30f,
        lifetime = 7f,
        damage = 50,
        useGravity = true,
        collisionDetectionMode = CollisionDetectionMode.Discrete
    };

    SkillSettings flashbang = new SkillSettings
    {
        skillCoolTime = 10,
        skillDuration = 1,
        isSkillPossible = true
    };

    [Header("Combat Roll")]
    SkillSettings combatRoll = new SkillSettings
    {
        skillCoolTime = 6f,
        isSkillPossible = true
    };

    [Header("Deadeye")]
    UltimateSettings deadeye = new UltimateSettings
    { 
        maxUltimatePoint = 1800,
        damagePerSecond =  150,
        ultimatePointPerSecond = 5,
        isUltimatePossible = true,
    };


    private void Awake()
    {
        peacekeeperCurrentBulletCount = peacekeeper.bulletInitCount;
        cassidyUlt.InjectUltimateSettings(deadeye);
    }

    private void Update()
    {
        // NormalAttack();

        // 기본 공격
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            NormalAttack();
        }

        // 스킬 1 - 섬광탄
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Skill_Flashbang(); 
        }

        // 스킬 2 - 구르기
        if (Keyboard.current.shiftKey.wasPressedThisFrame)
        {
           Skill_CombatRoll();
        }

        // 궁극기
        if (_buttonDown)
        {
            cassidyUlt.TrackInput();
            cassidyUlt.IncreaseDamage();
        }

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            cassidyUlt.FirstInput();
        }

        if (Keyboard.current.qKey.isPressed)
        {
            cassidyUlt.TrackInput();
            cassidyUlt.IncreaseDamage();
        }

        if (Keyboard.current.qKey.wasReleasedThisFrame)
        {
            cassidyUlt.FireUltimate();
        }
    }

    public void OnPointerDown()
    {
        _buttonDown = true;

        cassidyUlt.FirstInput();
    }

    public void OnPointerUp()
    {
        _buttonDown = false;

        cassidyUlt.FireUltimate();
    }


    public override void NormalAttack()
    {
        StartCoroutine(C_NormalAtk());
    }

    public IEnumerator C_NormalAtk()
    {
        if (!peacekeeper.isSkillPossible)
            yield break;

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            if (hit.collider.CompareTag("Zomnic"))
            {
                projectileManager.FireProjectile(Camera.main.transform.position, Camera.main.transform.forward, peacekeeperBullet);
                peacekeeper.isSkillPossible = false;
                peacekeeperCurrentBulletCount--;

                Debug.Log("총알 발사");
                Zomnic zomnic = hit.collider.GetComponent<Zomnic>();
                zomnic.TakeDamage(peacekeeperBullet.damage);
            }
        }
        yield return new WaitForSeconds(peacekeeper.skillCoolTime);

        if (peacekeeperCurrentBulletCount <= 0)
        {
            Debug.Log("재장전");
            yield return new WaitForSeconds(peacekeeper.bulletReloadTime);
            peacekeeperCurrentBulletCount = peacekeeper.bulletInitCount;
            Debug.Log("재장전 완료");
        }

        peacekeeper.isSkillPossible = true;
    }

    public void Skill_Flashbang()
    {
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

        Vector3 dir;
        if (inputVector.sqrMagnitude > 0f)
            dir = inputVector.normalized;
        else
        {
            dir = cam.forward;
            dir.y = 0f;
            dir.Normalize();
        }

        float rollDistance = 6f;
        float rollDuration = 0.5f;
        Vector3 rollVelocity = dir * (rollDistance / rollDuration);
        float halfTime = rollDuration * 0.5f;

        Transform model = transform.GetChild(0);
        Quaternion startRot = model.localRotation;
        Quaternion tiltRot = Quaternion.Euler(30f, 0f, 0f);

        float elapsed = 0f;
        while (elapsed < rollDuration)
        {
            characterController.Move(rollVelocity * Time.deltaTime);

            if (elapsed < halfTime)
                model.localRotation = Quaternion.Lerp(startRot, tiltRot, elapsed / halfTime);
            else
                model.localRotation = Quaternion.Lerp(tiltRot, startRot, (elapsed - halfTime) / halfTime);

            elapsed += Time.deltaTime;
            yield return null;
        }

        model.localRotation = startRot;

        yield return new WaitForSeconds(combatRoll.skillCoolTime);
        combatRoll.isSkillPossible = true;
    }
}