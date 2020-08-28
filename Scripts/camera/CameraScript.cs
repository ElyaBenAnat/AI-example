using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float speed =10;
    const float movement_range = 50f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        if(pos.x<movement_range&&-movement_range<pos.x&&
            pos.y < movement_range && -movement_range < pos.y&&
            pos.z < movement_range && -movement_range < pos.z)
        if (Input.GetKey(KeyCode.RightArrow))
        {
            
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime , 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, -speed * Time.deltaTime , 0));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, speed * Time.deltaTime , 0));
        }
    }
}
