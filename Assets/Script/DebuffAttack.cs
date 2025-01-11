using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffAttack: AbilityBase
{


    void Start()
    {
        targetType = TargetType.Enemy;
    }
    public override void Execute(Unit caster, Unit target)
    {
        target.PowerControl(false);
    }

    
}
