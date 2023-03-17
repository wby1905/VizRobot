using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Workspace : MonoBehaviour
{
    public GameObject robot;
    public GameObject pointPrefab;
    public float sampleStep;

    private List<Angle[]> sampledPoints;

    public List<ArticulationBody> joints;

    void Start()
    {
        // the first is base, the last is end effector (no rotation)
        joints = robot.GetComponentsInChildren<ArticulationBody>().ToList();
        // sampledPoints = sampling();
        sampledPoints = new List<Angle[]>();
        // sampledPoints.Add(new Angle[] { 0, 0, 0, 0, 0, 0, 0, 0 });
        // sampledPoints.Add(new Angle[] { 0, 90, 0, 0, 0, 0, 0, 0 });
        // sampledPoints.Add(new Angle[] { 0, 0, 90, 0, 90, 0, 0, 0 });
        sampledPoints.Add(new Angle[] { 0, 4.2f, -11, 1.168f, -55, 0.35f, 18.9f, 0, 0 });

        Debug.Log(sampledPoints.Count);

        drawWorkspace();

    }

    private List<Angle[]> sampling()
    {
        List<Angle[]> points = new List<Angle[]>();
        for (float j1 = joints[1].xDrive.lowerLimit; j1 <= joints[1].xDrive.upperLimit; j1 += sampleStep)
            for (float j2 = joints[2].xDrive.lowerLimit; j2 <= joints[2].xDrive.upperLimit; j2 += sampleStep)
                for (float j3 = joints[3].xDrive.lowerLimit; j3 <= joints[3].xDrive.upperLimit; j3 += sampleStep)
                    for (float j4 = joints[4].xDrive.lowerLimit; j4 <= joints[4].xDrive.upperLimit; j4 += sampleStep)
                    {
                        points.Add(new Angle[] { new Angle(0), new Angle(j1), new Angle(j2), new Angle(j3), new Angle(j4), new Angle(0), new Angle(0), new Angle(0) });
                    }
        return points;
    }


    private void drawWorkspace()
    {
        int cnt = 0;

        foreach (var point in sampledPoints)
        {
            GameObject p = new GameObject("point" + cnt++ + " " + point[1].value + " " + point[2].value + " " + point[3].value + " " + point[4].value);
            p.transform.parent = transform;

            Vector3 eePos = Kinematics.ForwardKinematics(joints, point);

            GameObject tmp = Instantiate(pointPrefab, eePos, Quaternion.identity, p.transform);

        }
    }

}
