using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    private AICharacter aiCharacter;
    
    public abstract void Tick();
    public abstract void OnStateEnter();
    public abstract void OnStateExit();


    public State(AICharacter aiCharacter)
    {
        this.aiCharacter = aiCharacter;
    }
}
