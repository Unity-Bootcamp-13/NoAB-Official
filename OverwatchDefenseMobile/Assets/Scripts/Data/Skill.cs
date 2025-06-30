using UnityEngine;


public enum SkillType
{
    None,
    HitScan,
    Projectile
}


[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Objects/Skill")]
public class Skill : ScriptableObject
{
    public SkillType skill_type;
    public string skill_name;
    public int cooldown_time;
    public int atk;



}
