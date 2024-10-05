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
    public int CardType;//1 - defence, 2 - defence buff, 3 - attack, 4 - attack buff
}
