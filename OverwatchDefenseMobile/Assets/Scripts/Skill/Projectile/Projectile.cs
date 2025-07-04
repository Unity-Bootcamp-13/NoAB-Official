using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] Collider _collider;
    [SerializeField] ProjectileTarget target;
    private int _id;
    private float _lifetime;
    private int _damage;
    private float _spawnTime;
    private bool _isReleased = false;

    public int Id { get { return _id; } }

    private ProjectilePoolManager _projectilePoolManager;

    public void Initialize(Vector3 position, Vector3 direction, ProjectileSettings projectileSettings)
    {
        transform.position = position;

        _id = projectileSettings.id;
        _lifetime = projectileSettings.lifetime;
        _damage = projectileSettings.damage;
        _spawnTime = Time.time;
        _isReleased = false;

        _rigidbody.useGravity = projectileSettings.useGravity;
        _rigidbody.collisionDetectionMode = projectileSettings.collisionDetectionMode;
        _rigidbody.linearVelocity = direction * projectileSettings.speed;

        _collider.isTrigger = false;
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
            Release();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isReleased) return;

        if (collision.gameObject.CompareTag("Player"))
            return;

        if (_id == 10002)
        {
            List<Collider> targetlist = new List<Collider>(target.targetList);

            foreach (Collider target in targetlist)
            {
                Zomnic zomnic = target.GetComponent<Zomnic>();
                zomnic.TakeDamage(_damage);
                zomnic.isSlowed = true;
            }
        }

        Release();
    }

    private void Release()
    {
        _isReleased = true;
        _projectilePoolManager.ReturnProjectile(this);
    }
}