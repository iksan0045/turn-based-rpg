using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public GameObject prefabButton;
    public BattleManager battleManager;
    [SerializeField] private TextMeshProUGUI nameUnitText;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private Transform enemyTargetButtonContainer;
    [SerializeField] private Transform heroTargetButtonContainer;
    [SerializeField] private Transform heroAbilityButtonContainer;
    [SerializeField] private GameObject abilityButtonPrefab;
    [SerializeField] private List<Button> enemyTargetButtons = new List<Button>();
    [SerializeField] private List<Button> heroTargetButtons = new List<Button>();
    [SerializeField] private GameObject turnPanel;
    [SerializeField] private GameObject actionPanel;
    [SerializeField] private GameObject targetPanel;
    [SerializeField] private GameObject targetAllyPanel;
    [SerializeField] private GameObject winPanel;


    void Awake()
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

    public void NameUnitTurn(string nameUnit)
    {
        nameUnitText.text = $"Current Character is {nameUnit}" ;
    }

    void Start()
    {
       InitializeEnemyTargetUI();
       InitializeHeroTargetUI();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void TurnActionSelection()
    {
        actionPanel.SetActive(true);
        targetAllyPanel.SetActive(false);
        targetPanel.SetActive(false); 
    }

    public void TurnTargetSelection(bool ally)
    {
        if (ally)
        {
            targetAllyPanel.SetActive(true);
            targetPanel.SetActive(false); 
        }
        else
        {
            targetPanel.SetActive(true);
            targetAllyPanel.SetActive(false); 
        }
    }

    public void ShowResultPanel(bool win)
    {
        winPanel.SetActive(true);
        if (win)
        {
            resultText.text = "We Win";
        }
        else
        {
            resultText.text = "We Lose";
        }

    }
    public void InitializeEnemyTargetUI()
    {
        InitializeTargetButtons(battleManager.GetEnemyUnit(), enemyTargetButtons, enemyTargetButtonContainer, battleManager.SelectTarget);

    }

    private void InitializeHeroTargetUI()
    {
        InitializeTargetButtons(battleManager.GetPlayerUnit(), heroTargetButtons, heroTargetButtonContainer, battleManager.SelectHeroTarget);
    }
    private void InitializeTargetButtons(List<Unit> units, List<Button> targetPool, Transform container, System.Action<int> onClickCallback)
    {

        if (targetPool.Count < units.Count)
        {
            for (int i = targetPool.Count; i < units.Count; i++)
            {
                GameObject buttonObj = Instantiate(prefabButton, container);
                Button button = buttonObj.GetComponent<Button>();
                targetPool.Add(button);

                int index = i; 
                button.onClick.AddListener(() => onClickCallback(index));
            }
        }

        for (int i = 0; i < targetPool.Count; i++)
        {
            if (i < units.Count && units[i].GetStatusAlive())
            {
                targetPool[i].gameObject.SetActive(true);
                TextMeshProUGUI unitText = targetPool[i].GetComponentInChildren<TextMeshProUGUI>();
                unitText.text = units[i].GetName();
            }
            else
            {
                targetPool[i].gameObject.SetActive(false);
            }
        }
    }

    public void UpdateActionPanel(List<AbilityBase> abilities)
    {
        foreach (Transform child in heroAbilityButtonContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < abilities.Count; i++)
        {
            int index = i;
            AbilityBase ability = abilities[i];

            GameObject buttonObj = Instantiate(abilityButtonPrefab, heroAbilityButtonContainer);
            Button button = buttonObj.GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = ability.abilityName;
            button.onClick.AddListener(() =>
            {
                SelectAbility(index);
            });
        }
    }

    private void SelectAbility(int abilityIndex)
    {
        BattleManager.Instance.UseAbility(abilityIndex);
    }
    
}
