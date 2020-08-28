using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasActivity 
{
    private string title;
    private List<string> logs;
    private int size;

    public CanvasActivity(string title,int size)
    {
        this.title = title;
        this.size = size;
        logs = new List<string>();
        for(int i=0;i<size;i++)
        {
            logs.Add("/");
        }
    }

    public void updateNewMassege(string s)
    {

        for (int i = 1; i < size; i++)
        {
            logs[i - 1] = logs[i];
        }
        logs[size - 1] = s;
    }

    public string getTitle()
    {
        return title;
    }

    public List<string> getMassages()
    {
        return logs;
    }
}
