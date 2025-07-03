using UnityEngine;

public struct ProjectileSettings
{
    public int id;
    public int damage;
    public float speed;
    public float lifetime;
    public bool useGravity;
    public CollisionDetectionMode collisionDetectionMode;
}

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] Collider _collider;
    private int _id;
    private float _lifetime;
    private int _damage;
    private float _spawnTime;

    public int Id { get { return _id; } }

    private ProjectilePoolManager _projectilePoolManager;

    public void Initialize(Vector3 position, Vector3 direction, ProjectileSettings projectileSettings)
    {
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(direction);

        _id = projectileSettings.id;
        _lifetime = projectileSettings.lifetime;
        _damage = projectileSettings.damage;
        _spawnTime = Time.time;

        _rigidbody.useGravity = projectileSettings.useGravity;
        _rigidbody.collisionDetectionMode = projectileSettings.collisionDetectionMode;
        _rigidbody.linearVelocity = direction * projectileSettings.speed;

        _collider.isTrigger = true;
    }

    public void InjectPoolManager(ProjectilePoolManager projectilePoolManager)
    {
        _projectilePoolManager = projectilePoolManager;
    }

    private void Update()
    {
        if (Time.time - _spawnTime > _lifetime)
        {
            _projectilePoolManager.ReturnProjectile(this);
        }
    }
}