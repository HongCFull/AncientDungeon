using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimFSMCoroutine : MonoBehaviour
{
    private IEnumerator coroutineToExecute;


    public StartAnimFSMCoroutine(IEnumerator coroutineToExecute)
    {
        if (coroutineToExecute != null)
            StartCoroutine(coroutineToExecute);
    }
    
    public StartAnimFSMCoroutine(IEnumerator coroutineToExecute, float delay)
    {
        this.coroutineToExecute = coroutineToExecute;
        StartCoroutine(ExecuteCoroutineWithDelay(delay));
    }

    public void StopExecutedCoroutine()
    {
        StopCoroutine(coroutineToExecute);
        coroutineToExecute = null;
    }
    
    IEnumerator ExecuteCoroutineWithDelay(float delay)
    {
        if(coroutineToExecute== null)
            yield break;

        yield return new WaitForSeconds(delay);
        StartCoroutine(coroutineToExecute);

    }
    
}
