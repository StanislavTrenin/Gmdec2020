using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    [SerializeField] private float timeHandle;
    private Action onLeftTimeHandler;

    public void HandleButton(Action onLeftTimeHandler)
    {
        this.onLeftTimeHandler = onLeftTimeHandler;
        StopAllCoroutines();
        StartCoroutine(StartHandle());
    }

    private IEnumerator StartHandle()
    {
        yield return new WaitForSeconds(timeHandle);
        if(Input.GetMouseButton(1))
            onLeftTimeHandler?.Invoke();
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0))
        {
            StopAllCoroutines();
        }
    }
    
}
