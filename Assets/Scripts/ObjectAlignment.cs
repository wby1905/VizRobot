using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using TreeEditor;
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

    private Vector3 startPos;
    private Quaternion startRot;

    private float timer;
    public float timeLimit;

    Statistics statistics;

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
    }

    void FixedUpdate()
    {
        if (curTask >= 0)
        {

            if (timer > 0)
            {
                timer -= Time.deltaTime;
                countdownText.text = ((int)timer).ToString() + "s";
            }
            else
            {
                timer = 0;
                Statistics.Task t = statistics.GetCurrentTask();
                SetScore(t);

                // Reset
                tasks[curTask].SetActive(false);
                objects[curTask].transform.position = startPos;
                objects[curTask].transform.rotation = startRot;

                curTask++;
                if (curTask < objects.Length)
                {
                    startPos = objects[curTask].transform.position;
                    startRot = objects[curTask].transform.rotation;
                    timer = timeLimit;
                    tasks[curTask].SetActive(true);
                    statistics.NewTask(t.index, !t.isVizEnabled);
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
        startPos = objects[curTask].transform.position;
        startRot = objects[curTask].transform.rotation;

        int count = statistics.GetTaskCount();
        if (count % 2 == 0)
            statistics.NewTask(count / objects.Length, true);
        else
            statistics.NewTask(count / objects.Length, false);

        countdownText.transform.parent.gameObject.SetActive(true);
        toggle.enabled = false;

        trajectory.StartRobot();

    }

    void SetScore(Statistics.Task t)
    {
        float dis = Vector3.Distance(objects[curTask].transform.position, targets[curTask].transform.position);
        float rot = Quaternion.Angle(objects[curTask].transform.rotation, targets[curTask].transform.rotation);
        t.positionDiff = dis;
        t.angleDiff = rot;
    }


}
