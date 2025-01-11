using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }
    [SerializeField] private List<Unit> playerParty;
    [SerializeField] private List<Unit> enemyParty;

    private Unit _selectedUnit;
    private Unit _selectedAllyUnit;
    private Unit _selectedTarget;
    [SerializeField] private Action actionTaken;
    [SerializeField] private TurnState playerTurnState;
    private int selectedAbiltyIndex;

    public bool isPlayerTurn;
    private int currentCharacterIndex = 0; 



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        isPlayerTurn = true;
        StartTurn();
        
    }

    void Update()
    {
        switch (playerTurnState)
        {
            case TurnState.None:
                break;
            case TurnState.SelectAction:
                UIManager.Instance.TurnActionSelection();
                break;          
            case TurnState.SelecTarget:
                UIManager.Instance.TurnTargetSelection(false);
                break;        
            case TurnState.SelectTargetAlly:
                UIManager.Instance.TurnTargetSelection(true);
                break;
        }

        if(AreAllEnemiesDefeat())
        {
            Debug.Log("we win");
            UIManager.Instance.ShowResultPanel(true);
        }
        else if (AreAllAllyDefeat())
        {
            UIManager.Instance.ShowResultPanel(false);
        }
    }

    public void StartTurn()
    {
        
        currentCharacterIndex = 0;
        actionTaken = Action.None;
       
        if (isPlayerTurn)
        {
           foreach(var hero in playerParty)
           {
                hero.StartTurn();
           }
            SelectNextPlayerUnit();
        }
        else
        {
            
            StartCoroutine(EnemyTurn());
        }
    }

    private void SelectNextPlayerUnit()
    {
        playerTurnState = TurnState.SelectAction;
        

         while (currentCharacterIndex < playerParty.Count && !playerParty[currentCharacterIndex].GetStatusAlive())
        {
            currentCharacterIndex++;
        }

        if (currentCharacterIndex < playerParty.Count)
        {
            foreach (var unit in playerParty)
            {
                unit.isSelected = false;
            }

            _selectedUnit = playerParty[currentCharacterIndex];
            _selectedUnit.isSelected = true;
            UIManager.Instance.NameUnitTurn(_selectedUnit.GetName());
            
            UIManager.Instance.UpdateActionPanel(_selectedUnit.GetAbilities());
        }
        else
        {
            EndPlayerTurn();
        }
    }

    public void OnPlayerActionCompleted()
    {
        currentCharacterIndex++;

        if (currentCharacterIndex < playerParty.Count)
        {
            SelectNextPlayerUnit();
        }
        else
        {
            EndPlayerTurn();
        }
        UIManager.Instance.InitializeEnemyTargetUI();
    }

    public void EndPlayerTurn()
    {
        isPlayerTurn = false;
        StartTurn();
    }

    private IEnumerator EnemyTurn()
    {
        if (AreAllEnemiesDefeat())
        {
            yield break; 
        }
        for (int i = 0; i < enemyParty.Count; i++)
        {
            yield return new WaitForSeconds(1f); 
            EnemyAI enemyAI = enemyParty[i].GetComponent<EnemyAI>();
            enemyAI.TakeTurn();
        }

        EndEnemyTurn();
    }

    private bool AreAllEnemiesDefeat()
    {
        foreach (var enemy in enemyParty)
        {
            if (enemy.GetStatusAlive())
            {
                return false; 
            }
        }
        return true; 
    }

    private bool AreAllAllyDefeat()
    {
        foreach (var ally in playerParty)
        {
            if (ally.GetStatusAlive())
            {
                return false; 
            }
        }
        return true; 
    }

    public void UseAbility(int abilityIndex)
    {
        selectedAbiltyIndex = abilityIndex;
        actionTaken = Action.Ability;
        if(_selectedUnit.GetAbilities()[abilityIndex].targetType == TargetType.Ally)
        {
            playerTurnState = TurnState.SelectTargetAlly;
        }        
        else if (_selectedUnit.GetAbilities()[abilityIndex].targetType == TargetType.Enemy)
        {
            playerTurnState = TurnState.SelecTarget;
        }
    }

    private void EndEnemyTurn()
    {
        isPlayerTurn = true;
        StartTurn();
    }

    public void ChoseAttack()
    {
        actionTaken = Action.Attack;
        playerTurnState = TurnState.SelecTarget;
    }

    public void ChoseDefend()
    {
        _selectedUnit.Defend();
        playerTurnState = TurnState.SelecTarget;
        OnPlayerActionCompleted();
    }

    

    public List<Unit> GetPlayerUnit()
    {
        return playerParty;
    }

    public List<Unit> GetEnemyUnit()
    {
        return enemyParty;
    }

    public void SelectTarget(int targetIndex)
    {
        if (actionTaken == Action.Attack)
        {
            _selectedUnit.Attack(enemyParty[targetIndex]);
            OnPlayerActionCompleted();
        }
        else if (actionTaken == Action.Ability)
        {
            if (_selectedUnit != null && selectedAbiltyIndex < _selectedUnit.GetAbilities().Count)
            {
                _selectedUnit.UseAbility(selectedAbiltyIndex,enemyParty[targetIndex]);
                OnPlayerActionCompleted();
            }
        }
    }

    public void SelectHeroTarget(int targetIndex)
    {
       
        if (actionTaken == Action.Ability)
        {
            if(_selectedUnit != null)
            {
                if (_selectedUnit != null && selectedAbiltyIndex < _selectedUnit.GetAbilities().Count)
                {
                    _selectedUnit.UseAbility(selectedAbiltyIndex,playerParty[targetIndex]);
                    OnPlayerActionCompleted();
                }
            }
        }
    }

    public enum TurnState
    {
        None,
        SelectAction,
        SelecTarget,
        SelectTargetAlly,
    }
    public enum Action
    {
        None,
        Attack,
        Defend,
        Ability
    }
}
