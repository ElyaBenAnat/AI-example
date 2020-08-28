using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Node : IComparable<Node>
{

    Node Target;
    Node parent;
    int x;
    int y;
    int G = 0;
    color c;
    house pathKind;

    public Node(int x,int y)
    {
        this.x = x;
        this.y = y;
    }
    public Node(int x,int y,color c,house pk)
    {
        this.x = x;
        this.y = y;
        this.c = c;
        pathKind = pk;
    }
    int IComparable<Node>.CompareTo(Node other)
    {
        //euclidien distance
        double deltaX1;
        double deltaY1;
        double deltaX2;
        double deltaY2;
        if (this.GetTarget() is null)
        {
            deltaX1 = 0;
            deltaY1 = 0;
        }
        else
        {
            deltaX1 = (double)(this.getX() - this.GetTarget().getX());
            deltaY1 = (double)(this.getY() - this.GetTarget().getY());
        }

        if (other.GetTarget() is null)
        {
            deltaX2 = 0;
            deltaY2 = 0;
        }
        else
        {
            deltaX2 = (double)(other.getX() - other.GetTarget().getX());
            deltaY2 = (double)(other.getY() - other.GetTarget().getY());
        }


        double dist1 = Math.Sqrt(deltaX1 * deltaX1 + deltaY1 * deltaY1) +this.G;
        double dist2 = Math.Sqrt(deltaX2 * deltaX2 + deltaY2 * deltaY2) +other.GetG();

        int res = 0;
        if(dist1 > dist2)
        {
            res = 1;
        }
        
        return res;
    }

    public void SetG(int val)
    {
        this.G = val;
    }

    public int GetG()
    {
        return G;
    }

    public void setParent(Node parent)
    {
        this.parent = parent;
    }

    public Node  getParent()
    {
        return parent;
    }

    public void setTarget(Node target)
    {
        this.Target = target;
    }
    public Node GetTarget()
    {
        return Target;
    }

    public int  getX()
    {
        return x;
    }

    public int getY()
    {
        return y;
    }

    public color getColor()
    {
        return c;
    }
    public void setColor(color c)
    {
        this.c = c;
    }

    public double GetUclidienDistance(Node n)
    {
        double deltaX1;
        double deltaY1;
        
        deltaX1 = (double)(this.getX() - n.getX());
        deltaY1 = (double)(this.getY() - n.getY());
       
        double dist1 = Math.Sqrt(deltaX1 * deltaX1 + deltaY1 * deltaY1);
        return dist1;
    }

    public void printNode(String description)
    {
        Debug.Log(description+" (" + x + "," + y + ")");
    }
}
