using UnityEngine;

public class TurnController : MonoBehaviour
{
    public Unit player;
    public Unit enemy;
    private bool isPlayerTurn;

    
    public void EndTurn()
    {
        isPlayerTurn = !isPlayerTurn;
        if (!isPlayerTurn)
        {
            EnemyTurn();
        }
    }

    private void EnemyTurn()
    {
        
        EndTurn();
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
