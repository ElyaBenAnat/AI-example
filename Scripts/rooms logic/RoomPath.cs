using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPath 
{
    public bool visited = false;
    public GameObject northWall, southWall, eastWall, westWall, floor, wall;
    public int log_northWall = 0, log_southWall = 0, log_eastWall = 0, log_westWall = 0;
    int type;
    public RoomPath()
    {

    }
}
