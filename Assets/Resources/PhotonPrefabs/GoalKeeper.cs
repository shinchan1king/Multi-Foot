

using Photon.Realtime;
using Photon.Pun;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System;
using System.Threading;
using System.IO;
using System.Runtime.CompilerServices;
using ExitGames.Client.Photon;

using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GoalKeeper:MonoBehaviourPunCallbacks
{
    
    public GameObject Gamemanager;
  private  Gamemanager gm;
   public newTrijectory Ball;
    float force=12.5f;

     float speed=5.0f;
    float rotationSpeed=100.0f;
     float rotation;
    float translation;
    public SphereCollider sphereCollider;
    public MeshCollider meshCollider;
    public Transform goalkeeper;
    public GameObject fb;
    Transform target;
    public Transform hand;
    public Transform hand2;
   public  Animator anim;
    float weight=1f;
    public GameObject cursor;
    public LayerMask layer;

    public Rigidbody rb;
    private Camera cam;
    bool isshoot=false;
    public Transform cr;
    
    public GameObject Camerachanger;
    public Transform cm;
    int dive=0;
    public Collider objectCollider;
 public Collider anotherCollider;
    bool isTouching =false;
    bool isPlayed =false;
     PhotonView PV;
     public GameObject newBall;
     public int Team;
     bool assignComponents=false;
     public TMP_Text playerName;
    void Awake() {
     
     
      PV=GetComponent<PhotonView>();
     }
    void Start()
    {
       // Camerachanger= Instantiate(Camerachanger,Camerachanger.transform.position,Quaternion.identity );
      // target=gm.fb.transform;
      
        cam=Camera.main;
        anim=GetComponent<Animator>();
        
           if(!PV.IsMine)
       {
           Destroy(GetComponentInChildren<Camera>().gameObject);
       }
      if (photonView.IsMine)
        {
            //Player is local
            gameObject.tag = "Player";
            //Add Rigidbody to make the player interact with rigidbody
            Rigidbody r = gameObject.AddComponent<Rigidbody>();
            r.isKinematic = true;
        }
        
       fb=GameObject.Find ("football-soccer-ball");
       gm=FindObjectOfType<Gamemanager>();
       Camerachanger=GameObject.Find ("camerachanger");
        //fb=Instantiate(gm.ballInstance,gm.ballInstance.transform.position,Quaternion.identity);
        cursor=GameObject.Find ("Circle");
       cr=cursor.GetComponent<Transform>();
    }
    
    void Update()
    {
        
       
        if(!PV.IsMine)
       {
           return;
       }
       float right_boundary=-52f;
       float left_boundary=66f;
       float up_boundary=86f;
       float down_boundary=-86f;
        if(transform.position.y<17f ||transform.position.y>17f)
       {
           transform.position=new Vector3(transform.position.x,17f,transform.position.z);
       }
       if(transform.position.x>=left_boundary)
       {
           transform.position=new Vector3(left_boundary,transform.position.y,transform.position.z);
       }
       if(transform.position.x<=right_boundary)
       {
           transform.position=new Vector3(right_boundary,transform.position.y,transform.position.z);
           
       }
       if(transform.position.z>=up_boundary)
       {
           transform.position=new Vector3(transform.position.x,transform.position.y,up_boundary);
       }
       if(transform.position.z<=down_boundary)
       {
           transform.position=new Vector3(transform.position.x,transform.position.y,down_boundary);
       }
       if(!assignComponents&&PhotonNetwork.CurrentRoom.Name!=null)
     {

       Assigning();
       assignComponents=true;
     }

        if (Input.GetKeyDown("j")&& Vector3.Distance(goalkeeper.position, fb.transform.position)<=2)
        {
            isshoot=true;
            anim.SetTrigger("pickup");
            cm.parent=Camerachanger.GetComponent<Transform>();
            dive=0;
            //cm.position=Camerachanger.position;
          // cm.rotation=Camerachanger.rotation;
        }
        /*if(isPlayed&&isTouching)
        {
             dive=0;
                        async Task delaycreator()
                        {
                        var t1=Task.Delay(2000);
                        await t1;
                        anim.SetTrigger("goalkeeper_idle");
                        Debug.Log("Hii");
                        }
                         delaycreator();
        }*/
         if((isPlaying("l_dive")||isPlaying("r_Dive")||isPlaying("goalkeeper_catch")))
              {
                  
                  if (isTouching)
                  {
                      
                         dive=0;
                        async Task delaycreator()
                        {
                        var t1=Task.Delay(2000);
                        await t1;
                        anim.SetTrigger("goalkeeper_idle");
                        Debug.Log("Hii");
                        Debug.Log(isTouching);
                        }
                         delaycreator();
                        isTouching=false;
                  }
                   // OnTriggerEntered(objectCollider,anotherCollider);
                //isTouching=false;
              }
        goalkeeper.rotation = Quaternion.Euler(0,goalkeeper.eulerAngles.y,0);
        
        if(!isshoot)
        {
            cm.parent=goalkeeper;
            cm.localPosition=new Vector3(0.3968916f,1.125919f,-4.491429f);
            //goalkeeper.localPosition=new Vector3(x,16.8f,z);
           cm.localRotation=Quaternion.Euler(1.685f,7.015f,-0.006f);
        }
        translation=Input.GetAxis("Vertical")*speed;
         rotation=Input.GetAxis("Horizontal")*rotationSpeed;
       
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
       
        transform.Translate(0, 0, translation);
        transform.Rotate(0,rotation,0);
        if(translation != 0)
        {
            //force=7.5f;
            cm.parent=goalkeeper;
            isshoot=false;
            anim.SetBool("isRunning",true);
            anim.SetFloat("characterSpeed",translation);
            isTouching=false;
        }
        else
        {
            force=7.5f;
           if(isPlaying("goalkeeper_idle"))
              {isshoot=true;
               LaunchProjectile();
              // cm.parent=Camerachanger;
              }
            anim.SetBool("isRunning",false);
             anim.SetFloat("characterSpeed",0);
             isTouching=false;
            
        }
        if(Input.GetKeyDown("space"))
        {
            force=10.0f;
            anim.SetTrigger("jumpHeader");
            isTouching=false;
            
        }
        if(Input.GetKeyDown("q"))
        {
            dive=1;
            anim.SetTrigger("leftDive");
            
            
        }
        if(Input.GetKeyDown("e"))
        {
            dive=1;
            anim.SetTrigger("rightDive");
            
            
        }
        if(Input.GetKeyDown("v"))
        {
            force=12.0f;
            anim.SetTrigger("slide");
            isTouching=false;
        }
        if(Input.GetKeyDown("x"))
        {
            dive=1;
            Debug.Log(dive);
            anim.SetTrigger("goalkeeper_catch");
        }
        if(Input.GetKeyDown("r"))
        {
            anim.SetTrigger("rightBlock");
            isTouching=false;
        }
        if(Input.GetKeyDown("l"))
        {
            anim.SetTrigger("leftBlock");
            isTouching=false;
        }
        if(Input.GetKeyDown("f"))
        {
            anim.SetTrigger("sideLeft");
            isTouching=false;
        }
        if(Input.GetKeyDown("g"))
        {
            anim.SetTrigger("sideRight");
            isTouching=false;
        }
        if(Input.GetKeyDown("z"))
        {
            anim.SetTrigger("cycleKick");
            isTouching=false;
        }
        if(Input.GetKeyDown("c"))
        {
            force=10.5f;
            anim.SetTrigger("chip");
            isTouching=false;
        }
        if(Input.GetKeyDown("p"))
        {
            force=15.0f;
            anim.SetTrigger("pass");
            isTouching=false;
        }
       if(isPlaying("goalkeeper_idle")||isPlaying("pickup"))
       {
           isTouching=false;
           cm.parent=Camerachanger.GetComponent<Transform>();
           sphereCollider=fb.GetComponent<SphereCollider>();
            sphereCollider.enabled=false;
            meshCollider=fb.GetComponent<MeshCollider>();
            meshCollider.enabled=false;
            fb.transform.GetComponent<Rigidbody>().useGravity=false;
            float x=goalkeeper.position.x;
            float z=goalkeeper.position.z;
            fb.transform.parent=hand;
           // target.parent=hand2;
            fb.transform.localPosition=new Vector3(0f,0.1f,0.19f);
            //goalkeeper.localPosition=new Vector3(x,16.8f,z);
            fb.transform.localRotation=Quaternion.Euler(154.537f,64.417f,39.14799f);
       }
       if(isPlaying("running"))
       {

           sphereCollider=fb.GetComponent<SphereCollider>();
            sphereCollider.enabled=false;
            meshCollider=fb.GetComponent<MeshCollider>();
            meshCollider.enabled=false;
            fb.transform.GetComponent<Rigidbody>().useGravity=false;
            float x=goalkeeper.position.x;
            float z=goalkeeper.position.z;
            fb.transform.parent=hand;
           // fb.transform.parent=hand2;
            fb.transform.localPosition=new Vector3(0f,0.1f,0.19f);
            //goalkeeper.localPosition=new Vector3(x,16.8f,z);
            fb.transform.localRotation=Quaternion.Euler(154.537f,64.417f,39.14799f);
       }
        if(isPlaying("throw"))
        {
             cm.parent=goalkeeper;
            cm.localPosition=new Vector3(0.3968916f,1.125919f,-4.491429f);
            //goalkeeper.localPosition=new Vector3(x,16.8f,z);
           cm.localRotation=Quaternion.Euler(1.685f,7.015f,-0.006f);
        }
       /*if(!isPlaying("goalkeeper_idle"))
       {
           cursor.SetActive(false);
       }*/
       
    }
    void OnAnimatorIK(int layerIndex)
    {
        
        weight= anim.GetFloat("IKPickup");
        if(weight>0.8&&dive==0)
        {
            sphereCollider=fb.GetComponent<SphereCollider>();
            sphereCollider.enabled=false;
            meshCollider=fb.GetComponent<MeshCollider>();
            meshCollider.enabled=false;
            fb.transform.GetComponent<Rigidbody>().useGravity=false;
            float x=goalkeeper.position.x;
            float z=goalkeeper.position.z;
            fb.transform.parent=hand;
           // fb.transform.parent=hand2;
            fb.transform.localPosition=new Vector3(0f,0.1f,0.19f);
            //goalkeeper.localPosition=new Vector3(x,16.8f,z);
            fb.transform.localRotation=Quaternion.Euler(154.537f,64.417f,39.14799f);
        }
        anim.SetIKPosition(AvatarIKGoal.RightHand,fb.transform.position);
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand,weight);
        anim.SetIKPosition(AvatarIKGoal.LeftHand,fb.transform.position);
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand,weight);
    }
     bool isPlaying(string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName))
    {
        return true;
    }

        else
            return false;
    }
   void LaunchProjectile()
   {
      // cm.parent=default;
       Ray camRay =cam.ScreenPointToRay(Input.mousePosition);
       RaycastHit hit;
       if(isshoot)
       {if(Physics.Raycast(camRay,out hit,100f,layer))
       {
           // cm.parent=Camerachanger;
          // cm.position=Camerachanger.position;
          // cm.rotation=Camerachanger.rotation;
           cursor.SetActive(true);
           cursor.transform.position=hit.point+Vector3.up*0.1f;
           Vector3 Vo = CalculateVelocity(hit.point,hand.position,1f);
          transform.LookAt(cursor.transform.position);
          //transform.rotation=Quaternion.LookRotation(Vo);
           if(Input.GetMouseButtonDown(0))
           {
               isshoot=false;
               cursor.SetActive(false);
                anim.SetTrigger("throw");
              // Rigidbody obj=Instantiate(rb,goalkeeper.position,Quaternion.identity);
               async Task delaycreator()
                {
                   var t1=Task.Delay(1500);
                   await t1;
                    fb.transform.parent=default;
                    sphereCollider=fb.GetComponent<SphereCollider>();
                    sphereCollider.enabled=true;
                    meshCollider=fb.GetComponent<MeshCollider>();
                    meshCollider.enabled=true;
                    fb.GetComponent<Rigidbody>().useGravity=true;
                    fb.GetComponent<Rigidbody>().velocity=Vo;
               }
                delaycreator();
                isTouching=false;
           }
       }
       
       }
      // isshoot=false;
   }
  
   Vector3 CalculateVelocity(Vector3 target,Vector3 origin,float time){
      Vector3 distance =target - origin;
      Vector3 distanceXZ = distance;
      distanceXZ.y=0f;
      float Sy=distance.y;
      float Sxz=distanceXZ.magnitude;
      float Vxz= Sxz/time;
      float Vy=Sy/time+0.5f*Math.Abs(Physics.gravity.y)*time;
      Vector3 result=distanceXZ.normalized;
      result *= Vxz;
      result.y= Vy;
      return result;
   }
   private void OnCollisionEnter(Collision collision)
   {
      
       ContactPoint[] contacts = new ContactPoint[10];
       int numContacts = collision.GetContacts(contacts);
      var hash = new Hashtable();
          hash.Add("lastTouched",Team);
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
       for(int i=0;i<numContacts;i++)
       {
             float x=fb.transform.position.x;
            float z=fb.transform.position.z;
            
           if(Vector3.Distance(contacts[i].point,hand.position)<.2f&&Vector3.Distance(contacts[i].point,hand2.position)<.2f)
           {
               if(((x>48f&&z>-77f)||z>-47f||(x<-34f&&z>-77f)||(x>48f&&z>77f)||z>47f||(x<-34f&&z>77f))&&(x>-50f||x<64f))
            {
                var hash2 = new Hashtable();
                         hash2.Add("isHand",1);
                        PhotonNetwork.CurrentRoom.SetCustomProperties(hash2);
            }
            else
            {
                 if(!isPlaying("goalkeeper_idle")&&!isPlaying("throw"))
                isTouching=true;
               Debug.Log("hii2");
            }

           }
       }
       if(!isTouching)
       {
          Vector3 hitPos= collision.contacts[0].point - transform.position;
          hitPos = -hitPos.normalized;
        hitPos =Camera.main.ScreenToWorldPoint(hitPos);
       fb.transform.LookAt(hitPos);
       fb.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward*force, ForceMode.Impulse);
        Debug.Log(force);
       }
   }
   void Assigning()
   {
       playerName.text=PhotonNetwork.NickName;
       fb=GameObject.Find ("football-soccer-ball");
         Camerachanger=GameObject.Find ("camerachanger");
        cursor=GameObject.Find ("Circle");
      cr=cursor.GetComponent<Transform>();
        gm=FindObjectOfType<Gamemanager>();
   }
}


