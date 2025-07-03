using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cassidy : Character
{
    [SerializeField] private ProjectileManager projectileManager;
    [SerializeField] private Rigidbody _playerRb;
    
    private Vector3 _rollDirVector;
    private float _rotationY;


    [Header("Peacekeeper")]
    ProjectileSettings peacekeeper = new ProjectileSettings
    {
        id = 10001,
        speed = 30f,
        lifetime = 5f,
        damage = 100,
        useGravity = false,
        collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic
    };

    [Header("Flashbang")]
    ProjectileSettings flashbang = new ProjectileSettings
    {
        id = 10002,
        speed = 30f,
        lifetime = 5f,
        damage = 50,
        useGravity = true,
        collisionDetectionMode = CollisionDetectionMode.Discrete
    };

    private void Awake()
    {
        _rotationY = transform.eulerAngles.y;
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
        projectileManager.FireProjectile(Camera.main.transform.position, Camera.main.transform.forward, peacekeeper);
    }

    public void Skill_Flashbang()
    {
        projectileManager.FireProjectile(Camera.main.transform.position, (Camera.main.transform.forward + Camera.main.transform.up * 0.5f).normalized, flashbang);
    }

    public void Skill_CombatRoll()
    {
        Vector3 _inputRollVector  = _playerRb.linearVelocity.normalized;

        // 방향 입력이 없으면
        if (_inputRollVector == Vector3.zero)
        {
            Debug.Log("입력없었다");            
            StartCoroutine(C_Rolling()); 
        }
        else // 방향 입력이 있으면            
        {
            Debug.Log("입력있었다");
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