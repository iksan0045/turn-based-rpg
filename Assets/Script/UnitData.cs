using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "UnitData")]
public class UnitData : ScriptableObject
{
    public string characterName;
    public int maxHealth;
    public int attackPower;
    public int defense;
    public int abilityId;
    public Sprite characterIcon;
}