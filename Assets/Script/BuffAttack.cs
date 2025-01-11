using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffAttack : AbilityBase
{



    void Start()
    {
        targetType = TargetType.Ally;
    }
    public override void Execute(Unit caster, Unit target)
    {
        target.PowerControl(true);
    }

    
}
