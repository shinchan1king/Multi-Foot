using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_for_1st_goal : MonoBehaviour
{
    public Gamemanager Gm;
    public GameObject fb;
     double time=double.NegativeInfinity;
               
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fb=GameObject.Find("football-soccer-ball");
        Gm=FindObjectOfType<Gamemanager>();
    }
      private void OnCollisionEnter(Collision collision)
   {
       
       if(collision.gameObject.name == "football-soccer-ball")
        {
            float x= fb.transform.position.x;
            float y=fb.transform.position.y;
            float z=fb.transform.position.z;
            if(x<14.5f&&x>-0.9f&&z>-81.5f&&y<21.5f)
            {
                if(time==double.NegativeInfinity)
                    {
                        Gm.TeamBlue++;
                        Gm.LastGoal="TeamBlue";
                        Gm.Goal();
                        time=Gm.time;
                    }
                if(Gm.LastGoal=="TeamBlue")
                {
                    if(time==double.NegativeInfinity||time+10<Gm.time)
                    {
                        Gm.TeamBlue++;
                        Gm.LastGoal="TeamBlue";
                        Gm.Goal();
                        time=Gm.time;
                    }
                    
                }
                else{
                    Gm.TeamBlue++;
                        Gm.LastGoal="TeamBlue";
                        Gm.Goal();
                         time=Gm.time;
                    }
                }
                }
            else
            {
                Gm.ThrowIn();
            }
       }
   }

