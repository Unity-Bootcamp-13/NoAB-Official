using System;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public CharacterSkillSet _characterSkillSet;

    private float _time;

    private void Update()
    {
        _time += Time.deltaTime;
        if (_time >= _characterSkillSet.skill_normal.cooldown_time)
        {
            _characterSkillSet.skill_normal.starter.StartSkill();
            _time = 0;
        }
    }
}