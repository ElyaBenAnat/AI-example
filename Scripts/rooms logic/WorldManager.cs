using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager instance = null;
    public int inRows, inColumns;
    int map_size;
    public GameObject wall;
    public GameObject floor;
    public GameObject door;
    public float size;
    public MapsCreator layout;
    public VRoom physicalMap;
    private RoomPath[,] path;
    private house[,] worldMap;
    private bool unlockUpdate =false;
    private GameObject sc;
    private GameObject canvas;
    private Dictionary<GameObject, Renderer> dict;
    

    public ArrayList  activ_list;
   


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
        
    }

    // Use this for initialization
    void Start()
    {
       // sc = GameObject.FindObjectOfType<StatusCanvas>();
        dict = new Dictionary<GameObject, Renderer>();
        firstBuildFunction();


    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("exit");
            Application.Quit();
        }
        if(unlockUpdate)
        {
            foreach (GameObject go in activ_list)
            {
                ActivityObject ao = go.GetComponent<ActivityObject>();

                if(ao.getNeed() == activities.social)
                {
                    go.gameObject.transform.position = ao.getLocation();
                    //go.gameObject.transform.Translate( ao.getLocation(),Space.World);

                    NPCObject npc_temp = go.GetComponent<NPCObject>();

                    dict[go].material.SetColor("_Color", npc_temp.getStatusColor());
                    if (npc_temp.getCurrentIdleStats() ==idle.searching)
                    {
                        ao.invokeStatusMassage("(" + ao.getX() + "," + ao.getY() + ")");
                    }
                    else
                    {
                        ao.invokeStatusMassage(npc_temp.getOccupiedActivity()+"");
                    }
                    
                }
                else
                {
                    if (ao.getStatus())
                    {
                        ao.invokeStatusMassage("Occupied");
                        dict[go].material.SetColor("_Color", Color.red);
                    }
                    else
                    {
                        ao.invokeStatusMassage("avilable");
                        dict[go].material.SetColor("_Color", Color.green);
                    }
                }

                       
                    

            }
            unlockUpdate = false;
        }
        
    }

    private void firstBuildFunction()
    {
        layout = new MapsCreator();
        wall = GameObject.Find("wall");
        if (wall is null)
        {
            Debug.Log("could not find wall");
        }
        floor = GameObject.Find("floor");
        if (floor is null)
        {
            Debug.Log("could not find floor");
        }
        physicalMap = layout.getMaps();
        map_size = layout.getSize();
        size = 1f;
        worldMap = new house[layout.getSize(), layout.getSize()];
        drawPysicalrooms();

        initTheActivities();
    }

    private void drawPysicalrooms()
    {
        path = new RoomPath[layout.getSize(), layout.getSize()];
        for (int i=0;i<layout.getSize();i++)
        {
            for(int j=0;j<layout.getSize();j++)
            {
                path[i, j] = new RoomPath();
                if(physicalMap.getpathMapValue(i,j) is house.wall)
                { 
                    path[i, j].wall = Instantiate(wall, new Vector3((i * size) , -(size / 1f), j * size), Quaternion.identity) as GameObject;
                    path[i, j].wall.name = " Wall " + i + "," + j;
                    worldMap[i, j] = house.wall;
                    
                }
                else if(physicalMap.getpathMapValue(i, j) is house.floor)
                {
                    path[i,j].floor = Instantiate(floor, new Vector3(i * size, -(size / 1f), j * size), Quaternion.identity) as GameObject;
                    path[i,j].floor.name = "Floor " +i + "," + j;
                    worldMap[i, j] = house.floor;
                }
            }
        }

        foreach (Node n in layout.getEntrences())
        {
            Instantiate(door, new Vector3((n.getX() * size), 0, n.getY() * size), Quaternion.identity);
        }
    }

    private void initTheActivities()
    {
        activ_list = new ArrayList();
        
        int i, j;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("fun"))
        {
            Debug.Log("fun");
            ActivityObject ob = go.GetComponent<ActivityObject>();
            ob.setNeed(activities.fun);
            i = Random.Range(2, layout.getSize() - 2);
            j = Random.Range(2, layout.getSize() - 2);
            while (physicalMap.getpathMapValue(i, j) is house.wall ||
                physicalMap.getpathMapValue(i + 1, j) is house.wall ||
                physicalMap.getpathMapValue(i + 1, j + 1) is house.wall ||
                physicalMap.getpathMapValue(i, j + 1) is house.wall ||
                physicalMap.getpathMapValue(i - 1, j) is house.wall ||
                physicalMap.getpathMapValue(i - 1, j - 1) is house.wall ||
                physicalMap.getpathMapValue(i, j - 1) is house.wall)
            {
                i = Random.Range(2, layout.getSize() - 2);
                j = Random.Range(2, layout.getSize() - 2);
            }
            worldMap[i, j] = house.unavailable_floor;
            ob.setMapLocation(i, j, size);
            
            ob.setUpdateable();
           
            go.gameObject.transform.position = ob.getLocation();
            dict.Add(go, go.GetComponentInChildren<Renderer>());
            activ_list.Add(go);
        }
        //set the entrences objects

        
   
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("bladder"))
        {
            Debug.Log("bladder");
            ActivityObject ob = go.GetComponent<ActivityObject>();
            ob.setNeed(activities.bladder);
            i = Random.Range(2, layout.getSize()-2);
            j = Random.Range(2, layout.getSize()-2);
            while (physicalMap.getpathMapValue(i, j) is house.wall ||
                physicalMap.getpathMapValue(i +1, j) is house.wall ||
                physicalMap.getpathMapValue(i +1, j +1) is house.wall ||
                physicalMap.getpathMapValue(i, j +1) is house.wall ||
                physicalMap.getpathMapValue(i-1, j) is house.wall ||
                physicalMap.getpathMapValue(i - 1, j-1) is house.wall ||
                physicalMap.getpathMapValue(i , j - 1) is house.wall 
                )
            {
                i = Random.Range(2, layout.getSize() - 2);
                j = Random.Range(2, layout.getSize() - 2);
            }
            worldMap[i, j] = house.unavailable_floor;
            ob.setMapLocation(i, j, size);
            
            ob.setUpdateable();
            go.gameObject.transform.position = ob.getLocation();
            dict.Add(go, go.GetComponentInChildren<Renderer>());
            activ_list.Add(go);
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("hunger"))
        {
            Debug.Log("hunger");
            ActivityObject ob = go.GetComponent<ActivityObject>();
            ob.setNeed(activities.hunger);
            i = Random.Range(2, layout.getSize() - 2);
            j = Random.Range(2, layout.getSize() - 2);
            while (physicalMap.getpathMapValue(i, j) is house.wall ||
                physicalMap.getpathMapValue(i + 1, j) is house.wall ||
                physicalMap.getpathMapValue(i + 1, j + 1) is house.wall ||
                physicalMap.getpathMapValue(i, j + 1) is house.wall ||
                physicalMap.getpathMapValue(i - 1, j) is house.wall ||
                physicalMap.getpathMapValue(i - 1, j - 1) is house.wall ||
                physicalMap.getpathMapValue(i, j - 1) is house.wall)
            {
                i = Random.Range(2, layout.getSize() - 2);
                j = Random.Range(2, layout.getSize() - 2);
            }
            worldMap[i, j] = house.unavailable_floor;
            ob.setMapLocation(i, j, size);
            
            ob.setUpdateable();
            go.gameObject.transform.position = ob.getLocation();
            dict.Add(go, go.GetComponentInChildren<Renderer>());
            activ_list.Add(go);
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("hygiene"))
        {
            Debug.Log("hygiene");
            ActivityObject ob = go.GetComponent<ActivityObject>();
            ob.setNeed(activities.hygiene);
            i = Random.Range(2, layout.getSize() - 2);
            j = Random.Range(2, layout.getSize() - 2);
            while (physicalMap.getpathMapValue(i, j) is house.wall ||
                physicalMap.getpathMapValue(i + 1, j) is house.wall ||
                physicalMap.getpathMapValue(i + 1, j + 1) is house.wall ||
                physicalMap.getpathMapValue(i, j + 1) is house.wall ||
                physicalMap.getpathMapValue(i - 1, j) is house.wall ||
                physicalMap.getpathMapValue(i - 1, j - 1) is house.wall ||
                physicalMap.getpathMapValue(i, j - 1) is house.wall)
            {
                i = Random.Range(2, layout.getSize() - 2);
                j = Random.Range(2, layout.getSize() - 2);
            }
            worldMap[i, j] = house.unavailable_floor;
            ob.setMapLocation(i, j, size);
            
            ob.setUpdateable();
            go.gameObject.transform.position = ob.getLocation();
            dict.Add(go, go.GetComponentInChildren<Renderer>());
            activ_list.Add(go);
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("energy"))
        {
            Debug.Log("energy");
            ActivityObject ob = go.GetComponent<ActivityObject>();
            ob.setNeed(activities.energy);
            i = Random.Range(2, layout.getSize() - 2);
            j = Random.Range(2, layout.getSize() - 2);
            while (physicalMap.getpathMapValue(i, j) is house.wall ||
                physicalMap.getpathMapValue(i + 1, j) is house.wall ||
                physicalMap.getpathMapValue(i + 1, j + 1) is house.wall ||
                physicalMap.getpathMapValue(i, j + 1) is house.wall ||
                physicalMap.getpathMapValue(i - 1, j) is house.wall ||
                physicalMap.getpathMapValue(i - 1, j - 1) is house.wall ||
                physicalMap.getpathMapValue(i, j - 1) is house.wall)
            {
                i = Random.Range(2, layout.getSize() - 2);
                j = Random.Range(2, layout.getSize() - 2);
            }
            worldMap[i, j] = house.unavailable_floor;
            ob.setMapLocation(i, j, size);
           
            ob.setUpdateable();
            go.gameObject.transform.position = ob.getLocation();
            dict.Add(go, go.GetComponentInChildren<Renderer>());
            activ_list.Add(go);
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("social"))
        {
            Debug.Log("social");
            ActivityObject ob = go.GetComponent<ActivityObject>();
            ob.setNeed(activities.social);
            i = Random.Range(2, layout.getSize() - 2);
            j = Random.Range(2, layout.getSize() - 2);
            while (physicalMap.getpathMapValue(i, j) is house.wall ||
                physicalMap.getpathMapValue(i + 1, j) is house.wall ||
                physicalMap.getpathMapValue(i + 1, j + 1) is house.wall ||
                physicalMap.getpathMapValue(i, j + 1) is house.wall ||
                physicalMap.getpathMapValue(i - 1, j) is house.wall ||
                physicalMap.getpathMapValue(i - 1, j - 1) is house.wall ||
                physicalMap.getpathMapValue(i, j - 1) is house.wall)
            {
                i = Random.Range(2, layout.getSize() - 2);
                j = Random.Range(2, layout.getSize() - 2);
            }
            worldMap[i, j] = house.unavailable_floor;
            ob.setMapLocation(i, j, size);
            
            NPCObject npc = go.GetComponent<NPCObject>();
            
            

            
            ob.setUpdateable();


            go.gameObject.transform.position = ob.getLocation();
            dict.Add(go, go.GetComponentInChildren<Renderer>());
            activ_list.Add(go);
        }

        Debug.Log("all activities"+activ_list.Count);
       
    }


    public List<ActivityObject> getRelevantActivities(activities activity)
    {
        List<ActivityObject> l = new List<ActivityObject>();
        foreach(GameObject go in activ_list)
        {
            ActivityObject ao = go.GetComponent<ActivityObject>();
            //Debug.Log("object need " + ao.getNeed()+ " vs " + activity);
            if(ao.getNeed()  == activity)
            {
                
                    //Debug.Log("match found");
                    l.Add(ao);
                
                
            }
            
        }
        
        //Debug.Log(l.Count);
        return l;
    }

    public List<ActivityObject> getAllAvilableActivities()
    {
        List<ActivityObject> l = new List<ActivityObject>();
        foreach (GameObject go in activ_list)
        {
            ActivityObject ao = go.GetComponent<ActivityObject>();
            //Debug.Log("object need " + ao.getNeed()+ " vs " + activity);
            if (!ao.getStatus())
            {

                
                l.Add(ao);


            }

        }

        //Debug.Log(l.Count);
        return l;
    }

    public house[,] getWorldState()
    {

        return worldMap;
        
    }

    public void UpdateNPCPlaces(int oldx,int oldy,int newx,int newy)
    {
        worldMap[oldx, oldy] = house.floor;
        worldMap[newx, newy] = house.unavailable_floor;
        RealeaseLockWorldUpdate();


    }

    public void RealeaseLockWorldUpdate()
    {
        unlockUpdate = true;
    }

    public ArrayList getAllActivities()
    {
        return activ_list;
    }

    
}
