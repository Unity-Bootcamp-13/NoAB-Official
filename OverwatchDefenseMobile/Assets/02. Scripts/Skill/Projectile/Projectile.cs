using System.Collections.Generic;
using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] Collider _collider;
    [SerializeField] ProjectileTarget target;
    [SerializeField] ParticleSystem collisionEffect;
    [SerializeField] AudioSource collisionSound;

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
        _isCoStarted = false;

        _rigidbody.useGravity = projectileSettings.useGravity;
        _rigidbody.collisionDetectionMode = projectileSettings.collisionDetectionMode;
        _rigidbody.linearVelocity = direction * projectileSettings.speed;

        _collider.isTrigger = false;

        if (_id == 10001)
        {
          //  ParticleSystem effect = Instantiate(collisionEffect, Camera.main.transform.position + Camera.main.transform.forward, Quaternion.identity);
          //  effect.Clear();
          //  effect.Play();

            collisionSound.Play();
            StartCoroutine(C_ReleaseAfterSound());
        }
    }

    private IEnumerator C_ReleaseAfterSound()
    {
        yield return new WaitForSeconds(collisionSound.clip.length);
        Release();
    }

    public void InjectPoolManager(ProjectilePoolManager projectilePoolManager)
    {
        _projectilePoolManager = projectilePoolManager;
    }

    private void Update()
    {
        if (_isReleased ||
            _isCoStarted) return;

        if (Time.time - _spawnTime > _lifetime)
        {
            if (_id == 10001)
                Release();
            else if (_id == 10002)
                StartCoroutine(C_PlayFlashbangEffect());
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
            
            if (!_isCoStarted)
                StartCoroutine(C_PlayFlashbangEffect());
        }  
    }

    private void Release()
    {
        if (_isReleased) return;        

        _isReleased = true;
        _projectilePoolManager.ReturnProjectile(this);
    }

    private bool _isCoStarted;
    private IEnumerator C_PlayFlashbangEffect()
    {
        _isCoStarted = true;
        collisionEffect.Play();
        collisionSound.Play();
        
        yield return new WaitForSeconds(collisionEffect.main.duration);
        Release();
    }
}