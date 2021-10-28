using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CombatCharacter : MonoBehaviour
{
    
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    
    [Tooltip("when it is damaged and survived afterward")]
    [SerializeField] private UnityEvent whenItIsDamaged;
    
    [SerializeField] private UnityEvent whenItIsDead;
    public bool IsDead() => currentHealth <= 0;

    
    protected virtual void Awake()
    {
        //Debug.Log("Damageable::Awake");
        currentHealth =Mathf.Clamp(currentHealth,0,maxHealth);
    }
    
    /// <summary>
    /// Deal damage to this damageable gameObject.
    /// Invoke events when it is damaged or dead.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamageBy(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            whenItIsDead.Invoke();
        }
        else {
            whenItIsDamaged.Invoke();
        }
    }
}
