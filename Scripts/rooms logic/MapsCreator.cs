using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MapsCreator 
{
    int size = 49;
    public VRoom maps;
    public List<Node> enternces;
    public MapsCreator()
    {
        maps = new VRoom(size);
        enternces = new List<Node>();
        setPysicalMap();
    }

    private void setPysicalMap()
    {
        //bulding plane
        for(int i=0;i<size;i++)
        {
            for(int j=0;j<size;j++)
            {
                maps.setpathMap(i, j, house.floor);
            }
        }
        //build the outer walls 
        for(int i=0;i<size;i++)
        {
            maps.setpathMap(i, 0, house.wall);
            maps.setpathMap(0, i, house.wall);
            maps.setpathMap(i, size-1, house.wall);
            maps.setpathMap(size-1, i, house.wall);
        }
        
        //build kitchen
        setRoom(0, 0, 20, 10,20, 5);
        //build bedroom:
        setRoom(0,9, 20, 20, 20, 5);
        //build tv room:
        setRoom(0, 28, 20, 21, 20, 5);
        //room:
        setRoom(29, 0, 20, 20, 1, 6);
        //room:
        setRoom(29, 19, 20, 20, 1, 6);
    }

    public int getSize()
    {
        return size;
    }

    public VRoom getMaps()
    {
        return maps;
    }

    private void setRoom(int r_start, int c_start,int size_length,int size_width,int r_enternce,int c_enternce)
    {
        for (int i = r_start; i < r_start+size_length; i++)
        {
            maps.setpathMap(i, c_start, house.wall);
            maps.setpathMap(i, c_start+size_width-1, house.wall);
        }
        for (int j = c_start; j < c_start + size_width; j++)
        {
            maps.setpathMap(r_start, j, house.wall);
            maps.setpathMap(r_start+size_length-1, j, house.wall);
        }
        // set the enterence:
        maps.setpathMap(r_start +r_enternce-1,c_start+ c_enternce-1 , house.floor);
        enternces.Add(new Node(r_start + r_enternce - 1, c_start + c_enternce - 1));
       
    }

    public List<Node> getEntrences()
    {
        return enternces;
    }
    


    
}
