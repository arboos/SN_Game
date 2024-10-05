using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProperties : MonoBehaviour
{
    public static PlayerProperties Instance { get; private set; }
    [Header("��������������")]
    public int maxHP;
    public float damageResistance;
    public List<GameObject> currentDeckBuild;
    public int fame;
    [Header("UI/UX")]
    [SerializeField] private Image hpBar;
    [SerializeField] private HonestReactions honestReaction;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
            DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void Start()
	{
        StartCombat();
		UpdateViewModels();
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
        //fame = Mathf.Clamp(fame + heal,0,maxHP);
        fame = fame + heal;
        UpdateViewModels();
    }

    public void SetResistance(float resistance)
    {
        damageResistance = 1 - resistance;
    }

    private void UpdateViewModels()
    {
        hpBar.fillAmount = fame / (float)maxHP;
        CardPlacementSystem.Instance.textHP_Player.text = fame.ToString();
    }

    private void Die()
    {
        MenuManager.Instance.looseScreen.SetActive(true);
    }
}
