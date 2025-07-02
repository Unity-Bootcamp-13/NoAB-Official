using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] ProjectilePoolManager projectilePoolManager;
    [SerializeField] Camera camera;

    [Header("Peacekeeper")]
    ProjectileSettings peacekeeper = new ProjectileSettings
    {
        name = "Peacekeeper",
        speed = 8000f,
        lifetime = 0.1f,
        damage = 70,
        useGravity = false,
        collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic
    };

    [Header("Flashbang")]
    ProjectileSettings flashbang = new ProjectileSettings
    {
        name = "Flashbang",
        speed = 30f,
        lifetime = 5f,
        damage = 25,
        useGravity = true,
        collisionDetectionMode= CollisionDetectionMode.Discrete
    };

    public void FireProjectile(Vector3 position, Vector3 direction, ProjectileSettings projectileSettings)
    {
        if (projectileSettings.name == "Peacekeeper")
        {
            Projectile projectile = projectilePoolManager.GetPeacekeeper();
            projectile.Initialize(position, direction, peacekeeper);
        }

        if (projectileSettings.name == "Flashbang")
        {
            Projectile projectile = projectilePoolManager.GetFlashbang();
            projectile.Initialize(position, direction, flashbang);
        }
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            FireProjectile(camera.transform.position, camera.transform.forward, peacekeeper);
        }

        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            FireProjectile(camera.transform.position, (camera.transform.forward + camera.transform.up * 0.5f).normalized, flashbang);
        }
    }
}