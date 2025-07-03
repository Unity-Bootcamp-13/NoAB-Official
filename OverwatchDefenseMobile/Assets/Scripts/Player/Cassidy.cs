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
        speed = 8000f,
        lifetime = 0.1f,
        damage = 70,
        useGravity = false,
        collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic
    };

    [Header("Flashbang")]
    ProjectileSettings flashbang = new ProjectileSettings
    {
        id = 10002,
        speed = 30f,
        lifetime = 5f,
        damage = 25,
        useGravity = true,
        collisionDetectionMode = CollisionDetectionMode.Discrete
    };

    private void Awake()
    {
        _rotationY = transform.eulerAngles.y;
    }


    IEnumerator C_Rolling(Vector3 rollDirVector)
    {
        float Z = rollDirVector.x;
        float X = rollDirVector.z;


        while (X * X + Z * Z < 900)
        {
            _playerRb.rotation = Quaternion.Euler(-X, _rotationY, Z);

            X += X;
            Z -= Z;

            //_playerRb.MovePosition(new Vector3(0, -Time.deltaTime, 0));
        }

        yield return null;
    }


    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Skill_Flashbang();
        }
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            NormalAttack();
        }
        if (Keyboard.current.shiftKey.wasPressedThisFrame)
        {
            // Skill_CombatRoll();
            StartCoroutine(C_Rolling());
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

        // 입력이 없으면
        if (_inputRollVector == Vector3.zero)
        {
            Debug.Log("입력없었다");
            // 구를 방향 = Camera.main.transform.forward
            StartCoroutine(C_Rolling(Camera.main.transform.forward)); 
        }
        else
        {
            Debug.Log("입력있었따");
            // 입력이 있으면
            // 구를 방향 =_moveDir
            StartCoroutine(C_Rolling(_inputRollVector));
        }

        // 구르기
        // 캐서디 rotation x값 조절해주기
       
    }

    public override void Ultimate()
    {
        
    }

    public IEnumerator C_Rolling()
    {
        Vector3 vector3 = Camera.main.transform.forward;
        vector3.y = 0f;

        _playerRb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        float i = 1;

        while (i < 10)
        {
            transform.Rotate(new Vector3(5, 0, 0));
            i++;            
        }
        _playerRb.useGravity = true;
        _playerRb.AddForce(Vector3.down * 200);
        transform.position += vector3 * 8;

        yield return new WaitForSeconds(2f);
        _playerRb.useGravity = false;
        transform.Rotate(new Vector3(-50, 0, 0));
        _playerRb.useGravity = true;
    }
}