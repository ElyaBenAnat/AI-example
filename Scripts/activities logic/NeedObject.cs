using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Need", menuName = "ScriptableObjects/NeedObject", order = 1)]
public class NeedObject :ScriptableObject
{
    public activities need;
    public int threshold;
    public int Status;
    private const int _bound= 100;

    public NeedObject()
    {
        
       

    }

    public void SetNeedObjectValues(activities need)
    {
        this.need = need;
        threshold = Random.Range(30, 80);
        Status = 90;
    }

    public activities getNeed()
    {
        return need;
    }

    public int getThreshold()
    {
        return threshold;
    }

    public int getStatus()
    {
        return Status;
    }

    public void setNeed(activities need)
    {
        this.need = need;
    }

    public void setThreshold(int Threshold)
    {
        this.threshold = Threshold;
    }

    public void setStatus(int Status)
    {
        this.Status = Status;
    }

    public void decrease()
    {
        if(Status>0)
        {
            Status = Status - 1;
        }
        
    }

    public bool getIfBarIsEmpty()
    {
        if(Status==0)
        {
            return true;
        }
        return false;
  
    }

    public bool getIfNeedUnderThreshold()
    {
        return Status < threshold;
    }

    public void increaseBy(int amount)
    {
        if(Status+amount<_bound)
        {
            Status = Status + amount;
        }
        else
        {
            Status = _bound;
        }
    }

    public void PrintNeed()
    {
        Debug.Log("activity: " + need + " stat:" + Status +" < "+threshold);
    }

    public string NeedToString()
    {
        return  need + " stat:" + Status + " < " + threshold;
    }
}
