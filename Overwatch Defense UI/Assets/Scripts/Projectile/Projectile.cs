using UnityEngine;

public struct ProjectileSettings
{
    public string name;
    public float speed;
    public float lifetime;
    public int damage;
    public bool useGravity;
    public CollisionDetectionMode collisionDetectionMode;
}

[RequireComponent(typeof(Rigidbody),typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [SerializeField] Rigidbody rigidbody;
    private float _lifetime;
    private int _damage;

    private ProjectilePoolManager _projectilePoolManager;

    /// <summary>
    /// 
    /// </summary>
    public void Initialize(Vector3 position, Vector3 direction, ProjectileSettings projectileSettings)
    {
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(direction);

        _lifetime = projectileSettings.lifetime;
        _damage = projectileSettings.damage;

        rigidbody.useGravity = projectileSettings.useGravity;
        rigidbody.collisionDetectionMode = projectileSettings.collisionDetectionMode;
        rigidbody.linearVelocity = direction * projectileSettings.speed;
    }

    public void InjectPoolManager(ProjectilePoolManager projectilePoolManager)
    {
        _projectilePoolManager = projectilePoolManager;
    }
}