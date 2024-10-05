using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInfo : MonoBehaviour
{
    public string Name;
    public int Damage;
    public int DamageResistance;
    public int Heal;

    public int Price;
    public int CardType;//1 - attack, 2 - attack buff, 3 - defence, 4 - defence buff
}
