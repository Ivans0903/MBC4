using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivationPause : MonoBehaviour
{
    [Header("Active Status")]
    public bool ApplicationActive = true;

    [Header("Event Settings")]
    public UnityEvent StartEvents;
    public UnityEvent UpdateEvents;

    [Header("Pause Settings")]
    public UnityEvent PauseEvents;
    public UnityEvent ResumeEvents;
    bool CanTrigger;

    public void Pause()
    {
        CanTrigger = true;
        ApplicationActive = false;
        Time.timeScale = 0f;
        PauseEvents?.Invoke();
    }

    public void Play()
    {
        CanTrigger = true;
        ApplicationActive = true;
        Time.timeScale = 1f;
        ResumeEvents?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        CanTrigger = false;
        StartEvents?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEvents?.Invoke();
        if (!ApplicationActive && CanTrigger)
        {
            Pause();
            CanTrigger = false;
        }
        else if (ApplicationActive && CanTrigger)
        {
            Play();
            CanTrigger = false;
        }
    }
}
