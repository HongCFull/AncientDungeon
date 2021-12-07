using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class KeyCodeSkillPair
{
    public KeyCode keyToTrigger;
    public ShirleySkill shirleySkill;
}

public class KeyCodeSkillUISlotPair
{
    public KeyCode keyToTrigger;
    public ShirleySkill shirleySkill;
}

public class ShirleySkillManager : MonoBehaviour
{
    // acceptable as the ability & slots aren't large in scale.
    // can improve it by using a serialized Dictionary
    [SerializeField] private List<KeyCodeSkillPair> keyCodeAbilityList;
    [SerializeField] private List<KeyCodeSkillUISlotPair> skillUISlotList;

    private void Start()
    {
        InitializeAbilityUI();
    }

    private void InitializeAbilityUI()
    {
        
    }
    
    //Can be better if don't need to do 2 linear search each time 
    public void PerformSkill(KeyCode triggeringKey)
    {
        foreach(KeyCodeSkillPair pair in keyCodeAbilityList) {
            if (pair.keyToTrigger == triggeringKey) {
                pair.shirleySkill.PerformSkill();
                break;
            }
        }
        
        // foreach(ShirleySkillUISlot slot in skillUISlotList) {
        //     if (slot.GetTriggerKeyCode() == triggeringKey) {
        //         break;
        //     }
        // }
    }
    
}
