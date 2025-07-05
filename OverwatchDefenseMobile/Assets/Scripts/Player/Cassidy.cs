using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class Cassidy : Character
{
    [SerializeField] private ProjectileManager projectileManager;
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private PlayerMovement PlayerMovement;

    private int peacekeeperCurrentBulletCount;

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
        skillCoolTime = 6f,
        isSkillPossible = true
    };

    private void Start()
    {
        peacekeeperCurrentBulletCount = peacekeeper.bulletInitCount;
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
        Vector3 inputRollVector  = PlayerMovement.MoveDir;
        
        StartCoroutine(C_Rolling(inputRollVector));
    }

    public IEnumerator C_Rolling(Vector3 inputVector)
    {
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0f;
        forward.Normalize();

        // playerRb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        playerRb.useGravity = false;

        float rollDuration = 0.25f;
        float elapsed = 0;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(30, transform.eulerAngles.y, transform.eulerAngles.z);

        Vector3 startPos = transform.position;
        Vector3 endPos;



        if (inputVector == Vector3.zero)
        {
            endPos = startPos + forward * 6f;
        }
        else
        {
            endPos = startPos + inputVector * 6f;
        }

        Vector3 midPos = (startPos + endPos) / 2f;



        while (elapsed < rollDuration)
        {
            float t = elapsed / rollDuration;

            Vector3 targetPos = Vector3.Lerp(startPos, midPos, t);
            Quaternion targetRot = Quaternion.Lerp(startRotation, endRotation, t);

            playerRb.MovePosition(targetPos);
            playerRb.MoveRotation(targetRot);

            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        elapsed = 0;

        while (elapsed < rollDuration)
        {
            float t = elapsed / rollDuration;

            Vector3 targetPos = Vector3.Lerp(midPos, endPos, t);
            Quaternion targetRot = Quaternion.Lerp(endRotation, startRotation, t);

            playerRb.MovePosition(targetPos);
            playerRb.MoveRotation(targetRot);

            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        playerRb.useGravity = true;
    }

}
        
    
    


