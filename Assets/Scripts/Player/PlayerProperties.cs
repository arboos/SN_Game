using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProperties : MonoBehaviour
{
    [Header("Характеристики")]
    public int HP;
    public int maxHP;
    public float damageResistance;
    public List<GameObject> currentDeckBuild;
    public int fame;
    [Header("UI/UX")]
    [SerializeField] private Image hpBar;
 
    public void TakeDamage(int damage)
    {
        if (HP - (int)(damage * damageResistance) <= 0)
        {
            Die();
            return;
        }
        if (damageResistance != 0)
        {
            HP -= (int)(damage * damageResistance);
        }
        else
        {
            HP -= damage;
        }
        UpdateViewModels();
    }

    public void Heal(int heal)
    {
        HP = Mathf.Clamp(HP + heal,0,maxHP);
        UpdateViewModels();
    }

    public void SetResistance(float resistance)
    {
        damageResistance = 1 - resistance;
    }

    private void UpdateViewModels()
    {
        hpBar.fillAmount = HP / (float)maxHP;
    }

    private void Die()
    {
        //Обработка смерти
    }
}
