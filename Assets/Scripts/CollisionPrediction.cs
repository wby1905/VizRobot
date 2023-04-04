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

    void Update()
    {
        var t = Statistics.Instance.GetCurrentTask();
        if (t == null || t.isVizEnabled)
        {
            m_collider.enabled = true;
            alarm.enabled = true;
        }
        else
        {
            m_collider.enabled = false;
            alarm.enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (1 << other.gameObject.layer == layerMask.value)
        {
            if (!entered)
                alarm.Play();
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
