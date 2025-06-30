using UnityEngine;


public class PlayerSkill : MonoBehaviour
{
    [SerializeField] private CharacterSkillSet _characterSkillSet;
    [SerializeField] private Camera _camera;


    void Update()
    {
        if (_characterSkillSet.skill_normal.skill_type == SkillType.HitScan)
        {
            HitScanAttack();
        }

        if (_characterSkillSet.skill_normal.skill_type == SkillType.Projectile)
        {

        }
    }


    public void HitScanAttack()
    {
        Ray attackRay = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit attackedEnemy;

        if (!Physics.Raycast(attackRay, out attackedEnemy, 100.0f))
            return;

        if (attackedEnemy.collider.CompareTag("Enemy"))
        {
            Debug.Log("총알 자동 발사");
        }
    }
}