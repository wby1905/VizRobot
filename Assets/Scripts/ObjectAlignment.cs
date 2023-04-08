using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;

public class ObjectAlignment : MonoBehaviour
{


    public TextMeshPro countdownText;
    public Interactable toggle;
    public Trajectory trajectory;

    private int curTask;

    public GameObject[] tasks;
    private GameObject[] objects;
    private GameObject[] targets;

    private Vector3[] startPos;
    private Quaternion[] startRot;

    private float timer;
    public float timeLimit;

    Statistics statistics;

    private float waitTimer;
    private Statistics.Task t;

    // Start is called before the first frame update
    void Start()
    {
        objects = new GameObject[tasks.Length];
        targets = new GameObject[tasks.Length];

        for (int i = 0; i < tasks.Length; i++)
        {
            targets[i] = tasks[i].transform.GetChild(0).gameObject;
            objects[i] = tasks[i].transform.GetChild(1).gameObject;
        }
        curTask = -1;
        countdownText.transform.parent.gameObject.SetActive(false);
        statistics = Statistics.Instance;

        waitTimer = -1000;
    }

    void Update()
    {


        if (curTask >= 0)
        {
            if (waitTimer > 0)
            {
                waitTimer -= Time.deltaTime;
                countdownText.text = waitTimer.ToString("0.0") + "s";
                return;
            }
            else if (waitTimer != -1000)
            {
                startPos = new Vector3[objects[curTask].transform.childCount];
                startRot = new Quaternion[objects[curTask].transform.childCount];
                for (int i = 0; i < objects[curTask].transform.childCount; i++)
                {
                    var child = objects[curTask].transform.GetChild(i);
                    startPos[i] = child.position;
                    startRot[i] = child.rotation;
                }

                timer = timeLimit;
                tasks[curTask].SetActive(true);
                trajectory.StartRobot();
                statistics.NewTask(t.index, !t.isVizEnabled);
                waitTimer = -1000;
            }
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                countdownText.text = timer.ToString("0.0") + "s";
            }
            else
            {
                timer = 0;
                t = statistics.GetCurrentTask();
                SetScore(t);

                // Reset
                tasks[curTask].SetActive(false);
                for (int i = 0; i < objects[curTask].transform.childCount; i++)
                {
                    var child = objects[curTask].transform.GetChild(i);
                    child.position = startPos[i];
                    child.rotation = startRot[i];
                }


                curTask++;
                if (curTask < objects.Length)
                {
                    statistics.StopTask();
                    trajectory.StopRobot();
                    waitTimer = 5;
                }
                else
                {
                    countdownText.transform.parent.gameObject.SetActive(false);
                    curTask = -1;
                    toggle.enabled = true;
                    statistics.StopTask();
                    trajectory.StopRobot();
                }
            }
        }
    }

    public void StartTest()
    {
        curTask = 0;
        timer = timeLimit;
        tasks[curTask].SetActive(true);
        startPos = new Vector3[objects[curTask].transform.childCount];
        startRot = new Quaternion[objects[curTask].transform.childCount];

        for (int i = 0; i < objects[curTask].transform.childCount; i++)
        {
            var child = objects[curTask].transform.GetChild(i);
            startPos[i] = child.position;
            startRot[i] = child.rotation;
        }
        int count = statistics.GetTaskCount() / objects.Length;
        if (count % 2 == 0)
            statistics.NewTask(count, true);
        else
            statistics.NewTask(count, false);


        countdownText.transform.parent.gameObject.SetActive(true);
        toggle.enabled = false;

        trajectory.StartRobot();

    }

    void SetScore(Statistics.Task t)
    {
        float loss = 0;
        HashSet<int> used = new HashSet<int>();
        for (int i = 0; i < objects[curTask].transform.childCount; i++)
        {
            var child = objects[curTask].transform.GetChild(i);
            float minLoss = 1000;
            int minIndex = -1;
            for (int j = 0; j < targets[curTask].transform.childCount; j++)
            {
                var target = targets[curTask].transform.GetChild(j);
                float l = 1 - Vector3.Dot((-child.forward).normalized, target.up.normalized);
                // object point to target line's distance
                var p = child.position - target.position;
                var d = Vector3.Project(p, target.up);
                l += Mathf.Sqrt(p.sqrMagnitude - d.sqrMagnitude);
                if (l < minLoss && !used.Contains(j))
                {
                    minLoss = l;
                    minIndex = j;
                }
            }
            loss += minLoss;
            used.Add(minIndex);
            Debug.Log("Object " + i + " to target " + minIndex + " loss: " + minLoss);
        }

        t.loss = loss;
    }


}
