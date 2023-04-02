using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionPrediction : MonoBehaviour
{
    private Collider m_collider;
    public LayerMask layerMask;

    private AudioSource alarm;
    private bool entered;

    void Start()
    {
        m_collider = GetComponent<Collider>();
        alarm = GetComponent<AudioSource>();
        entered = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (1 << other.gameObject.layer == layerMask.value)
        {
            if (!entered)
                alarm.Play();
            entered = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (1 << other.gameObject.layer == layerMask.value)
        {
            alarm.Stop();
            entered = false;
        }
    }
}
