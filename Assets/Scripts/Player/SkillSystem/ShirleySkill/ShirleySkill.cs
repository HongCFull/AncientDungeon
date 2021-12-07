using System;
using System.Collections;
using System.Collections.Generic;
using Element;
using Player;
using UnityEngine;

public abstract class ShirleySkill : MonoBehaviour
{
    public string stateNameInAnimator;
    public ElementType elementType;
    public float coolDownInSec;
    public float skillPower;
    [HideInInspector] public bool canPerformSkill = true;
    
    protected int hashedAnimStateID;
    protected float coolDownTimer;    //How much time still need to wait for to perform the skill again  
    
    private void Awake()
    {
        hashedAnimStateID = Animator.StringToHash(stateNameInAnimator);
        coolDownTimer = coolDownInSec;
    }
    
    private void Update()
    {
        if (!canPerformSkill) {
            coolDownTimer -= Time.deltaTime;
            if (coolDownTimer <=0 ) {
                coolDownTimer = coolDownInSec;
                canPerformSkill = true;
            }
        }
    }

    public abstract void PerformSkill();
}
