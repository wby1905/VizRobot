using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Trajectory : MonoBehaviour
{
    public List<Vector3> targets = new List<Vector3>();

    public GameObject robot;
    public List<ArticulationBody> joints = new List<ArticulationBody>();
    private Angle[] angles;
    private Vector3[] initLocalPositions;
    private Quaternion[] initLocalRotations;

    public Transform targetPoint;
    public Transform endEffector;


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

        joints = robot.GetComponentsInChildren<ArticulationBody>().ToList();
        angles = new Angle[joints.Count];
        initLocalPositions = new Vector3[joints.Count];
        initLocalRotations = new Quaternion[joints.Count];
        for (int i = 0; i < joints.Count; i++)
        {
            angles[i] = joints[i].xDrive.target;
            initLocalPositions[i] = joints[i].transform.localPosition;
            initLocalRotations[i] = joints[i].transform.localRotation;
        }

    }


    void Update()
    {
        if (isMoving)
        {
            bool hasReached = targetPoint.GetComponent<Collider>().bounds.Contains(endEffector.position);
            if (curLerp >= 1)
            {
                curLerp = 0;
                curTarget++;
                if (curTarget >= targets.Count)
                {
                    isMoving = false;
                    targetPoint.gameObject.SetActive(false);
                }
            }
            else if (hasReached)
            {
                curLerp += Time.deltaTime;
                targetPoint.position = Vector3.Lerp(targets[curTarget - 1], targets[curTarget], curLerp);
            }
            else
            {
                Kinematics.InverseKinematics(joints, initLocalPositions, initLocalRotations, targetPoint.position, angles);

                for (int i = joints.Count - 1; i > 0; i--)
                {
                    // joints[i].transform.localRotation = initLocalRotations[i] * Quaternion.AngleAxis(-angles[i].value, Vector3.up);

                    var drive = joints[i].xDrive;
                    drive.target = angles[i].value;
                    joints[i].xDrive = drive;
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
        targetPoint.gameObject.SetActive(true);
        targets[0] = targetPoint.position;
        targetPoint.position = targets[0];
        curTarget = 1;
        curLerp = 0;

    }
}
