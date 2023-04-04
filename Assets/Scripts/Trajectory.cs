using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Trajectory : MonoBehaviour
{
    public List<Vector3> sampledPoints = new List<Vector3>();

    public GameObject robot;
    public List<ArticulationBody> joints = new List<ArticulationBody>();
    private Angle[] angles;
    private Vector3[] initLocalPositions;
    private Quaternion[] initLocalRotations;

    public Transform targetPoint;
    public Transform endEffector;

    public VizTrajectory trail;


    private bool isMoving;
    private Vector3 prevTarget, curTarget, nextTarget;
    private float curLerp;

    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;
        curLerp = 0;

        joints = robot.GetComponentsInChildren<ArticulationBody>().ToList();
        angles = new Angle[joints.Count];
        initLocalPositions = new Vector3[joints.Count];
        initLocalRotations = new Quaternion[joints.Count];

        initLocalPositions[0] = joints[0].transform.position;
        initLocalRotations[0] = joints[0].transform.rotation;
        for (int i = 1; i < joints.Count; i++)
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
                prevTarget = curTarget;
                curTarget = nextTarget;
                nextTarget = sampledPoints[Random.Range(0, sampledPoints.Count)];
                if (trail.gameObject.activeInHierarchy)
                    trail.NextViz(nextTarget);
            }
            else if (hasReached)
            {
                curLerp += Time.deltaTime;
                targetPoint.localPosition = Vector3.Lerp(prevTarget, curTarget, curLerp);
            }
            else
            {
                Kinematics.InverseKinematics(joints, initLocalPositions, initLocalRotations, targetPoint.position, angles);

                for (int i = joints.Count - 1; i > 0; i--)
                {
                    var drive = joints[i].xDrive;
                    drive.target = angles[i].value;
                    joints[i].xDrive = drive;
                }
            }
        }



    }

    public void StartRobot()
    {
        if (isMoving || sampledPoints.Count < 2 || targetPoint == null)
        {
            return;
        }
        isMoving = true;
        targetPoint.gameObject.SetActive(true);
        targetPoint.position = endEffector.position;
        curTarget = targetPoint.localPosition;
        prevTarget = curTarget;
        curLerp = 1;

        if (trail.gameObject.activeInHierarchy)
            trail.AlignRobot(joints);

        // Make sure the movement is deterministic
        Random.InitState(0);
        nextTarget = sampledPoints[Random.Range(0, sampledPoints.Count)];
    }

    public void StopRobot()
    {
        isMoving = false;
        targetPoint.gameObject.SetActive(false);
        trail.ResetViz();
        ResetRobot();
    }

    public void ResetRobot()
    {
        for (int i = 0; i < joints.Count; i++)
        {
            var drive = joints[i].xDrive;
            drive.target = 0;
            joints[i].xDrive = drive;
        }
        angles = new Angle[joints.Count];
    }
}
