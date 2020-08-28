using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasObject : MonoBehaviour
{
    private CanvasActivity _canvas_activity;
    Text title;
    List<Text> _textList;
    public Text textPrefab;
    
    private double dur_delta = 2;
    private double durration_stemp;
    private bool update = false;
    // Start is called before the first frame update

    private void Awake()
    {
        
    }


    void Start()
    {
        
      
    }

    public void setViewParameterForCanvas(CanvasActivity ca)
    {
        _canvas_activity = ca;
        _textList = new List<Text>();
        title = Instantiate(textPrefab) as Text;
        title.transform.SetParent(transform, false);
        title.fontSize = 25;
        title.text = ca.getTitle();
        title.color = Color.yellow;
        foreach(string s in ca.getMassages())
        {
            Text t= Instantiate(textPrefab) as Text;
            t.transform.SetParent(transform, false);
            t.fontSize = 15;
            t.text = s;
            _textList.Add(t);
        }
        durration_stemp = Time.time;
        update = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (update)
        {
            List<string> l = _canvas_activity.getMassages();
            if (Time.time - durration_stemp > dur_delta)
            {
                int i = 0;
                foreach (Text rt in _textList)
                {
                    rt.text = l[i];
                    i++;
                }
            }
        }
    }

    public void setNewMassage(string s)
    {
        _canvas_activity.updateNewMassege(s);
    }
}
