using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State 
{
    private State parent;
    private activities TargetActivity;
    private Node target;
    private Node start;

    public State()
    {

    }
    public State(State parent,activities TargetActivity, Node start,Node target)
    {
        this.parent = parent;
        this.TargetActivity = TargetActivity;
        this.start = start;
        this.target = target;
    }

    public activities getActivity()
    {
        return TargetActivity;

    }

    public void setActivity(activities activity)
    {
        TargetActivity = activity;
    }
    
    public Node getTarget()
    {
        return target;
    }

    public Node getStart()
    {
        return start;
    }

    public void SetState(activities TargetActivity, Node start, Node target)
    {
        this.parent = null;
        this.TargetActivity = TargetActivity;
        this.start = start;
        this.target = target;
    }
}
