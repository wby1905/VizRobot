using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixKnife : MonoBehaviour
{
    private GameObject currentKnife;
    public float loss;
    public float lossThreshold = 0.03f;

    void OnEnable()
    {
        loss = -1;
        // if (currentKnife)
        // {
        //     currentKnife.GetComponent<Rigidbody>().isKinematic = false;
        //     currentKnife.GetComponent<Rigidbody>().useGravity = true;
        // }
        transform.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);
    }

    void Update()
    {

        if (currentKnife)
        {
            loss = ObjectAlignment.GetLoss(currentKnife.transform, transform);
            if (loss < lossThreshold)
            {
                currentKnife.GetComponent<Rigidbody>().isKinematic = true;
                currentKnife.GetComponent<Rigidbody>().useGravity = false;
                transform.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.5f);
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Knife")
        {
            currentKnife = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Knife")
        {
            currentKnife = null;
            loss = -1;
        }
    }
}
