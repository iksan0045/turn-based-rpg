using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{
    private float health;
    private float maxHealth;
    private float attackPower;
    private float defense;
    private bool isDefense;
    public bool myTurn;
    [SerializeField] private List<AbilityBase> abilities;

    public ParticleSystem shieldFx;
    public ParticleSystem attackFx;
    private bool isAlive;

    [SerializeField] private Image hpBar;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI attackText;
    [SerializeField] TextMeshProUGUI defText;
    [SerializeField] private bool isBuffed;
    [SerializeField] private bool isDebuffed;
    [SerializeField] private string unitName;
    [SerializeField] private TextMeshPro bufftext;
    [SerializeField] private TextMeshProUGUI textDamage;
    [SerializeField] private GameObject turnIndicator;

    public bool isSelected;
    public UnitData unitData;

    
    void Awake()
    {
        
        Initialize();
        isAlive = true;
    }

    void Update()
    {
        hpBar.fillAmount = health / maxHealth;
        attackText.text = "Attack :"+attackPower.ToString();
        defText.text = "Defend :" +defense.ToString();
        
        HandleVfxBuff();
        HandleTurnIndicator();
    }

    private void HandleVfxBuff()
    {
        if(isBuffed)
        {
            Debug.Log("play vfx buff");
            bufftext.text = "Buff";
        }
        else if (isDebuffed)
        {
            bufftext.text = "Debuff";
        }
        else
        {
            bufftext.text = "";
        }
    }

    public void HandleTurnIndicator()
    {
        if (isSelected)
        {
            turnIndicator.SetActive(true);
        }
        else
        {
            turnIndicator.SetActive(false);
        }
    }
    public void StartTurn()
    {
        isDefense = false;
        isDebuffed = false;
        isBuffed = false;
        myTurn = true;
    }

    public void EndTurn()
    {
        myTurn = false;
    }

    private void Initialize()
    {   
       maxHealth = unitData.maxHealth;
       health = maxHealth;
       unitName = unitData.characterName;
       nameText.text = unitName;
       attackPower = unitData.attackPower;
       defense = unitData.defense;
       Debug.Log("Unit name is :" + unitName);

    }
    public void TakeDamage(float damage)
    {
        float damageTaken;
        if(isDefense)
        {
            float damageReduction = 100 / (100 + defense);
            float effectiveDamage = damage * damageReduction;
            health -= effectiveDamage;
            damageTaken = damage * damageReduction;
        }
        else
        {
            health -= damage;
            damageTaken = damage;
        }
        attackFx.Play();
        textDamage.text = $"- {damageTaken}";
        health = Mathf.Clamp(health, 0, maxHealth);
        if (health <= 0)
        {
            isAlive = false;
            gameObject.SetActive(false);
        }
       
    }
    public void PowerControl(bool buff)
    {
        if (buff)
        {
            isBuffed = true;
        }
        else 
        {
            isDebuffed = true;
        }
    }

    public void AddHealthPoint(float healthAmount)
    {
        Debug.Log("heal");
        health += healthAmount;
        
    }

    public bool GetStatusAlive()
    {
        return isAlive;
    }

    public string GetName()
    {
        return unitName;
    }

    public void Defend()
    {
        shieldFx.Play();
        isDefense = true;
        EndTurn();
    }
    public float GetHealth()
    {
        return health;
    }

    public void Attack(Unit target)
    {
        if(isAlive)
        {
            StartCoroutine(MoveToTargetAndAttack(target));
        }
        
    }


    public void UseAbility(int abilityIndex, Unit target)
    {
        abilities[abilityIndex].Execute(this,target);
        EndTurn();
    }   

    public List<AbilityBase> GetAbilities()
    {
        return abilities;
    }

    private IEnumerator MoveToTargetAndAttack(Unit target)
    {
        Vector3 originalPosition = transform.position;

        Vector3 targetPosition = target.transform.position + (originalPosition - target.transform.position).normalized;

        while (Vector3.Distance(transform.position, targetPosition) > 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 3 * Time.deltaTime);

        }
        if(isBuffed)
        {
            target.TakeDamage(attackPower * 1.5f);
        }
        else if (isDebuffed)
        {
            target.TakeDamage(attackPower * 0.5f);
        }
        else
        {
            target.TakeDamage(attackPower);
        }
        

        yield return new WaitForSeconds(0.5f);

        while (Vector3.Distance(transform.position, originalPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, 5 * Time.deltaTime);
        }
        EndTurn();
    }
}
