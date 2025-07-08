using UnityEngine;

public class ProjectileEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem flashbangEffect;
    [SerializeField] AudioSource flashbangSound;

    public void PlayFlashbangEffect(Vector3 transform)
    {
        flashbangEffect.transform.position = transform;
        flashbangEffect.Play();
        flashbangSound.Play();
    }
}