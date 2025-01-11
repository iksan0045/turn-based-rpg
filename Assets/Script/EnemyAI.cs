using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Unit _unit;

    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    public void TakeTurn()
    {
        if (_unit.GetHealth() < 30)
        {
            Defend();
        }
        else
        {
            Attack();
        }
    }

    private void Attack()
    {
        Unit target = BattleManager.Instance.GetPlayerUnit()[Random.Range(0, BattleManager.Instance.GetPlayerUnit().Count)];
        _unit.Attack(target);
    }

    private void Defend()
    {
        _unit.Defend();
    }

    
}
