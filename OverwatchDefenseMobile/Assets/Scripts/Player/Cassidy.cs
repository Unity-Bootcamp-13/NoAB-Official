using UnityEngine;
using UnityEngine.Rendering;

public class Cassidy : MonoBehaviour
{

    private Skill normalAttack;

    public Projectile attackProjectile;
    public Projectile skill01Projectile;
    public Projectile skill02Projectile;
    public Projectile ultimateProjectile;

    void Start()
    {
        normalAttack = new Skill(SkillType.Attack, CastType.Auto, RequirementType.Cooltime, attackProjectile);
        normalAttack.action += NormalAttack;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            normalAttack.Activate();
    }

    private void NormalAttack()
    {
        Debug.Log("ATTACK");

    }
}
