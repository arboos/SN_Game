using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerProperties : MonoBehaviour
{
	public static PlayerProperties Instance { get; private set; }
	[Header("��������������")]
	public int maxHP;
	public int damageResistance;
	public List<GameObject> currentDeckBuild;
	public int fame;
	[Header("UI/UX")]
	[SerializeField] private Image hpBar;
	public HonestReactions honestReaction;
	[Header("Audio")]
	public AudioSource audioSource;
	public AudioClip defeat;
	public AudioClip win;
	public AudioClip cardPlaced;
	public AudioClip cardTaken;
	public AudioClip damaged;
	public AudioClip healed;
	public AudioClip shopMusic;

	private void Awake()
	{
		SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
		{
			honestReaction = GameObject.Find("HonestReaction").GetComponent<HonestReactions>();
		};

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
		if (damage - damageResistance > 0)
		{
			honestReaction.Shake(1);
			audioSource.PlayOneShot(damaged);
			honestReaction.PlayAngry();
		}
		if (fame - (damage - damageResistance) <= 0)
		{
			Die();
			return;
		}
		if (damageResistance != 0)
		{
			fame -= Mathf.Clamp(damage - damageResistance,0,1000);
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
		audioSource.PlayOneShot(healed);
		fame = fame + heal;
		UpdateViewModels();
	}

	public void SetResistance(int resistance)
	{
		damageResistance = resistance;
	}

	private void UpdateViewModels()
	{
		//hpBar.fillAmount = fame / (float)maxHP;
		CardPlacementSystem.Instance.textHP_Player.text = "Слава: " + fame.ToString() +"\nЗащита: " + damageResistance;
	}

	private void Die()
	{
		audioSource.PlayOneShot(defeat);
		MenuManager.Instance.looseScreen.SetActive(true);
	}
}
