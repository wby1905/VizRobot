using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Statistics : MonoBehaviour
{
    public static Statistics Instance { get; private set; }

    public class Task
    {
        public int index;
        public bool isVizEnabled;
        // public float avgCompletionTime;
        public float loss;
        public int collisionCount;

        public override string ToString()
        {
            return this.index + " " +
            this.isVizEnabled + " " +
            // MathF.Round(this.avgCompletionTime, 2) + " " +
            MathF.Round(this.loss, 2) + " " +
            this.collisionCount;
        }
    }

    List<Task> tasks = new List<Task>();
    Task currentTask;

    public TextMeshPro statText;

    public GameObject vizSystem;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // Update is called once per frame
    public void NewTask(int idx, bool isVizEnabled)
    {
        if (currentTask != null)
            Debug.Log(currentTask.ToString());
        currentTask = new Task();
        currentTask.index = idx;
        currentTask.isVizEnabled = isVizEnabled;
        tasks.Add(currentTask);

        vizSystem.SetActive(isVizEnabled);

    }

    public void StopTask()
    {
        currentTask = null;
        vizSystem.SetActive(true);
    }

    public Task GetCurrentTask()
    {
        return currentTask;
    }

    public int GetTaskCount()
    {
        return tasks.Count;
    }

    public string SerializeAllTasks()
    {
        string s = "Index VizEnabled Loss CollisionCount";
        foreach (Task t in tasks)
        {
            s += "\n" + t.ToString();
        }
        Debug.Log(s);
        return s;
    }

    public void SetStatText()
    {
        statText.text = SerializeAllTasks();
    }

}
