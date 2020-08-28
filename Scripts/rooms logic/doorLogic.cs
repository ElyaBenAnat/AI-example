using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorLogic : MonoBehaviour
{
    public Animator door;
    public BoxCollider collider_b;
    public GameObject door_object;
    bool open = false;
    bool close = true;
    
    // Start is called before the first frame update
    void Start()
    {
        collider_b = transform.GetComponent<BoxCollider>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (open)
        {
            Debug.Log("u-open");
            if (door_object.transform.eulerAngles.y<90)
            {
                Debug.Log("u-openning");
                door_object.transform.eulerAngles = new Vector3(
                 door_object.transform.eulerAngles.x,
                 90,
                 door_object.transform.eulerAngles.z);
                //door_object.transform.localPosition = new Vector3(door_object.transform.localPosition.x - 0.5f, door_object.transform.localPosition.y, door_object.transform.localPosition.z);


                open = false;
            }
            
                
               
            
        }
        if (close)
        {
            Debug.Log("u-close");
            if (door_object.transform.eulerAngles.y>0)
            {
                Debug.Log("u-closing");
                door_object.transform.eulerAngles = new Vector3(
                  door_object.transform.eulerAngles.x,
                  0,
                  door_object.transform.eulerAngles.z);
                //door_object.transform.localPosition = new Vector3(door_object.transform.localPosition.x + 0.5f, door_object.transform.localPosition.y, door_object.transform.localPosition.z);
                close = false;
            }
            
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        
       
        if(!open)
        {
            Debug.Log("open");
            open = true;
        }
        
        
    }
    private void OnTriggerExit(Collider other)
    {
        
        if(!close)
        {
            Debug.Log("close");
            close = true;
        }
        
    }
}
