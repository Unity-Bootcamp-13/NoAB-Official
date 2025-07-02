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
    public Projectile projectilePrefab; // ���������� ����
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
                    // CastLogic���� �޾ƿ� �Լ� ���� RequirementLogic���� �޾ƿ� �Լ��� ���Ϲ����� true�� ����ü ������
                    // action += ~~~
                    action += () => {
                        // ���⼭ �ٷ� ���� �ۼ�
                        Debug.Log($"Ÿ���� {skillType}");
                    };
                    break;
                }
            case SkillType.Move:
                {
                    // action += �̵� ����
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
                {// �ڵ����� ��� ����
                    action += () => {
                        // ���⼭ �ٷ� ���� �ۼ�
                        Debug.Log($"ĳ��ƮŸ���� {castType}, ����ü������ {projectile}");
                    };
                }
                break;
            case CastType.Charging:
                // ��¡�ؼ� ��� ����
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
                // ��Ÿ�ӿ� ���� �� ���� ����
                break;
            case RequirementType.UltGauge:
                // ������ ���� ���ɵǴ� ����
                break;
        }
    }


    public void Activate()
    {
        action.Invoke(); // ��� �������� ����
    }
}
