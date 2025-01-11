using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : MonoBehaviour
{
    public string abilityName;
    public string description;
    public TargetType targetType;
    public bool isOnCoolDown;
    public int coolDownTurn;

    public abstract void Execute(Unit caster, Unit target);

    public virtual bool CanExecute(Unit caster)
    {
        return true;
    }
}
public enum TargetType
{
    Enemy,   
    Ally,   
    Self     
}