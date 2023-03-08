using System.Collections;
using System.Collections.Generic;
using BioIK;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    public List<Vector3> targets = new List<Vector3>();

    public GameObject targetPoint;
    public GameObject endEffector;


    private bool isMoving;
    private int curTarget;
    private float curLerp;

    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;
        curTarget = 0;
        curLerp = 0;
        targets.Insert(0, Vector3.zero);

    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            if (curLerp < 1)
            {
                curLerp += Time.deltaTime;
                targetPoint.transform.position = Vector3.Lerp(targets[curTarget - 1], targets[curTarget], curLerp);
            }
            else
            {
                curLerp = 0;
                curTarget++;
                if (curTarget >= targets.Count)
                {
                    isMoving = false;
                    targetPoint.SetActive(false);
                }
            }
        }
    }

    public void visualizeTrajectory()
    {
        if (isMoving || targets.Count < 2 || targetPoint == null)
        {
            return;
        }

        isMoving = true;
        targetPoint.SetActive(true);
        Debug.Log(endEffector.transform.TransformPoint(endEffector.transform.position));
        targets[0] = endEffector.transform.TransformPoint(endEffector.transform.position);
        targetPoint.transform.position = targets[0];
        curTarget = 1;
        curLerp = 0;
    }
}
