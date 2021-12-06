using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    using Element;
    
    public static class CombatDamageManager 
    {
     
        public static void DealDamageTo(CombatCharacter attacker,CombatCharacter receiver,float skillPower)
        {
            if (attacker == null || receiver == null)
                return;
            
            float elementBuffFactor =
                ElementBuffFactor.GetElementalBuffFactor(attacker.GetElementType(), receiver.GetElementType());

            float damage = elementBuffFactor * skillPower * attacker.GetAttack() / receiver.GetDefense();
           
            //Debug.Log(attacker.name+" dealed "+damage+" damage to "+receiver.name);
            receiver.TakeDamageBy(damage);
        }
        
    }
    
}
