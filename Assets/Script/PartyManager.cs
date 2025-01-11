using UnityEngine;

public class PartyManager : MonoBehaviour
{
    private Unit[] units;
    private Unit targetUnit;
    private Unit selectedUnit;

    public void SelectHeroUnit(int index)
    {
        selectedUnit = units[index];
    }

    public void SelectTarget(Unit target)
    {
        targetUnit = target;
    }

}