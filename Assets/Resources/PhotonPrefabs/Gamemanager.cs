using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEditor;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using TMPro;
using System;

public class Gamemanager :MonoBehaviourPunCallbacks

{
int a=-1;
  int k=0;
  public int TeamRed=0;
  public int TeamBlue=0;
  [SerializeField] TMP_Text TeamBlueScore1;
  [SerializeField] TMP_Text TeamBlueScore2;
  [SerializeField] TMP_Text TeamRedScore1;
  [SerializeField] TMP_Text TeamRedScore2;
  private string dbName="URI=file:Room.db";
    public GameObject fb;
    public GameObject Gm;
    public float force;
    bool ballcreated=false;
       private PlayerController[] PlayerControllers;
       private GoalKeeper[]  goalkeeper;
     public List<GameObject> players = new List<GameObject>();
     public List<GameObject> gk = new List<GameObject>();
     public int lastTouched;
     public bool assignTeam=false;
     public bool isHand=false;
     public bool throwIn=false;
     public string LastGoal;
     float x;
     float z;
     public Buttonassignmanager bam;
     public int match_length=180;
     [SerializeField] TMP_Text TimerText1;
     [SerializeField] TMP_Text TimerText2;
    bool starttimer=false;
    double timerincrementValue;
    double startTime;
    public double time;
    public double minute;
    public double seconds;
    ExitGames.Client.Photon.Hashtable CustomeValue;
    public GameObject mapcam;
    public GameObject endgame;
    public TMP_Text RedScore;
    public TMP_Text BlueScore;
    
    void Start()
    {
      
             CustomeValue = new ExitGames.Client.Photon.Hashtable();
             startTime = PhotonNetwork.Time;
             starttimer = true;
             CustomeValue.Add("StartTime", startTime);
            mapcam.SetActive(false);
         endgame.SetActive(false);
        
    }
     
    void Update()
    {
      if (!starttimer) return;
 
      timerincrementValue = PhotonNetwork.Time - startTime;
       time=Math.Round(timerincrementValue , 0);
       seconds=Math.Round(time%60,0);
       minute=Math.Floor(time/60);
      TimerText1.text=minute.ToString()+":"+seconds.ToString();
      TimerText2.text=minute.ToString()+":"+seconds.ToString();
 
      if (minute >= 15)
      {
        EndGame();
        //Timer Completed
        //Do What Ever You What to Do Here
      }
      fb=GameObject.Find("football-soccer-ball");
      x=fb.transform.position.x;
       z=fb.transform.position.z;
       
     if(!assignTeam&&PhotonNetwork.CurrentRoom.Name!=null)
     {
       Assigning();
     }
     if((PhotonNetwork.CurrentRoom.CustomProperties["isHand"]!=null))
     {
       if((int)PhotonNetwork.CurrentRoom.CustomProperties["isHand"]==1)
        FreeKick();
     }
     PlayerControllers = FindObjectsOfType<PlayerController>();
     goalkeeper=FindObjectsOfType<GoalKeeper>();
     TeamBlueScore1.text=TeamBlue.ToString();
     TeamBlueScore2.text=TeamBlue.ToString();
     TeamRedScore1.text=TeamRed.ToString();
     TeamRedScore2.text=TeamRed.ToString();
     for (int i = 0; i < PlayerControllers.Length; i++)
         {
             players.Add(PlayerControllers[i].gameObject);
         }
     for (int i = 0; i < goalkeeper.Length; i++)
     {
        gk.Add(goalkeeper[i].gameObject);
     }
   
    if(((x>63.8f||x<-50.1||z>77.5||z<-77.7))&&!((x<14.5f&&x>-0.9f)&&(z>77.5||z<-77.7))&&!throwIn)
      {
        Debug.Log("Throw In");
        Debug.Log((int)PhotonNetwork.CurrentRoom.CustomProperties["lastTouched"]);
        int j=0;
        float distance=float.PositiveInfinity;
        Debug.Log(PlayerControllers.Length);
        for (int i = 0; i < PlayerControllers.Length; i++)
         {
           if(PlayerControllers[i].Team!=(int)PhotonNetwork.CurrentRoom.CustomProperties["lastTouched"])
            {
              Debug.Log(PlayerControllers[i].Team);
               if(distance==float.PositiveInfinity)
             {
                distance=Vector3.Distance(fb.transform.position,players[i].transform.position);
                Debug.Log(distance);
                j=i;
             }
             else if(distance>Vector3.Distance(fb.transform.position,players[i].transform.position))
             {
               distance=Vector3.Distance(fb.transform.position,players[i].transform.position);
                Debug.Log(distance);
                j=i;
             }
            }
         }
        Debug.Log(j);
         players[j].transform.position = new Vector3(x, 17f, z);
         Debug.Log(PlayerControllers[j].Team);
         PlayerControllers[j].isThrowIn=true;
         throwIn=true;
      }

    }
    void storePosition()
    {
         Player[] players = PhotonNetwork.PlayerList;
         foreach(Player v in players)
        {
          Debug.Log(v.NickName);
        }
    }
    void Assigning()
    {
      foreach (Player p in PhotonNetwork.PlayerList)
                {
                  int i=int.Parse(p.CustomProperties["Button"].ToString());
                     if(i<10)
                      {
                      var hash = new Hashtable();
                        hash.Add("Team",0);
                        p.SetCustomProperties(hash);
                      }
                      else
                      {
                      var  hash2 = new Hashtable();
                        hash2.Add("Team", 1);
                        p.SetCustomProperties(hash2);
                      }
                }
      var hash3 = new Hashtable();
       hash3.Add("isHand",null);
       PhotonNetwork.CurrentRoom.SetCustomProperties(hash3);
       var hash4 = new Hashtable();
       hash4.Add("lastTouched",null);
       PhotonNetwork.CurrentRoom.SetCustomProperties(hash4);
      assignTeam=true;
    }
  public  void ThrowIn()
    {
      throwIn=true;
        GameObject player2;
        int j=0;
        float distance=0f;
        for (int i = 0; i < PlayerControllers.Length; i++)
         {
           if(PlayerControllers[i].Team!=(int)PhotonNetwork.CurrentRoom.CustomProperties["lastTouched"])
            { Debug.Log(PlayerControllers[i].Team);
              if(distance==0)
             {
                distance=Vector3.Distance(fb.transform.position,players[i].transform.position);
             }
             else if(distance>Vector3.Distance(fb.transform.position,players[i].transform.position))
             {
               distance=Vector3.Distance(fb.transform.position,players[i].transform.position);
              j=i;
             }
            }
         }

         players[j].transform.position = new Vector3(x, 17f, z);
         PlayerControllers[j].isThrowIn=true;
         throwIn=true;
    }
  void FreeKick()
  {
   
   
    int kick=(int)PhotonNetwork.CurrentRoom.CustomProperties["lastTouched"];
     
     
    
        int j=0;
        float distance=float.PositiveInfinity;
        for (int i = 0; i < PlayerControllers.Length; i++)
         {
           if(PlayerControllers[i].Team!=(int)PhotonNetwork.CurrentRoom.CustomProperties["lastTouched"])
            { if(distance==float.PositiveInfinity)
             {
                distance=Vector3.Distance(fb.transform.position,players[i].transform.position);
             }
             else if(distance>Vector3.Distance(fb.transform.position,players[i].transform.position))
             {
               distance=Vector3.Distance(fb.transform.position,players[i].transform.position);
              j=i;
             }
            }
            else{
              PlayerControllers[i].fb=null;
            }
         }
        
        if((int)PhotonNetwork.CurrentRoom.CustomProperties["lastTouched"]!=a&&a!=-1)
        {
           for (int i = 0; i < PlayerControllers.Length; i++)
          {
            if(PlayerControllers[i].Team!=(int)PhotonNetwork.CurrentRoom.CustomProperties["lastTouched"])
            { 
              PlayerControllers[i].fb=GameObject.Find ("football-soccer-ball");
            }
          }
          a=-1;
           var hash2 = new Hashtable();
                         hash2.Add("isHand",0);
                        PhotonNetwork.CurrentRoom.SetCustomProperties(hash2);;
        }
        a=kick;
    
  }
  
  public void Goal()
  {
    if(LastGoal=="TeamBlue")
    {
      if(goalkeeper[0].Team==0)
      {
        gk[0].transform.position=new Vector3(7f,17f,-68.09866f);
        goalkeeper[0].anim.SetTrigger("goalkeeper_idle");
      }
      else
      {
          gk[1].transform.position=new Vector3(7f,17f,-68.09866f);
        goalkeeper[1].anim.SetTrigger("goalkeeper_idle");
      }
    }
    if(LastGoal=="TeamRed")
    {
      if(goalkeeper[0].Team==1)
      {
        gk[0].transform.position=new Vector3(7f,17f,68.09866f);
        goalkeeper[0].anim.SetTrigger("goalkeeper_idle");
      }
      else
      {
          gk[1].transform.position=new Vector3(7f,17f,68.09866f);
        goalkeeper[1].anim.SetTrigger("goalkeeper_idle");
      }
    }
  }
 private void EndGame()
 {
   if(PhotonNetwork.IsMasterClient)
   {
     PhotonNetwork.DestroyAll();
     PhotonNetwork.CurrentRoom.IsVisible=false;
     PhotonNetwork.CurrentRoom.IsOpen=false;
   }
   mapcam.SetActive(true);
   endgame.SetActive(true);
   RedScore.text=TeamRed.ToString();
   BlueScore.text=TeamBlue.ToString();
   StartCoroutine(end(6f));
 }
  private System.Collections.IEnumerator end(float p_wait)
  {
    yield return new WaitForSeconds(p_wait);
      PhotonNetwork.AutomaticallySyncScene=false;
      PhotonNetwork.LeaveRoom();
  }
  public override void OnLeftRoom()
  {
    base.OnLeftRoom();
    SceneManager.LoadScene(0);

  }
}
