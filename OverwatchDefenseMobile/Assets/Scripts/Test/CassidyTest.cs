using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class CassidyTest: Character
{
    [SerializeField] private ProjectileManager projectileManager;
    [SerializeField] private PlayerMovementTest playerMovement;
    [SerializeField] private Ult ult;
    [SerializeField] private CharacterController characterController;

    private bool _isRolling;

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
        skillCoolTime = 0,
        skillDuration = 1,
        isSkillPossible = true
    };

    [Header("Rolling")]
    SkillSettings rolling = new SkillSettings
    {
        skillCoolTime = 1f,
        isSkillPossible = true
    };

    [Header("Highnoon")]
    SkillSettings highnoon = new SkillSettings
    {
        ultimatePoint = 1800,
        isSkillPossible = true,
    };

    private void Start()
    {
        peacekeeperCurrentBulletCount = peacekeeper.bulletInitCount;
    }

    public void OnPointerDown()
    {
        if (highnoon.isSkillPossible ==false)
            return;

        _buttonDown = true;

        ult.FirstInput(highnoon.ultimatePoint);
    }

    public void OnPointerUp()
    {
        _buttonDown = false;

        if (highnoon.isSkillPossible == false)
            return;

        ult.FireUltimate();
    }

    void Update()
    {
        // NormalAttack();

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Skill_Flashbang(); 
        }

        if (Keyboard.current.shiftKey.wasPressedThisFrame)
        {
           Skill_CombatRoll();
        }

        if (_buttonDown)
        {
            ult.TrackInput();
            ult.IncreaseDamage();
        }

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            ult.FirstInput(highnoon.ultimatePoint);
        }

        if (Keyboard.current.qKey.isPressed)
        {
            ult.TrackInput();
            ult.IncreaseDamage();
        }

        if (Keyboard.current.qKey.wasReleasedThisFrame)
        {
            ult.FireUltimate();
        }
    }
        

    public override void NormalAttack()
    {
        StartCoroutine(NormalAtk());
    }


    public IEnumerator NormalAtk()
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
        StartCoroutine(Skill_FB());
    }

    IEnumerator Skill_FB()
    {
        if (!flashbang.isSkillPossible)
            yield break;
        projectileManager.FireProjectile(Camera.main.transform.position, (Camera.main.transform.forward + Camera.main.transform.up * 0.5f).normalized, flashbangBullet);
        flashbang.isSkillPossible = false;

        yield return new WaitForSeconds(flashbang.skillCoolTime);
        flashbang.isSkillPossible = true;
    }


    public void Skill_CombatRoll()
    {
        if (!rolling.isSkillPossible || _isRolling) return;
        StartCoroutine(C_Rolling(playerMovement.MoveDir));
    }

    private IEnumerator C_Rolling(Vector3 inputVector)
    {
        rolling.isSkillPossible = false;
        _isRolling = true;

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
        _isRolling = false;

        yield return new WaitForSeconds(rolling.skillCoolTime);
        rolling.isSkillPossible = true;
    }



}





