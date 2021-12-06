using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Not prefer switching to scriptable object as each instance could be different in the data
/// i.e. hp, lv, atk, etc.
/// </summary>
public class CombatCharacterData : MonoBehaviour
{
    [Header("Character Combat Settings")]
    public float attack;
    public float defense;
    public float currentHealth;
    public float maxHealth;
    public Element.ElementType elementType;
}
