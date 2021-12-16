using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Element;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private UnityEvent OnBattleBegin;
    [SerializeField] private UnityEvent OnBattleFinish;
    
    public static CombatManager Instance { get; private set; }
    private List<AICharacter> hatredAIEnemyList = new List<AICharacter>();
    
    private void Start()
    {
        if (!Instance){
            Instance = this;
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += ClearHatedEnemyList;
        }
        else {
            Destroy(this);
            return;
        }
    }
    
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ClearHatedEnemyList;
    }

    public void RegisterAsHatredEnemy(AICharacter aiCharacter)
    {
        if (hatredAIEnemyList.Count == 0)
        {
            //Debug.Log("OnbattleBegin");
            OnBattleBegin.Invoke();
        }
        hatredAIEnemyList.Add(aiCharacter);
    }

    public void UnregisterFromHatredEnemyList(AICharacter aiCharacter)
    {
        hatredAIEnemyList.Remove(aiCharacter);
        if (hatredAIEnemyList.Count == 0)
        {
            //Debug.Log("OnbattleFinish");
            OnBattleFinish.Invoke();
        }
    }

    public void DealDamageTo(CombatCharacter attacker,CombatCharacter receiver,float skillPower)
    {
        if (attacker == null || receiver == null)
            return;
        
        float elementBuffFactor =
            ElementBuffFactor.GetElementalBuffFactor(attacker.GetElementType(), receiver.GetElementType());

        float damage = elementBuffFactor * skillPower * attacker.GetAttack() / receiver.GetDefense();
       
        //Debug.Log(attacker.name+" dealed "+damage+" damage to "+receiver.name);
        receiver.TakeDamageBy(damage);
    }
    
    private void ClearHatedEnemyList(Scene current, LoadSceneMode mode) => hatredAIEnemyList.Clear();
}


