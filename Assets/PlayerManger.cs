

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System.Data;
using Mono.Data.Sqlite;
public class PlayerManger : MonoBehaviourPunCallbacks
{
    public GameObject Gamemanager;
    public Buttonassignmanager bam;
    PhotonView PV;
    
    string RoomName;
    private string dbName="URI=file:Room.db";
    void Awake()
    {
        PV=GetComponent<PhotonView>();
    }
    void Start()
    {
        if(PV.IsMine)
        {
            bam=FindObjectOfType<Buttonassignmanager>();
            CreateController();
            
        }
        RoomName=PhotonNetwork.CurrentRoom.Name.ToString();
    }
    
   
    void CreateController()
    {
         Debug.Log(PhotonNetwork.NickName);
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
        int value=int.Parse(PhotonNetwork.LocalPlayer.CustomProperties["Button"].ToString());
       

       
        if(value==9)
       {
            Vector3 p = new Vector3(7f,17f,-68.09866f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","goalkeeper"),p,Quaternion.identity);
       }
       if(value==8)
       {
           Vector3 p = new Vector3(-37f,17f,-45f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerController"),p,Quaternion.identity);
       }
       if(value==7)
       {
           Vector3 p = new Vector3(7f,17f,-45f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerController"),p,Quaternion.identity);
       }
       if(value==6)
       {
           Vector3 p = new Vector3(51f,17f,-45f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerController"),p,Quaternion.identity);
       }
       if(value==5)
       {
           Vector3 p = new Vector3(-37f,17f,-25f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerController"),p,Quaternion.identity);
       }
       if(value==4)
       {
           Vector3 p = new Vector3(7f,17f,-25f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerController"),p,Quaternion.identity);
       }
       if(value==3)
       {
           Vector3 p = new Vector3(51f,17f,-25f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerController"),p,Quaternion.identity);
       }
       if(value==2)
       {
           Vector3 p = new Vector3(-37f,17f,-5f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerController"),p,Quaternion.identity);
       }
       if(value==1)
       {
           Vector3 p = new Vector3(7f,17f,-5f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerController"),p,Quaternion.identity);
       }
       if(value==0)
       {
           Vector3 p = new Vector3(51f,17f,-5f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerController"),p,Quaternion.identity);
       }
       if(value==10)
       {
           Vector3 p = new Vector3(-37f,17f,5f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerController1"),p,Quaternion.identity);
       }
       if(value==11)
       {
           Vector3 p = new Vector3(7f,17f,5f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerController1"),p,Quaternion.identity);
       }
       if(value==12)
       {
           Vector3 p = new Vector3(51f,17f,5f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerController1"),p,Quaternion.identity);
       }
       if(value==13)
       {
           Vector3 p = new Vector3(-37f,17f,25f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerController1"),p,Quaternion.identity);
       }
       if(value==14)
       {
           Vector3 p = new Vector3(7f,17f,25f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerController1"),p,Quaternion.identity);
       }
       if(value==15)
       {
           Vector3 p = new Vector3(51f,17f,25f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerController1"),p,Quaternion.identity);
       }
       if(value==16)
       {
           Vector3 p = new Vector3(-37f,17f,45f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerController1"),p,Quaternion.identity);
       }
       if(value==17)
       {
           Vector3 p = new Vector3(7f,17f,45f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerController1"),p,Quaternion.identity);
       }
       if(value==18)
       {
           Vector3 p = new Vector3(51f,17f,45f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerController1"),p,Quaternion.identity);
       }
        if(value==19)
       {
            Vector3 p = new Vector3(7f,17f,68.09866f);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","goalkeeper1"),p,Quaternion.identity);
       }
    }
}
