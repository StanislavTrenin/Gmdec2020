using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    [SerializeField] private float timeHandle;
    private double currentTimeHandle;
    private Action onLeftTimeHandler;
    private bool enableHandling;
    
    public void HandleButton(Action onLeftTimeHandler)
    {
        this.onLeftTimeHandler = onLeftTimeHandler;
        currentTimeHandle = Time.time;
        enableHandling = true;
    }

    private void FixedUpdate()
    {
        if (Time.time - currentTimeHandle > timeHandle && enableHandling == true && Input.GetMouseButton(0))
        {
            enableHandling = false;
            onLeftTimeHandler?.Invoke();
            onLeftTimeHandler = null;
        }
        if (Input.GetMouseButtonUp(0))
        {
            enableHandling = false;
            onLeftTimeHandler = null;
        }
    }
    
}
