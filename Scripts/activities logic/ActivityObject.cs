using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityObject : MonoBehaviour
{

    public GameObject activity;
    public CanvasActivity canvas;
    public CanvasObject canvas_object;
    bool isOccupied;
    private activities need;
    private int x;
    private int y;
    private int durretion;
    private int weight;
    private Vector3 location;
    
    private bool update;
    // Start is called before the first frame update
    void Start()
    {
        activity = transform.GetComponent<GameObject>();
        isOccupied = false;
        durretion = Random.Range(2, 8);
        weight= Random.Range(2, 8);
        update = false;
        canvas = new CanvasActivity(transform.name, 5);
        canvas_object.setViewParameterForCanvas(canvas);
        
        
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }
    public void setUnOcuppied()
    {
        isOccupied = false;
    }

    public void setOcuppied()
    {
        isOccupied = true;
    }

    public bool getStatus()
    {
        return isOccupied;
    }

    public void setNeed(activities need)
    {
        this.need = need;
    }

    public activities getNeed()
    {
        return need;
    }

    public void  setMapLocation(int x, int y,float size)
    {
        this.x = x;
        this.y = y;
        if(x>0&&y>0)
        {
            location = new Vector3((x - 1) * size + size / 2, -(size / 1f), (y - 1) * size + size / 2);
        }else
        {
            location = new Vector3(x  * size , -(size / 1f), y  * size );
        }
        
       // transform.Translate(location);
        
    }

    public int getX()
    {
        return x;

    }

    public int getY()
    {
        return y;

    }

    public Vector3 getLocation()
    {
        return location;
    }

    public void PrintActivity()
    {
        //Debug.Log("Activity:")
    }

    public void invokeStatusMassage(string s)
    {
        canvas_object.setNewMassage(s);
    }

    public int getDurration()
    {
        return durretion;
    }

    public int getWeight()
    {
        return weight;
    }


    public void setUpdateable()
    {
        update = true;
    }
}
