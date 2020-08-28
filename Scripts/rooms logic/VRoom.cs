using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum house{
    available_floor, unavailable_floor ,wall,floor,target,start
}

public enum activities
{
    hunger,energy,fun,hygiene,bladder,social
}

public enum color
{
   start,target,white,gray,black,visited,gray_target,white_target
}
public class VRoom 
{
    private house[,] pathMap;
    private activities[,] activitiesMap;
    public VRoom (int size )
    {
        pathMap = new house[size, size];
        activitiesMap = new activities[size, size];

    }

    public void setpathMap(int row, int col,house value)
    {
        pathMap[row, col] = value;
    }

    public house  getpathMapValue(int row, int col)
    {
        return pathMap[row, col];
    }

    public void setactivitiesMap(int row, int col,activities value)
    {
        activitiesMap[row, col] = value;
    }

    public activities  getactivitiesMapValue(int row, int col)
    {
        return activitiesMap[row,col];
    }
}
