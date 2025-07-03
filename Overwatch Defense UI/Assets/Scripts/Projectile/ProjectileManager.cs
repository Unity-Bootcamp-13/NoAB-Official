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

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] ProjectilePoolManager projectilePoolManager;

    public void FireProjectile(Vector3 position, Vector3 direction, ProjectileSettings projectileSettings)
    {
        Projectile projectile = projectilePoolManager.GetProjectile(projectileSettings.id);
        projectile.Initialize(position, direction, projectileSettings);
    }
}