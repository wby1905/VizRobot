using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public Collider[] colliders;
    public LayerMask layerMask;

    float timer = 0f;
    public float coolDown = 2f;

    void Start()
    {
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            foreach (Collider c in colliders)
                c.enabled = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (1 << other.gameObject.layer == layerMask.value)
        {
            Debug.Log("collided" + other.gameObject.name + " " + gameObject.name);
            foreach (Collider c in colliders)
                c.enabled = false;
            timer = coolDown;
            Statistics.Task t = Statistics.Instance.GetCurrentTask();
            if (t != null)
                t.collisionCount++;
        }
    }

}
