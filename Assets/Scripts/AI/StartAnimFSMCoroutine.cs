using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimFSMCoroutine : MonoBehaviour
{
    private IEnumerator coroutineToExecute;
    private float delay;
    
    public StartAnimFSMCoroutine(IEnumerator coroutineToExecute, float delay)
    {
        this.coroutineToExecute = coroutineToExecute;
        StartCoroutine(ExecuteCoroutine());
    }

    public void StopAssignedCoroutine()
    {
        StopCoroutine(coroutineToExecute);
        coroutineToExecute = null;
    }
    
    IEnumerator ExecuteCoroutine()
    {
        if(coroutineToExecute== null)
            yield break;

        yield return new WaitForSeconds(delay);
        StartCoroutine(coroutineToExecute);

    }
    
}
