using UnityEngine;


[CreateAssetMenu(fileName = "CharacterSkillSet", menuName = "Scriptable Objects/CharacterSkillSet")]
public class CharacterSkillSet : ScriptableObject
{
    public string character_name;
    public Skill skill_normal;
    public Skill skill_01;
    public Skill skill_02;
    public Skill skill_ultimate;
}
