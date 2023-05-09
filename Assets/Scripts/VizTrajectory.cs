using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class VizTrajectory : MonoBehaviour
{

    public Material mat;
    public GameObject vizRobot;
    public Transform vizEndEffector;
    public Transform vizTargetPoint;
    public Trajectory traj;
    public int trailNum = 2;
    public TextMeshPro trailNumText;
    public TextMeshPro percentileText;
    public GameObject pointPrefab;

    private float curLerp;
    private Vector3 prevTarget;

    private Queue<List<GameObject>> tmpRobots;
    private List<ArticulationBody> joints = new List<ArticulationBody>();
    private Queue<Vector3> targets = new Queue<Vector3>();

    private Angle[] angles;
    private Vector3[] initLocalPositions;
    private Quaternion[] initLocalRotations;
    public System.Random rand = new System.Random(0);
    public float percentile = 0.2f;
    private float curPercentile = 0.2f;
    private bool isFirst = true;
    private TrailStyle trailStyle = TrailStyle.Full;

    private int blinkIdx = 0;
    private float blinkTime = 0;
    public float blinkInterval = 0.05f;

    public Color highlightColor = new Color(1, 0, 0, 0.5f);

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

        for (int i = 0; i < joints.Count; i++)
        {
            var drive = joints[i].xDrive;
            drive.target = 0;
            joints[i].xDrive = drive;
            joints[i].transform.localPosition = initLocalPositions[i];
            joints[i].transform.localRotation = initLocalRotations[i];
        }
        joints[0].transform.position = initLocalPositions[0];
        joints[0].transform.rotation = initLocalRotations[0];

        isFirst = true;
    }


    void OnDisable()
    {
        ResetViz();
    }

    // Update is called once per frame
    void Update()
    {
        trailNumText.text = trailNum.ToString();
        percentileText.text = (percentile * 100).ToString() + "%";

        if (targets.Count > 0 && tmpRobots.Count <= trailNum)
        {
            bool reached = vizTargetPoint.GetComponent<Collider>().bounds.Contains(vizEndEffector.position);

            if (curLerp >= 1)
            {
                curPercentile = percentile;
                curLerp = 0;
                prevTarget = targets.Dequeue();
                tmpRobots.Enqueue(new List<GameObject>());
                int nextIdx = rand.Next(traj.sampledPoints.Count);
                while (nextIdx == traj.sampledPoints.IndexOf(prevTarget))
                {
                    nextIdx = rand.Next(traj.sampledPoints.Count);
                }
                NextViz(traj.sampledPoints[nextIdx]);
            }
            else if (reached)
            {
                curLerp += 8 * Time.deltaTime;
                vizTargetPoint.localPosition = Vector3.Lerp(prevTarget, targets.Peek(), curLerp);
                if (curLerp >= curPercentile || curLerp >= 1)
                {
                    curPercentile += percentile;
                    renderTrail();
                }
            }
            else
            {
                Kinematics.InverseKinematics(joints, initLocalPositions, initLocalRotations, vizTargetPoint.position, angles, 200);

                for (int i = joints.Count - 1; i > 0; i--)
                {
                    var drive = joints[i].xDrive;
                    drive.target = angles[i].value;
                    joints[i].xDrive = drive;
                }
            }
        }


        // The first trail blink
        if (tmpRobots.Count > 0 && blinkTime < blinkInterval)
        {
            blinkTime += Time.deltaTime;
        }
        else
        {
            blinkTime = 0;
            if (tmpRobots.Count > 0)
            {
                var tmp = tmpRobots.Peek();
                if (tmp.Count > 0 && blinkIdx < tmp.Count)
                {

                    foreach (var i in tmp[blinkIdx].GetComponentsInChildren<Renderer>())
                    {
                        i.material = mat;
                    }
                    blinkIdx = (blinkIdx + 1) % tmp.Count;
                    foreach (var i in tmp[blinkIdx].GetComponentsInChildren<Renderer>())
                    {
                        i.material.color = highlightColor;
                    }
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
        if (tmpRobots.Count <= trailNum)
        {
            return;
        }

        var tmp = tmpRobots.Dequeue();
        foreach (var trajectory in tmp)
        {
            Destroy(trajectory);
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
        if (tmpRobots.Count == 0)
        {
            return;
        }
        GameObject trail = new GameObject("Trail " + tmpRobots.Count + " " + curLerp);
        trail.transform.parent = transform;
        trail.transform.position = vizEndEffector.position;
        tmpRobots.Last().Add(trail);

        if ((trailStyle & TrailStyle.Full) == TrailStyle.Full)
        {
            GameObject tmpRobot = new GameObject("Robot");
            tmpRobot.transform.parent = trail.transform;
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
                GameObject tmpJoint = new GameObject(joint.name, typeof(MeshRenderer), typeof(MeshFilter));
                tmpJoint.transform.parent = tmpRobot.transform;
                tmpJoint.transform.SetPositionAndRotation(joint.position, joint.rotation);
                // TODO: This is a temporary solution to rescale the end effector (the knife)
                if (i == joints.Count() - 1)
                {
                    tmpJoint.transform.localScale = 0.0001f * Vector3.one;
                }

                MeshRenderer mr = tmpJoint.GetComponent<MeshRenderer>();
                MeshFilter mf = tmpJoint.GetComponent<MeshFilter>();
                mf.mesh = meshFilter.mesh;
                mr.material = mat;
                mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }
        }
        else if ((trailStyle & TrailStyle.Partial) == TrailStyle.Partial)
        {
            GameObject tmpJoint = new GameObject("Joint", typeof(MeshRenderer), typeof(MeshFilter));
            tmpJoint.transform.parent = trail.transform;
            tmpJoint.transform.SetPositionAndRotation(vizEndEffector.position, vizEndEffector.rotation);
            MeshRenderer mr = tmpJoint.GetComponent<MeshRenderer>();
            MeshFilter mf = tmpJoint.GetComponent<MeshFilter>();
            mf.mesh = vizEndEffector.GetComponentInChildren<MeshFilter>().mesh;
            mr.material = mat;
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            // TODO: This is a temporary solution to rescale the end effector (the knife)
            tmpJoint.transform.localScale = 0.0001f * Vector3.one;
        }



        if ((trailStyle & TrailStyle.Point) == TrailStyle.Point)
        {
            GameObject tmpPoint = Instantiate(pointPrefab, trail.transform);
            tmpPoint.transform.position = vizEndEffector.position;
            tmpPoint.name = "Point";
            tmpPoint.GetComponent<MeshRenderer>().material = mat;
        }
        if ((trailStyle & TrailStyle.Line) == TrailStyle.Line)
        {
            GameObject tmpLine = new GameObject("Line", typeof(LineRenderer));
            tmpLine.transform.parent = trail.transform;
            tmpLine.transform.position = vizEndEffector.position;
            LineRenderer lr = tmpLine.GetComponent<LineRenderer>();
            lr.material = mat;
            lr.startWidth = 0.01f;
            lr.endWidth = 0.01f;
            if (tmpRobots.Count > 0 && tmpRobots.Last().Count > 1)
            {
                lr.SetPosition(0, tmpRobots.Last()[tmpRobots.Last().Count - 2].transform.position);
                lr.SetPosition(1, vizEndEffector.position);
            }
        }
        var first = tmpRobots.Peek();
        foreach (var tmpRobot in first)
        {
            foreach (var renderer in tmpRobot.GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = highlightColor;
            }
        }
    }

    public void IncTrailNum()
    {
        if (tmpRobots.Count > 0)
        {
            return;
        }
        trailNum++;
        trailNum = Mathf.Clamp(trailNum, 0, 10);
    }

    public void DecTrailNum()
    {
        if (tmpRobots.Count > 0)
        {
            return;
        }
        trailNum--;
        trailNum = Mathf.Clamp(trailNum, 0, 10);
    }

    public void IncPercentile()
    {
        if (tmpRobots.Count > 0)
        {
            return;
        }
        percentile += 0.1f;
        percentile = Mathf.Clamp(percentile, 0, 0.5f);
    }

    public void DecPercentile()
    {
        if (tmpRobots.Count > 0)
        {
            return;
        }
        percentile -= 0.1f;
        percentile = Mathf.Clamp(percentile, 0, 0.5f);
    }



    public void ToggleTrailStyleFull()
    {
        trailStyle ^= TrailStyle.Full;
    }

    public void ToggleTrailStylePartial()
    {
        trailStyle ^= TrailStyle.Partial;
    }

    public void ToggleTrailStyleLine()
    {
        trailStyle ^= TrailStyle.Line;
    }

    public void ToggleTrailStylePoint()
    {
        trailStyle ^= TrailStyle.Point;
    }

    public void ClearTrailStyle()
    {
        trailStyle = TrailStyle.None;
    }
}

[Flags]
enum TrailStyle
{
    None = 0,
    Full = 1,
    Partial = 2,
    Line = 4,
    Point = 8,

}