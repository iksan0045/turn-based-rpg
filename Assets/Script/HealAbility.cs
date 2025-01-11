using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbility : AbilityBase
{
    public float healAmount = 15;//example


    void Start()
    {
        targetType = TargetType.Ally;
    }
    public override void Execute(Unit caster, Unit target)
    {
        target.AddHealthPoint(healAmount);
    }

    
}
