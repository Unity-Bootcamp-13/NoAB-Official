using System;
using UnityEngine;

public enum SkillType
{
    None,
    Attack,
    Move
}

public enum CastType
{
    None,
    Auto,
    Charging,
    Touch
}

public enum RequirementType
{
    None,
    Cooltime,
    UltGauge
}


public class Skill
{
    public SkillType skilltype;
    public CastType castType;
    public RequirementType requirementType;
    public Projectile projectilePrefab; // 프리팹으로 변경
    public Action action;

    public Skill(SkillType SkillType, CastType CastType, RequirementType RequirementType, Projectile ProjectilePrefab)
    {
        this.skilltype = SkillType;
        this.castType = CastType;
        this.requirementType = RequirementType;
        this.projectilePrefab = ProjectilePrefab;

        TypeLogic(SkillType);
        CastLogic(CastType, ProjectilePrefab);
        RequirementLogic(RequirementType);
    }

    public void TypeLogic(SkillType skillType)
    {
        switch(skillType)
        {
            case SkillType.None: 
                break;
            case SkillType.Attack:
                {
                    // CastLogic에서 받아온 함수 따라서 RequirementLogic에서 받아온 함수로 리턴받은게 true면 투사체 나가게
                    // action += ~~~
                    action += () => {
                        // 여기서 바로 로직 작성
                        Debug.Log($"타입은 {skillType}");
                    };
                    break;
                }
            case SkillType.Move:
                {
                    // action += 이동 로직
                    break;
                }
        }
    }


    public void CastLogic(CastType castType, Projectile projectile)
    {
        switch(castType)
        {
            case CastType.None:
                break;
            case CastType.Auto:
                {// 자동으로 쏘는 로직
                    action += () => {
                        // 여기서 바로 로직 작성
                        Debug.Log($"캐스트타입은 {castType}, 투사체종류는 {projectile}");
                    };
                }
                break;
            case CastType.Charging:
                // 차징해서 쏘는 로직
                break;
        }
    }

    public void RequirementLogic(RequirementType requirementType)
    {
        switch (requirementType)
        {
            case RequirementType.None:
                break;
            case RequirementType.Cooltime:
                // 쿨타임에 따라 쿨 도는 로직
                break;
            case RequirementType.UltGauge:
                // 게이지 차면 가능되는 로직
                break;
        }
    }


    public void Activate()
    {
        action.Invoke(); // 명령 시점에서 실행
    }
}
