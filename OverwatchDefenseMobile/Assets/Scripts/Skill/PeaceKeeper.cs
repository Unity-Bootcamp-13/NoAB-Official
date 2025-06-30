using UnityEngine;


[CreateAssetMenu(fileName = "SkillStarter", menuName = "Scriptable Objects/SkillStarter")]
public class PeaceKeeper : SkillStarter
{
    public override void StartSkill()
    {
        Camera camera = Camera.main;
        Enemy enemy;

        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, 100.0f))
            return;

        if (hit.collider.CompareTag("Enemy"))
        {
            Debug.Log("총알 자동 발사");
            enemy = hit.collider.GetComponent<Enemy>();
            enemy.Damage(50);
        }
    }
}
