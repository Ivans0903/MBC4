using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivationEvent : MonoBehaviour
{
    [Header("Main Settings")]
    public UnityEvent MainEvent;

    [Header("Event Settings")]
    public UnityEvent StartEvents;
    public UnityEvent UpdateEvents;

    // Start is called before the first frame update
    void Start()
    {
        StartEvents?.Invoke();
    }

    public void InvokeMainEvents()
    {
        MainEvent?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEvents?.Invoke();

    }

}
