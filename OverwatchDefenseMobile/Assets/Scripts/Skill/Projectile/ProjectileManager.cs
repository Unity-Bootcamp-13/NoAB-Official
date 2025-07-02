using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] ProjectilePoolManager projectilePoolManager;

    public void FireProjectile(Vector3 position, Vector3 direction, ProjectileSettings projectileSettings)
    {
        Projectile projectile = projectilePoolManager.GetProjectile(projectileSettings.id);
        projectile.Initialize(position, direction, projectileSettings);
    }
}