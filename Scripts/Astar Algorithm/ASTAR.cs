using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASTAR 
{
    private State state;
    private house[,] StateMap;
    private PriorityQueue<Node> pq;
    private PriorityQueue<Node> t_pq;
    private 
    
    List<Node> path;
    private bool run;
    private int epocsMax = 2000;
    List<Node> gray_queue;
    
    public ASTAR()
    {
        pq = new PriorityQueue<Node>();
        t_pq = new PriorityQueue<Node>();
        gray_queue = new List<Node>();
        path = new List<Node>();
    }

    public List<Node> runASTARonState(State s, house[,] map)
    {
        clearAll();
        run = true;
        state = s;
        StateMap = map;
        Node start = s.getStart();
        start.setParent(null);
        start.setColor(color.start);
        createLocalNeigbors(s.getTarget());
        if(t_pq.IsEmpty())
        {
            return null;
        }
        Node target = t_pq.Peek();
        
        path.Clear();
        target.setColor(color.white_target);
        target.setTarget(target);
        start.setTarget(target);
        pq.Enqueue(start);
        int counter = 0;
        while (run && counter<epocsMax)
        {
            ASTARStep();
            counter++;
        }
        if(run && !pq.IsEmpty())
        {
            setPath(pq.Peek());
        }
        clearAll();
        printToDebug("count of returned queue " + path.Count, 58);
        return path;
    }

    private void  ASTARStep()
    {
        //Debug.Log("astar step");
        Node current, next;
        if(pq.IsEmpty())
        {
            
            printToDebug("pq is empty", 71);
            run = false;
        }
        else
        {
            current = pq.Peek();
            pq.Dequeue();
            
            if (current.getColor() == color.gray_target)
            {
                printToDebug("found a solution",80);
                setPath(current);
                run = false;
            }
            else
            {
                createNeigbors(current);
                while(!t_pq.IsEmpty())
                {
                    next = t_pq.Peek();
                   // next.printNode();
                    t_pq.Dequeue();
                    if(next.getColor() == color.white || next.getColor()== color.start)
                    {
                        next.setColor(color.gray);
                        next.SetG(current.GetG() + 1);
                        next.setParent(current);
                        pq.Enqueue(next);
                    }else if(next.getColor() ==color.white_target)
                    {
                        next.setColor(color.gray_target);
                        next.SetG(current.GetG() + 1);
                        next.setParent(current);
                        pq.Enqueue(next);
                    }
                    else if(next.getColor() == color.gray_target || next.getColor() == color.gray)
                    {
                        int dist1 = next.GetG();
                        int dist2 = current.GetG() + 1;
                        if(dist1>dist2)
                        {
                            next.SetG(current.GetG() + 1);
                            updatePriorityQueue(next,current);
                        }
                    }
                }


            }
            
            gray_queue.Add(current);
        }
    }

    private void createLocalNeigbors(Node n)
    {
        
        while (!t_pq.IsEmpty())
        {
            t_pq.Dequeue();
        }
        int i = n.getX();
        int j = n.getY();
        if(n is null)
        {
            return;
        }
        if (0 < j - 1 )
        {
            if(StateMap[i, j - 1] == house.floor)
            {
                Node neigborNode = new Node(i, j - 1, color.white, house.floor);
                t_pq.Enqueue(neigborNode);
            }
            

        }
        if (j + 1 < StateMap.Length-1)
        {
            if(StateMap[i, j + 1] == house.floor)
            {
                Node neigborNode = new Node(i, j + 1, color.white, house.floor);
               // neigborNode.printNode();
                t_pq.Enqueue(neigborNode);
            }
            
        }
        if (0 < i - 1 )
        {
            if(StateMap[i-1, j] == house.floor)
            {
                Node neigborNode = new Node(i - 1, j, color.white, house.floor);
               // neigborNode.printNode();
                t_pq.Enqueue(neigborNode);
            }
            
        }
        if (i + 1 < StateMap.Length-1)
        {
            if(StateMap[i+1, j ] == house.floor)
            {
                Node neigborNode = new Node(i + 1, j, color.white, house.floor);
               // neigborNode.printNode();
                t_pq.Enqueue(neigborNode);
            }
            
        }
    }

    private void updatePriorityQueue(Node n,Node parent)
    {
       // Debug.Log("update queue");
        List<Node> tempNodeList = new List<Node>();
        Node tempNode = new Node(n.getX(), n.getY());
        Node pq_node = pq.Peek();
        pq.Dequeue();
        while (!pq.IsEmpty() && !MatchIndex(n, pq_node))
        {
            tempNodeList.Add(pq_node);
            pq_node = pq.Peek();
            pq.Dequeue();
        }
        
        if (pq.IsEmpty())
        {
            Debug.Log("problem");
        }
        else
        {
            pq_node.SetG( n.GetG());
            pq_node.setColor(n.getColor());
            pq_node.setParent(parent);
            pq_node.setTarget(n.GetTarget());
        }
        tempNodeList.Add(pq_node);
        //return everthing to the pq
        foreach (Node no in tempNodeList)
        {
            pq.Enqueue(no);
        }
       
    }

    private void setPath(Node current)
    {
        // Debug.Log("set next node");
        Node temp = new Node(current.getX(), current.getY());
        path.Add(temp);
        current = current.getParent();
        
        while(!((current.getParent()) is null))
        {
            Node temp2 = new Node(current.getX(),current.getY());
            path.Add(temp2);
            current = current.getParent();
            
        }
        
        
    }

    private void createNeigbors(Node n)
    {
       // Debug.Log("create neigbors");
        while (!t_pq.IsEmpty())
        {
            t_pq.Dequeue();
        }
        int i = n.getX();
        int j = n.getY();
        
        if (0<j-1)
        {
            if(StateMap[i, j - 1] == house.floor)
            {
                Node neigborNode = new Node(i, j - 1, color.white, house.floor);
                neigborNode.setTarget(n.GetTarget());
                if (IsInGrayQueue(neigborNode))
                {
                    neigborNode.setColor(getColorFromPQ(neigborNode));
                }
                neigborNode.setParent(n);
                
                t_pq.Enqueue(neigborNode);
            }
            
         
        }
        if ( j + 1<StateMap.Length)
        {
            if(StateMap[i, j - 1] == house.floor)
            {
                Node neigborNode = new Node(i, j + 1, color.white, house.floor);
                neigborNode.setTarget(n.GetTarget());
                if (IsInGrayQueue(neigborNode))
                {
                    neigborNode.setColor(getColorFromPQ(neigborNode));
                }
                neigborNode.setParent(n);
                
                t_pq.Enqueue(neigborNode);
            }
            
        }
        if (0 < i - 1 )
        {
            if(StateMap[i, j - 1] == house.floor)
            {
                Node neigborNode = new Node(i - 1, j, color.white, house.floor);
                neigborNode.setTarget(n.GetTarget());
                if (IsInGrayQueue(neigborNode))
                {
                    neigborNode.setColor(getColorFromPQ(neigborNode));

                }
                neigborNode.setParent(n);
                
                t_pq.Enqueue(neigborNode);
            }


            
        }
        if (i + 1 < StateMap.Length)
        {
            if(StateMap[i, j - 1] == house.floor)
            {
                Node neigborNode = new Node(i + 1, j, color.white, house.floor);
                neigborNode.setTarget(n.GetTarget());
                if (IsInGrayQueue(neigborNode))
                {
                        neigborNode.setColor(getColorFromPQ(neigborNode));
                }
                neigborNode.setParent(n);
               
                t_pq.Enqueue(neigborNode);
            }
            
        }


    }

    private bool IsInGrayQueue(Node n_in)
    {
       // Debug.Log("isin gray");
        foreach (Node n in gray_queue)
        {
            if(n.getX()==n_in.getX()&& n.getY()==n_in.getY())
            {
                return true;
            }
        }
        return false;
    }

    private color getColorFromPQ(Node n_in)
    {
        color ctemp = color.white;
        List<Node> l = new List<Node>();
        Node pcurrent = null;
        
        if (MatchIndex(n_in, n_in.GetTarget()))
        {
            ctemp = color.white_target;
            //for first attachment
        }
        while (!pq.IsEmpty())
        {
            pcurrent = pq.Peek();
            if (pcurrent != null)
            {
                pq.Dequeue();
                if (MatchIndex(n_in, pcurrent))
                {
                    ctemp= pcurrent.getColor();


                }
                l.Add(pcurrent);
            }



        }

        // insert back from tmp to pq 
        foreach(Node no in l)
        {
            pq.Enqueue(no);
        }
        l.Clear();

        return ctemp;
    }

    private bool MatchIndex(Node n1,Node n2)
    {
       // Debug.Log("match");
        if (n1.getX()==n2.getX()&&n1.getY()==n2.getY())
        {
            return true;
        }
        return false;
    }

    private void clearAll()
    {
        //Debug.Log("clear");
        while (!pq.IsEmpty())
        {
            pq.Dequeue();
        }
        while (!t_pq.IsEmpty())
        {
            t_pq.Dequeue();
        }
        
        gray_queue.Clear();
    }

    private void printToDebug(string description, int line)
    {
        Debug.Log("ASTAR :" + description + " ; Line: " + line);
    }
}
