using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProperties : MonoBehaviour
{
    public static PlayerProperties Instance { get; private set; }
    [Header("Характеристики")]
    public int HP;
    public int maxHP;
    public float damageResistance;
    public List<GameObject> currentDeckBuild;
    public int fame;
    [Header("UI/UX")]
    [SerializeField] private Image hpBar;

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

	public void Start()
	{
        StartCombat();
	}

	public void StartCombat()
    {
        maxHP = fame;
    }

	public void TakeDamage(int damage)
    {
        if (fame - (int)(damage * damageResistance) <= 0)
        {
            Die();
            return;
        }
        if (damageResistance != 0)
        {
            fame -= (int)(damage * damageResistance);
        }
        else
        {
            fame -= damage;
        }
        UpdateViewModels();
    }

    public void Heal(int heal)
    {
        fame = Mathf.Clamp(fame + heal,0,maxHP);
        UpdateViewModels();
    }

    public void SetResistance(float resistance)
    {
        damageResistance = 1 - resistance;
    }

    private void UpdateViewModels()
    {
        hpBar.fillAmount = fame / (float)maxHP;
    }

    private void Die()
    {
        MenuManager.Instance.looseScreen.SetActive(true);
    }
}
