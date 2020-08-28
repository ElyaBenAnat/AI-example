using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum idle
{
    searching,using_activity
}
public enum status
{
    good,bad,dead
}
public class NPCObject : MonoBehaviour
{
    
    private ActivityObject npcObject;
    private ActivityObject targetObject;
    public GameObject Avatar;
    public CanvasActivity canvas;
    public CanvasObject canvas_object;
    public Animator anim;
    private string self_name;
    private WorldManager manager;
    private house[,] StateMap;
    //private List<State> StateList;
    public List<NeedObject> needs;
    public List<ActivityObject> AOList;
    private idle doing;
    private status health;
    private int durretionCounter;
    private ASTAR algo;
    private State currentState;
    private double dur_delta = 2;
    private double durration_stemp;
    private bool active = false;
    private List<Node> path;
    private int activity_counter;
    
    private void Awake()
    {
        npcObject = gameObject.GetComponent<ActivityObject>();
        manager = GameObject.FindObjectOfType<WorldManager>();
        algo = new ASTAR();
        currentState = new State();
        path = new List<Node>();
        durration_stemp = Time.time;
        self_name = gameObject.name;
        health = status.good;
        canvas = new CanvasActivity(transform.name, 6);
        canvas_object.setViewParameterForCanvas(canvas);
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
        InitNeeds();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Time.time-durration_stemp>dur_delta)
        {
            StateCalculator();
            if (doing is idle.searching)
            {
                buildSelfState();
               // printToDebug("running", 56);
                if (path.Count == 0)
                {
                   // printToDebug("case 1 ", 59);

                   // printToDebug("Before Astar", 61);
                    path = algo.runASTARonState(currentState, StateMap);
                    if (path == null)
                    {
                        printToDebug("error case , path not found ", 65);
                    }
                    else
                    {
                        //printToDebug("path count" + path.Count, 67);
                       // printToDebug("After Astar", 68);
                    }

                }
                else
                {
                    //printToDebug("case 2 ", 68);
                    Node next = path[path.Count-1];
                    path.RemoveAt(path.Count - 1);
                    if (next != null)
                    {
                       // printToDebug("IN change", 68);
                       // currentState.getStart().printNode("current start at NPC");
                        int oldx = npcObject.getX();
                        int oldy = npcObject.getY();
                        setAvatarDirection(oldx, oldy, next.getX(), next.getY());
                       // next.printNode("next tile in NPC");
                        npcObject.setMapLocation(next.getX(), next.getY(), manager.size);
                        manager.UpdateNPCPlaces(oldx, oldy,
                        npcObject.getX(), npcObject.getY());


                    }
                    

                }
            }else if(doing is idle.using_activity)
            {
                //printToDebug("using activity", 102);
                //printToDebug("activity counter: " + activity_counter,103);
                activity_counter--;
            }
            decreaseNeeds();
            npcPrintStatus();
            manager.RealeaseLockWorldUpdate();
            durration_stemp = Time.time;
            
        }

        playAnimation();



    }

    private void setAvatarDirection(int oldx,int oldy,int nx,int ny)
    {
        
        if (oldx==nx)
        {
            if(ny-oldy>0)
            {
               
                Avatar.transform.eulerAngles = new Vector3(
                Avatar.transform.eulerAngles.x,
                0 ,
                Avatar.transform.eulerAngles.z
            );
            }
            else
            {
                
                Avatar.transform.eulerAngles = new Vector3(
                Avatar.transform.eulerAngles.x,
                180,
                Avatar.transform.eulerAngles.z);
            }
        }
        else
        {
            if (nx - oldx > 0)
            {
                
                Avatar.transform.eulerAngles = new Vector3(
                Avatar.transform.eulerAngles.x,
                90,
                Avatar.transform.eulerAngles.z);
            }
            else
            {
               
                Avatar.transform.eulerAngles = new Vector3(
                Avatar.transform.eulerAngles.x,
               270,
                Avatar.transform.eulerAngles.z);
            }
        }
    }
    /// all of the SEARCHING state of the NPC
    
    private void buildSelfState()
    {
        //running the current state and calculating the next step
        Node start = new Node(npcObject.getX(), npcObject.getY(), color.white, house.start);
        StateMap = manager.getWorldState();
        activities a = getLowestNeed();
        activity_counter = 0;
        AOList = manager.getRelevantActivities(a);
        Node target = findClosestActivityObject();
        int  tryTemp = 0;
        while (target is null && tryTemp < 2)
        {
            target = findClosestActivityObject();
            tryTemp++;
        }
        if (target is null)
        {
            AOList = manager.getAllAvilableActivities();
            target = findClosestActivityObject();
        }
        currentState.SetState(a, start, target);
        anim = transform.gameObject.GetComponentInChildren<Animator>();
       
    }

    


    private activities getLowestNeed()
    {
        int min = needs[0].getStatus();
        activities min_need = needs[0].getNeed();
        foreach(NeedObject k in needs)
        {
            int temp = k.getStatus();
            
            if(temp<min &&k.getIfNeedUnderThreshold())
            {
                min_need = k.getNeed();
                min = temp;
            }
        }
        return min_need;
    }
    
   

    private Node findClosestActivityObject()
    {
        
        Node start = new Node(npcObject.getX(), npcObject.getY(), color.white,house.start);
        Node target = null;
        
        foreach(ActivityObject ao in AOList)
        {
            Node tempTarget = new Node(ao.getX(), ao.getY(), color.target, house.target);
            if(!ao.getStatus())
            {
                if(target is null)
                {
                    target = tempTarget;
                    targetObject = ao;
                }
                else
                {
                    if(tempTarget.GetUclidienDistance(start)< target.GetUclidienDistance(start))
                    {
                        target = tempTarget;
                        targetObject = ao;
                    }

                }
            }
        }
        return target;
    }

    

    private bool IsByActivity()
    {
        if(npcObject.getX() ==targetObject.getX())
        {
            if(npcObject.getY() +1 == targetObject.getY())
            {
                return true;
            }else if(npcObject.getY()-1 == targetObject.getY())
            {
                return true;
            }
        }else if(npcObject.getY() == targetObject.getY())
        {
            if (npcObject.getX() + 1 == targetObject.getX())
            {
                return true;
            }
            else if (npcObject.getX() - 1 == targetObject.getX())
            {
                return true;
            }
        }
        return false;
    }

    private void StateCalculator()
    {
        if(targetObject !=null)
        {
            if (doing is idle.searching && IsByActivity() && !targetObject.getStatus())
            {
                
                
                targetObject.setOcuppied();
                activity_counter = targetObject.getDurration();
               // printToDebug("activity counet init: " + activity_counter, 218);
                path.Clear();
                doing = idle.using_activity;

            }
            if (doing is idle.using_activity && activity_counter == 0)
            {
                targetObject.setUnOcuppied();
                doing = idle.searching;
            }
        }
        
    }

    private void decreaseNeeds()
    {
        
            foreach (NeedObject no in needs)
            {
                if (doing is idle.using_activity && targetObject.getNeed() == no.getNeed())
                {
                    no.increaseBy(targetObject.getWeight());
                }
                else
                {
                    if (Random.Range(0,8)%2 == 0)
                    {
                    no.decrease();
                    }
                    
                }


            }
        
        
        
        
    }

    

    
    
    //// all the USING state of the NPC

    private void performeActivity()
    {
        targetObject.setOcuppied();
    }
    //preparing :

    private void InitNeeds()
    {
        needs = new List<NeedObject>();
        NeedObject n1 = ScriptableObject.CreateInstance<NeedObject>();
        n1.SetNeedObjectValues(activities.bladder);
        needs.Add(n1);

        NeedObject n2 = ScriptableObject.CreateInstance<NeedObject>();
        n2.SetNeedObjectValues(activities.energy);
        needs.Add(n2);

        NeedObject n3 = ScriptableObject.CreateInstance<NeedObject>();
        n3.SetNeedObjectValues(activities.fun);
        needs.Add(n3);

        NeedObject n4 = ScriptableObject.CreateInstance<NeedObject>();
        n4.SetNeedObjectValues(activities.hunger);
        needs.Add(n4);

        NeedObject n5 = ScriptableObject.CreateInstance<NeedObject>();
        n5.SetNeedObjectValues(activities.hygiene);
        needs.Add(n5);

        //NeedObject n6 = ScriptableObject.CreateInstance<NeedObject>();
        //n6.SetNeedObjectValues(activities.social);
        //needs.Add(n6);
        doing = idle.searching;
        anim.Play("walking");


    }
  ///debugging:
    private void printState()
    {
        //npcPrintStatus();
        printToDebug("start:(" +currentState.getStart().getX() + "," + currentState.getStart().getY() + ")",277);
        printToDebug("target:(" + currentState.getTarget().getX() + "," + currentState.getTarget().getY() + ")",278);
    }

  

    private void npcPrintStatus()
    {
        int count_state = 0;
        foreach(NeedObject n in needs)
        {
            if(n.getIfNeedUnderThreshold())
            {
                count_state++;
            }
            canvas_object.setNewMassage(n.NeedToString());
        }
        if(count_state<3)
        {
            health = status.good;
        }
        else if(count_state<5)
        {
            health = status.bad;
        }
        else
        {
            health = status.dead;
        }
    }

    private void printToDebug(string description,int line)
    {
        Debug.Log("NPC :"+self_name+":" + description + " ; Line: " + line);
    }

    public idle getCurrentIdleStats()
    {
        return doing;
    }

    public activities getOccupiedActivity()
    {
        return targetObject.getNeed();
    }

    public status getHealth()
    {
        return health;
    }

    public Color getStatusColor()
    {
        switch(health)
        {
            case status.bad:
                return Color.red;
                
            case status.good:
                return Color.green;
                
            case status.dead:
                return Color.black;
                
            default:
                return Color.gray;
                

        }
    }

    private void playAnimation()
    {
        if(doing is idle.searching)
        {
            if(!anim.GetCurrentAnimatorStateInfo(0).IsName("walking"))
            {
                anim.Play("walking");
            }
        }else if (doing is idle.using_activity)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
            {
                anim.Play("idle");
            }
        }
    }

}
