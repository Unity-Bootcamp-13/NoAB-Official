using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] Collider _collider;
    private int _id;
    private float _lifetime;
    private int _damage;
    private float _spawnTime;
    private bool _isReleased = false;
    private bool _useRay;

    public int Id { get { return _id; } }

    private ProjectilePoolManager _projectilePoolManager;

    public void Initialize(Vector3 position, Vector3 direction, ProjectileSettings projectileSettings)
    {
        transform.position = position;

        _id = projectileSettings.id;
        _lifetime = projectileSettings.lifetime;
        _damage = projectileSettings.damage;
        _useRay = projectileSettings.useRay;
        _spawnTime = Time.time;
        _isReleased = false;

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
        if (_isReleased) return;

        if (Time.time - _spawnTime > _lifetime)
        {
            _projectilePoolManager.ReturnProjectile(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_useRay) return;

        if (other.CompareTag("Zomnic"))
        {
            Zomnic zomnic = other.GetComponent<Zomnic>();
            zomnic.TakeDamage(_damage);
            zomnic.isSlowed = true;
            _projectilePoolManager.ReturnProjectile(this);
        }
    }
}