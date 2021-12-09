using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TeleportToRandPositionState : StateMachineBehaviour
{
    [SerializeField] private float randWanderRadius;
    private NavMeshAgent navMeshAgent;
    private AICharacter aiCharacter;
    private TeleportAIWithEffectsHelper tpAIHelper;
    private int animIDTeleportFinished;
    private bool hasEnter = false;
    private bool hasExit = false;
    //private bool teleported = false;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animIDTeleportFinished = Animator.StringToHash("teleportFinished");
        navMeshAgent = animator.GetComponent<NavMeshAgent>();
        aiCharacter = animator.GetComponent<AICharacter>();
        
        aiCharacter.SetCombatCharacterToInvulnerable(true);

        hasEnter = true;
        hasExit = false;
        
        TeleportAI();

    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateMove(animator, stateInfo, layerIndex);
        if (!hasEnter || hasExit)
            return;

        if (tpAIHelper !=null && tpAIHelper.operationFinished) {
            animator.SetTrigger(animIDTeleportFinished);
        }
        // if (!teleported) {
        //     teleported = true;
        //     TeleportAITo(GetRandomPosition());
        //     animator.SetTrigger(animIDTeleportFinished);
        // }
        
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        hasEnter = false;
        hasExit = true;
        
        aiCharacter.SetCombatCharacterToInvulnerable(false);
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 randomDistanceVec = Random.insideUnitSphere * randWanderRadius;
        Vector3 targetPos = navMeshAgent.gameObject.transform.position + randomDistanceVec;
        NavMeshHit hit;
        NavMesh.SamplePosition(targetPos, out hit,randWanderRadius,1);
        return hit.position;
    }

    private void TeleportAI()
    {
        GameObject tpAIHelperObj = new GameObject();
        TeleportAIWithEffectsHelper tpAIHelper =tpAIHelperObj.AddComponent<TeleportAIWithEffectsHelper>();
        this.tpAIHelper = tpAIHelper;
        tpAIHelper._navMeshAgent = navMeshAgent;
        tpAIHelper._aiCharacter = aiCharacter;
        tpAIHelper.tpPosition = GetRandomPosition();
        tpAIHelper.StartTeleportAIWithEffects();
    }
    

}

public class TeleportAIWithEffectsHelper: MonoBehaviour
{
    public bool operationFinished { get; private set; } = false;
    public NavMeshAgent _navMeshAgent;
    public AICharacter _aiCharacter;
    public Vector3 tpPosition;
    
    public void StartTeleportAIWithEffects()
    {
        StartCoroutine(StartOperation());
    }

    IEnumerator StartOperation()
    {
        yield return StartCoroutine(_aiCharacter.PerformDissolveEffectWithDuration(0.2f));
        _navMeshAgent.Warp(tpPosition);
        yield return StartCoroutine(_aiCharacter.PerformAppearEffectWithDuration(0.2f));
        operationFinished = true;
    }
}