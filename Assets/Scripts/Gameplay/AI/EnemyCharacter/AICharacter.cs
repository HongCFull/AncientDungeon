using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using HitBoxDefinition;
[RequireComponent(typeof(Animator))]
public abstract class AICharacter : CombatCharacter
{
    [Space]
    [Header("Debug Settings")] 
    [SerializeField] private bool showAttackDistance;
    [SerializeField] private Color attackDistanceColor;
    
    [Space]
    [Header("Perception Settings")]
    [SerializeField] private AIVision vision;

    [Space]
    [Header("Attack Settings")] 
    [SerializeField] private float attackDistance;
    
    [Space]
    [Header("Mesh and effects")] 
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private float dissolveDuration;
    [SerializeField] private float dissolveDelay;

    //Mesh 
    private Material dissolveMaterial;
    
    public Vector3 initPosition { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        
        InitializeVariables();
        //DisableAllAttackHitBoxes();
    }

    void InitializeVariables()
    {
        initPosition = transform.position;
        
        dissolveMaterial = skinnedMeshRenderer.material;
    }
    
    public float GetAttackDistance()
    {
        return attackDistance;
    }

    public bool PlayerIsInsideAttackArea()
    {
        Vector3 projectedDistance = (PlayerCharacter.Instance.GetPlayerWorldPosition() - transform.position);
        float verticalDist = Mathf.Abs(projectedDistance.y);
        
        projectedDistance.y = 0f;
        return projectedDistance.magnitude  <= attackDistance && verticalDist<= attackDistance;        
    }
    

    public bool CanSeePlayer()
    {
        return vision.canSeePlayer;
    }
    public Vector3 GetAICharacterWorldPosition()
    {
        return transform.position;
    }
    
    
    public void WrappedOperationWhenItIsDead()
    {
        StartCoroutine(OperationOnDeath());
    }
    
    IEnumerator OperationOnDeath()
    {
        DisableAllReceiveHitBoxes();
        //SetAnimationTriggerIsDead();
        yield return StartCoroutine(PerformDissolveEffectWithDuration(dissolveDuration));
        Destroy(gameObject);
    }


    private void DisableAllReceiveHitBoxes()
    {
        foreach (ReceiveHitBox receiveHitBox in receiveHitBoxes) {
            receiveHitBox.EnableHitBox(false);
        }    
    }

    public IEnumerator PerformDissolveEffectWithDuration(float duration)
    {
        //yield return new WaitForSeconds(dissolveDelay);
        float timer = 0;
        int dissolveID = Shader.PropertyToID("Vector1_a7ed521b31bd49a3aff17f95fd88251b");   //Forced to use the ugly dissolve ID in the shader graph :O

        while (timer<duration) {
            timer += Time.deltaTime;
            dissolveMaterial.SetFloat(dissolveID,Mathf.Clamp01(timer/duration));
            yield return null;
        }
    }
    
    
    public IEnumerator PerformAppearEffectWithDuration(float duration)
    {
        float timer = 0;
        int dissolveID = Shader.PropertyToID("Vector1_a7ed521b31bd49a3aff17f95fd88251b");  //Forced to use the ugly dissolve ID in the shader graph :O

        while (timer<duration) {
            timer += Time.deltaTime;
            dissolveMaterial.SetFloat(dissolveID,1-Mathf.Clamp01(timer/duration));
            yield return null;
        }
    }
    
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (showAttackDistance) {
            Gizmos.color = attackDistanceColor;
            Gizmos.DrawWireSphere(transform.position,attackDistance);
        }
        
    }
#endif
    
}
