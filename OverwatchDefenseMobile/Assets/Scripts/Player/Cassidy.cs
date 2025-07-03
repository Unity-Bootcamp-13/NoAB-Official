using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cassidy : Character
{
    [SerializeField] private ProjectileManager projectileManager;
    [SerializeField] private Rigidbody _playerRb;
    [SerializeField] private PlayerMovement PlayerMovement;

    private const float peacekeeperFireCoolTime = 0.5f;
    private const float reloadTime = 1.5f;
    private const float flashbangCoolTime = 5f;
    private bool flashbangOk = true;

    private bool peacekeeperOk = true;
    private const int peacekeeperInitBulletCount = 6;
    private int peacekeeperCurrentBulletCount;

    [Header("Peacekeeper")]
    ProjectileSettings peacekeeper = new ProjectileSettings
    {
        id = 10001,
        speed = 500,
        lifetime = 2f,
        damage = 100,
        useGravity = false,
        useRay = true,
        collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative
    };

    [Header("Flashbang")]
    ProjectileSettings flashbang = new ProjectileSettings
    {
        id = 10002,
        speed = 30f,
        lifetime = 5f,
        damage = 50,
        useGravity = true,
        useRay = false,
        collisionDetectionMode = CollisionDetectionMode.Discrete
    };

    private void Start()
    {
        peacekeeperCurrentBulletCount = peacekeeperInitBulletCount;
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Skill_Flashbang();
        }
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            NormalAttack();
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
        if (!peacekeeperOk)
            yield break;

        projectileManager.FireProjectile(Camera.main.transform.position, Camera.main.transform.forward, peacekeeper);

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
                

        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            if (hit.collider.CompareTag("Zomnic"))
            {
                peacekeeperOk = false;
                peacekeeperCurrentBulletCount--;

                Debug.Log("총알 발사");
                Zomnic zomnic = hit.collider.GetComponent<Zomnic>();
                zomnic.TakeDamage(peacekeeper.damage);
            }
        }
        yield return new WaitForSeconds(peacekeeperFireCoolTime);

        if (peacekeeperCurrentBulletCount <= 0)
        {
            Debug.Log("재장전");
            yield return new WaitForSeconds(reloadTime);
            peacekeeperCurrentBulletCount = peacekeeperInitBulletCount;
            Debug.Log("재장전 완료");
        }

        peacekeeperOk = true;
    }

    public void Skill_Flashbang()
    {
        StartCoroutine(Skill_FB());
    }

    IEnumerator Skill_FB()
    {
        if (!flashbangOk)
            yield break;
        projectileManager.FireProjectile(Camera.main.transform.position, (Camera.main.transform.forward + Camera.main.transform.up * 0.5f).normalized, flashbang);
        flashbangOk = false;

        yield return new WaitForSeconds(flashbangCoolTime);
        flashbangOk = true;
    }

    public void Skill_CombatRoll()
    {
        Vector3 _inputRollVector  = PlayerMovement.InputVector;


        // 방향 입력이 있으면 
        if (_inputRollVector != Vector3.zero)
        {
            
        }
        else // 방향 입력이 없으면      
        {
            
            StartCoroutine(C_Rolling());

        }
    }

    public override void Ultimate()
    {
        
    }

    public IEnumerator C_Rolling()
    {
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0f;
        forward.Normalize();

        _playerRb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        _playerRb.useGravity = false;

        float rollDuration = 0.25f;
        float elapsed = 0;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(30, transform.eulerAngles.y, transform.eulerAngles.z);


        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + forward * 6f;

        Vector3 midPos = (startPos + endPos) / 2f;
              


        while (elapsed < rollDuration)
        {
            float t= elapsed / rollDuration;

            transform.position = Vector3.Lerp(startPos, midPos, t); 
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0;

        while (elapsed < rollDuration)
        {
            float t = elapsed / rollDuration;

            transform.position = Vector3.Lerp(midPos, endPos, t);
            transform.rotation = Quaternion.Lerp(endRotation, startRotation, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        _playerRb.useGravity = true;        
    }
}