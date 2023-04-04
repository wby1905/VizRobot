using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
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

    void FixedUpdate()
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
                startPos = objects[curTask].transform.position;
                startRot = objects[curTask].transform.rotation;
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
                objects[curTask].transform.position = startPos;
                objects[curTask].transform.rotation = startRot;

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
        startPos = objects[curTask].transform.position;
        startRot = objects[curTask].transform.rotation;

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
        float dis = Vector3.Distance(objects[curTask].transform.position, targets[curTask].transform.position);
        float rot = Quaternion.Angle(objects[curTask].transform.rotation, targets[curTask].transform.rotation);
        t.positionDiff = dis;
        t.angleDiff = rot;
    }


}
