using Photon.Pun;
using System.Threading.Tasks;
using System;

using UnityEngine;
using ExitGames.Client.Photon;
using TMPro;
public class PlayerController :MonoBehaviourPunCallbacks
{
   
    float speed=5.0f;
    float rotationSpeed=100.0f;
    Animator anim;
    float corotate;
    float force=12.5f;
    float rotation;
    float translation;
    public Transform hand;
    public Transform hand2;
    public GameObject fb;
  public GameObject cursor;
    public LayerMask layer;
   private Camera cam;
    public GameObject Camerachanger;
    public Transform cm;
    public Transform cr;
    private SphereCollider sphereCollider;
    private MeshCollider meshCollider;
   string sName;
   Rigidbody rb;
   PhotonView PV;
    GameObject ballInstance;
    private Gamemanager Gm;
    public football fball;
    public int Team;
    public bool isThrowIn=false;
    public bool isShoot=false;
    public bool throwInDone=false;
    public bool throwInDone2=false;
    public GameObject newBall;
     bool assignComponents=false;
    int count=0;
    public TMP_Text playerName;
    void Awake()
    {
        
        PV=GetComponent<PhotonView>();
    }
    void Start()
    {
        anim=this.GetComponent<Animator>();
       cam=Camera.main;
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
       Gm = FindObjectOfType<Gamemanager>();
       Camerachanger=GameObject.Find ("camerachanger");
       cursor=GameObject.Find ("Circle");
       cr=cursor.GetComponent<Transform>();
       var hash2 = new Hashtable();
       hash2.Add("isHand",null);
       PhotonNetwork.CurrentRoom.SetCustomProperties(hash2);
       var hash = new Hashtable();
       hash.Add("lastTouched",null);
       PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    }
   
    // Update is called once per frame
    void Update()
    {
       float right_boundary=-52f;
       float left_boundary=66f;
       float up_boundary=86f;
       float down_boundary=-86f;
       if(!PV.IsMine)
       {
           return;
       }
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
     
      
         
       Debug.Log(Team);
         translation=Input.GetAxis("Vertical")*speed;
         rotation=Input.GetAxis("Horizontal")*rotationSpeed;
       
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        corotate=rotation;
        transform.Translate(0, 0, translation);
        transform.Rotate(0,rotation,0);

        if(translation != 0&&!isThrowIn)
        {
            anim.SetBool("isWalking",true);
            anim.SetFloat("characterSpeed",translation);
            
        }
        else
        {
            anim.SetBool("isWalking",false);
             anim.SetFloat("characterSpeed",0);
            
        }
        
        if(Input.GetKeyDown("space"))
        {
            anim.SetTrigger("jumpHeader");
            force=25.0f;
            sName="jump_header";
        }
        if(Input.GetKeyDown("v"))
        {
            anim.SetTrigger("isSliding");
            force =35.0f;
            sName="slide";
        }
        if(Input.GetKeyDown("c"))
        {
            anim.SetTrigger("Chip");
            force=30.0f;
            sName="chip";
        }
         if(Input.GetKeyDown("y"))
        {
            anim.SetTrigger("cycleKick");

            force=45.0f;
            sName="cycleKick";

        }
          if(Input.GetKeyDown("p"))
        {
            anim.SetTrigger("soccerPass");
            force=30.00f;
            sName="soccerPass";
        }
        if(isThrowIn)
        {

           isShoot=true;
            sphereCollider=fb.GetComponent<SphereCollider>();
            sphereCollider.enabled=false;
            meshCollider=fb.GetComponent<MeshCollider>();
            meshCollider.enabled=false;
            fb.transform.GetComponent<Rigidbody>().useGravity=false;
            
            fb.transform.parent=hand;
           // target.parent=hand2;
            fb.transform.localPosition=new Vector3(0f,0.1f,0.19f);
            //goalkeeper.localPosition=new Vector3(x,16.8f,z);
            fb.transform.localRotation=Quaternion.Euler(154.537f,64.417f,39.14799f);
           LaunchProjectile();

        }
        if(Input.GetKeyDown("r"))
        {
            anim.SetTrigger("recieveSoccer");
        }
        
        if(!isPlaying(sName))
        {
            force=12.5f;
        }
        if(throwInDone)
        {
               anim.SetTrigger("idle");
               throwInDone=false;
               sphereCollider=fb.GetComponent<SphereCollider>();
            sphereCollider.enabled=true;
            meshCollider=fb.GetComponent<MeshCollider>();
            meshCollider.enabled=true;
            fb.transform.GetComponent<Rigidbody>().useGravity=true;

            fb.transform.parent=default;
            async Task delaycreator()
            {
                var t1=Task.Delay(1500);
                    await t1;
                throwInDone2=false;
            }
            delaycreator();
        }

   /* if (!photonView.IsMine)
            {
                RefreshMultiplayerState();
                return;
            }*/
    }
   
    private void OnCollisionEnter(Collision collision)
   {
       if(collision.gameObject.name == "football-soccer-ball")
        {
            var hash = new Hashtable();
          hash.Add("lastTouched",Team);
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
            ContactPoint[] contacts = new ContactPoint[10];
            int numContacts = collision.GetContacts(contacts);
            for(int i=0;i<numContacts;i++)
            {
                if((Vector3.Distance(contacts[i].point,hand.position)<.2f||Vector3.Distance(contacts[i].point,hand2.position)<.2f)&&!isThrowIn)
                {
                    if(!throwInDone2)
                    {
                        var hash2 = new Hashtable();
                         hash2.Add("isHand",1);
                        PhotonNetwork.CurrentRoom.SetCustomProperties(hash2);
                    }
                    Debug.Log("Hand Touched");
                }
                else
                {
                    Vector3 hitPos= collision.transform.position - transform.position;
                    hitPos = hitPos.normalized;
                    hitPos =Camera.main.ScreenToWorldPoint(hitPos);
                    fb.transform.LookAt(hitPos);
                    fb.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward*force, ForceMode.Impulse);
                    
                }
            }
       }
      
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
      
       Ray camRay =cam.ScreenPointToRay(Input.mousePosition);
       
       RaycastHit hit;
       if(isShoot)
       { anim.SetTrigger("throwIn");
           if(Physics.Raycast(camRay,out hit,100f,layer))
        {
           cursor.SetActive(true);
           cursor.transform.position=hit.point+Vector3.up*0.1f;
           Vector3 Vo = CalculateVelocity(hit.point,hand.position,1f);
           if(Input.GetMouseButtonDown(0))
           {
                cursor.SetActive(false);
                anim.SetTrigger("throw");
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
                isShoot=false;
                throwInDone=true;
                
                Gm.throwIn=false;
                    isThrowIn=false;
                    throwInDone2=true;
               
           }
       }
       else
       {
          cursor.SetActive(false);
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
    void Assigning()
   {
       playerName.text=PhotonNetwork.NickName;
       fb=GameObject.Find ("football-soccer-ball");
         Camerachanger=GameObject.Find ("camerachanger");
        cursor=GameObject.Find ("Circle");
      cr=cursor.GetComponent<Transform>();
        Gm=FindObjectOfType<Gamemanager>();
   }
}
