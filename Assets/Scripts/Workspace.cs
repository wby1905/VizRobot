using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UIElements;

public class Workspace : MonoBehaviour
{
    public GameObject robot;
    public GameObject pointPrefab;
    public float sampleStep;

    private List<Angle[]> sampledPoints;

    public List<ArticulationBody> joints;
    public int[][] jointRanges = {
        new int[] { -170, 170 },
        new int[] { -120, 120 },
        new int[] { -170, 170 },
        new int[] { -120, 120 },
        new int[] { -170, 170 },
        new int[] { -120, 120 },
        new int[] { -175, 175 },
     };

    void Awake()
    {
        // the first is base, the last is end effector (no rotation)
        joints = robot.GetComponentsInChildren<ArticulationBody>().ToList();
        sampledPoints = sampling();
        // sampledPoints = new List<Angle[]>();
        // sampledPoints.Add(new Angle[] { 0, 0, 0, 0, 0, 0, 0, 0 });
        // sampledPoints.Add(new Angle[] { 0, 90, 0, 0, 0, 0, 0, 0 });
        // sampledPoints.Add(new Angle[] { 0, 0, 90, 0, 0, 0, 0, 0 });
        // sampledPoints.Add(new Angle[] { 0, 0, 0, 90, 0, 0, 0, 0 });
        Debug.Log(sampledPoints.Count);

        drawWorkspace();

    }

    private List<Angle[]> sampling()
    {
        List<Angle[]> points = new List<Angle[]>();
        for (float j1 = jointRanges[0][0]; j1 <= jointRanges[0][1]; j1 += sampleStep)
            for (float j2 = jointRanges[1][0]; j2 <= jointRanges[1][1]; j2 += sampleStep)
                for (float j3 = jointRanges[2][0]; j3 <= jointRanges[2][1]; j3 += sampleStep)
                    for (float j4 = jointRanges[3][0]; j4 <= jointRanges[3][1]; j4 += sampleStep)
                    // for (float j5 = jointRanges[4][0]; j5 <= jointRanges[4][1]; j5 += sampleStep)
                    // for (float j6 = jointRanges[5][0]; j6 <= jointRanges[5][1]; j6 += sampleStep)
                    // for (float j7 = jointRanges[6][0]; j7 <= jointRanges[6][1]; j7 += sampleStep)
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

            Vector3 prevPos = joints[0].transform.position;
            Quaternion prevRot = joints[0].transform.rotation;

            for (int i = 1; i < joints.Count; i++)
            {
                prevRot *= Quaternion.AngleAxis(point[i - 1].value, prevRot * joints[i - 1].transform.up);
                Vector3 nextPos = prevPos + prevRot * joints[i].transform.localPosition;
                prevRot *= joints[i].transform.localRotation;
                // Debug.Log(prevRot.eulerAngles);
                prevPos = nextPos;
            }

            GameObject tmp = Instantiate(pointPrefab, prevPos, Quaternion.identity, p.transform);

        }
    }



    // Update is called once per frame
    void Update()
    {

    }
}
