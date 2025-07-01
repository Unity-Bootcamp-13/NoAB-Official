using UnityEngine;


public enum SkillType
{
    None,
    HitScan,
    Projectile
}


public abstract class SkillStarter : ScriptableObject
{
    public abstract void StartSkill();
}


[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Objects/Skill")]
public class Skill : ScriptableObject
{
    public SkillType skill_type;
    public SkillStarter starter;
    public string skill_name;
    public int cooldown_time;
    public int atk;


    public void StartSkill()
    {
        starter.StartSkill();
    }
}
