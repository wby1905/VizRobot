using System.Collections.Generic;
using System.Linq;
using RosMessageTypes.Std;
using UnityEngine;
using UnityEngine.UIElements;

public class VizTrajectory : MonoBehaviour
{

    public Material mat;
    public GameObject vizRobot;
    public Transform vizEndEffector;
    public Transform vizTargetPoint;
    public Trajectory traj;

    private float curLerp;
    private Vector3 prevTarget;

    private Queue<List<GameObject>> tmpRobots;
    private List<ArticulationBody> joints = new List<ArticulationBody>();
    private Queue<Vector3> targets = new Queue<Vector3>();

    private Angle[] angles;
    private Vector3[] initLocalPositions;
    private Quaternion[] initLocalRotations;
    public System.Random rand = new System.Random(0);
    private float percentile = 0.2f;
    private float curPercentile = 0.2f;


    void Awake()
    {
        joints = vizRobot.GetComponentsInChildren<ArticulationBody>().ToList();
        tmpRobots = new Queue<List<GameObject>>();
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

    public void ResetViz()
    {
        foreach (var tmpRobot in tmpRobots)
        {
            foreach (var trajectory in tmpRobot)
            {
                Destroy(trajectory);
            }
        }
        angles = new Angle[joints.Count];
        tmpRobots.Clear();
        targets.Clear();
    }

    void OnEnable()
    {

    }

    void OnDisable()
    {
        ResetViz();
    }

    // Update is called once per frame
    void Update()
    {
        if (targets.Count > 0 && tmpRobots.Count < 3)
        {
            bool reached = vizTargetPoint.GetComponent<Collider>().bounds.Contains(vizEndEffector.position);

            if (curLerp >= 1)
            {
                curPercentile = percentile;
                curLerp = 0;
                prevTarget = targets.Dequeue();
                tmpRobots.Enqueue(new List<GameObject>());
                NextViz(traj.sampledPoints[rand.Next(traj.sampledPoints.Count)]);
            }
            else if (reached)
            {
                curLerp += 4 * Time.deltaTime;
                vizTargetPoint.localPosition = Vector3.Lerp(prevTarget, targets.Peek(), curLerp);
                if (curLerp >= curPercentile)
                {
                    curPercentile += percentile;
                    renderTrail();
                }
            }
            else
            {
                Kinematics.InverseKinematics(joints, initLocalPositions, initLocalRotations, vizTargetPoint.position, angles);

                for (int i = joints.Count - 1; i > 0; i--)
                {
                    var drive = joints[i].xDrive;
                    drive.target = angles[i].value;
                    joints[i].xDrive = drive;
                }
            }
        }
    }

    public void NextViz(Vector3 target)
    {
        if (tmpRobots.Count == 0)
        {
            tmpRobots.Enqueue(new List<GameObject>());
        }

        if (targets.Count == 0)
        {
            vizTargetPoint.position = vizEndEffector.position;
            prevTarget = vizTargetPoint.localPosition;
        }

        targets.Enqueue(target);
    }

    public void StartViz()
    {
        ResetViz();
        NextViz(traj.sampledPoints[rand.Next(traj.sampledPoints.Count)]);
    }

    public void PopViz()
    {
        if (tmpRobots.Count <= 1)
        {
            return;
        }
        var tmp = tmpRobots.Dequeue();
        foreach (var trajectory in tmp)
        {
            Destroy(trajectory);
        }

        var first = tmpRobots.Peek();
        foreach (var tmpRobot in first)
        {
            tmpRobot.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0.1f);
        }
    }

    public void AlignRobot(List<ArticulationBody> realJoints)
    {
        ResetViz();
        for (int i = 0; i < joints.Count(); i++)
        {
            var xDrive = joints[i].xDrive;
            xDrive.target = realJoints[i].xDrive.target;
            joints[i].xDrive = xDrive;
        }
    }

    private void renderTrail()
    {
        for (int i = 0; i < joints.Count(); i++)
        {
            Transform joint = joints[i].transform;
            MeshFilter meshFilter = null;
            if (joint.childCount >= 2)
            {
                meshFilter = joint.GetChild(1).GetComponentInChildren<MeshFilter>();
            }
            if (meshFilter == null)
            {
                continue;
            }
            GameObject tmpRobot = new GameObject(joint.name + tmpRobots.Count + " " + curLerp, typeof(MeshRenderer), typeof(MeshFilter));
            tmpRobot.transform.parent = transform;
            tmpRobot.transform.SetPositionAndRotation(joint.position, joint.rotation);
            MeshRenderer mr = tmpRobot.GetComponent<MeshRenderer>();
            MeshFilter mf = tmpRobot.GetComponent<MeshFilter>();
            mf.mesh = meshFilter.mesh;
            mr.material = mat;
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            tmpRobots.Last().Add(tmpRobot);
        }
    }
}
