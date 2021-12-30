using System.IO;
using System.Net.Http.Headers;
using System.Collections.ObjectModel;
using System.Threading;
using System.Xml.Linq;
//using System.Diagnostics;
//using System;
using System.Data;
using Mono.Data.Sqlite;

using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using static Photon.Realtime.WebFlags;
using ExitGames.Client.Photon;
//using UnityEngine.Networking;

public class Launcher : MonoBehaviourPunCallbacks
{ 
    public static Launcher Instance;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] TMP_Text PlayerNameText;

    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContent;
    public PlayerListItem playerListItemPrefab;
    [SerializeField] GameObject startGameButton;
    [SerializeField] TMP_Text[] PlayerListAssign;
   
     [SerializeField]  Hashtable playerButtons = new Hashtable();
   [SerializeField] TMP_Text[] copyPlayerListAssign;
    int PlayerNo;
    bool PlayerName = false;
    private string dbName="URI=file:Room.db";
    
    bool enteredroom =false;
    public GameObject buttonAssign;
    public Buttonassignmanager bam;
    PhotonView PV;
    void Awake()
    {
        Instance=this;
        PV=GetComponent<PhotonView>();
    }
    void Start()
    {

        Debug.Log("Conecting..");
        PhotonNetwork.ConnectUsingSettings();
       // copyPlayerListAssign=PlayerListAssign;
       if(!PV.IsMine)
       {
           Destroy(GetComponentInChildren<Camera>().gameObject);
       }
    }
 
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene=true;
    }
    public override void OnJoinedLobby()
    {
        
        menumanager.Instance.OpenMenu("Title");
        
        Debug.Log("Joined Lobby");
        var hash = new Hashtable();
        hash.Add("Button",null);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        if(!PlayerName)
        {
            PhotonNetwork.NickName="Player"+Random.Range(0,1000).ToString("0000");
            PlayerName=true;
        }
    }
    public void CreateRoom()
    {

        if(string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
       string RoomName=roomNameInputField.text.ToString();

        menumanager.Instance.OpenMenu("Loading");
    }
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        menumanager.Instance.OpenMenu("Loading");
        
    }
    public override void OnJoinedRoom()
    {
        PlayerNo=0;
        Debug.Log("Joined Room");
         menumanager.Instance.OpenMenu("room");
         roomNameText.text=PhotonNetwork.CurrentRoom.Name;
       
         Player[] players = PhotonNetwork.PlayerList;
         enteredroom=true;
       //  foreach (Transform trans in playerListContent)
        //{
        //    Destroy(trans.gameObject);
        //}
        
         foreach(Player v in players)
        {
            if(v.CustomProperties["Button"]==null)
            {for(int i=0;i<20;i++)
            {
                
                if(PlayerListAssign[i].text=="Attacker1"||PlayerListAssign[i].text=="Attacker2"||PlayerListAssign[i].text=="Attacker3"||PlayerListAssign[i].text=="MidFielder1"||PlayerListAssign[i].text=="MidFielder2"||PlayerListAssign[i].text=="MidFielder3"||PlayerListAssign[i].text=="Defender1"||PlayerListAssign[i].text=="Defender2"||PlayerListAssign[i].text=="Defender3"||PlayerListAssign[i].text=="Goalkeeper")
                {
                    PlayerListAssign[i].text=v.NickName;

                    var hash = new Hashtable();
                        hash.Add("Button",i);
                        v.SetCustomProperties(hash);
                    break;
                }
            }
            }
            else
            {
                PlayerListAssign[int.Parse(v.CustomProperties["Button"].ToString())].text=v.NickName;
            }
            //Instantiate(playerListItemPrefab,playerListContent).GetComponent<PlayerListItem>().SetUp(v);
            PlayerNo++;
            
        
        
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        }
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnCreateRoomFailed(short returncode,string message)
    {
        errorText.text="Room Creation Failed"+ message;
        menumanager.Instance.OpenMenu("error");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        menumanager.Instance.OpenMenu("Loading");
    }
    public override void OnLeftRoom()
    {
         menumanager.Instance.OpenMenu("Title");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("room List updated");
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for(int i=0;i<roomList.Count;i++)
        {
            if(roomList[i].RemovedFromList)
             continue;
            Instantiate(roomListItemPrefab,roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
        
        enteredroom=true;
        
        for(int i=0;i<20;i++)
            {
                if(PlayerListAssign[i].text=="Attacker1"||PlayerListAssign[i].text=="Attacker2"||PlayerListAssign[i].text=="Attacker3"||PlayerListAssign[i].text=="MidFielder1"||PlayerListAssign[i].text=="MidFielder2"||PlayerListAssign[i].text=="MidFielder3"||PlayerListAssign[i].text=="Defender1"||PlayerListAssign[i].text=="Defender2"||PlayerListAssign[i].text=="Defender3"||PlayerListAssign[i].text=="Goalkeeper")
                {
                    PlayerListAssign[i].text=newPlayer.NickName;
                   var hash = new Hashtable();
                        hash.Add("Button",i);
                        newPlayer.SetCustomProperties(hash);
                    break;
                }
            }
        
        //Instantiate(playerListItemPrefab,playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }
    public override void OnPlayerLeftRoom(Player  player) 
    {
        enteredroom=false;
        int i=int.Parse(player.CustomProperties["Button"].ToString());
        PlayerListAssign[i].text=copyPlayerListAssign[i].text;
        player.CustomProperties["Button"]=null;
    }
    void Update()
    {
        if(!PV.IsMine)
       {
           return;
       }
        if(enteredroom)
        {
           
            updateList();
        }
    }
    public void updateList()
    {
        int count=0;
        int[] assigned=new int[19];
        foreach(Player p in PhotonNetwork.PlayerList)
        {
               
                PlayerListAssign[int.Parse(p.CustomProperties["Button"].ToString())].text=p.NickName;
                assigned[count]=int.Parse(p.CustomProperties["Button"].ToString());
                count++;
        }
        for(int i=0;i<=19;i++)
        {
            bool isAssigned=false;
            for(int j=0;j<count;j++)
            {
                if(i==assigned[j])
                {
                    isAssigned=true;
                }
            }
            if(!isAssigned)
            {
                PlayerListAssign[i].text=copyPlayerListAssign[i].text;
            }
        }

    }
    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
    public void  SubmitName() {
        PlayerName=true;
        PhotonNetwork.NickName=PlayerNameText.text+Random.Range(0,1000).ToString("0000");
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        
        if(changedProps["Button"]!=null)
            {for(int i=0;i<20;i++)
            {
                if(PlayerListAssign[i].text.ToString()==targetPlayer.NickName.ToString())
                {
                    if(i!=int.Parse(changedProps["Button"].ToString()))
                    {
                        PlayerListAssign[i].text=copyPlayerListAssign[i].text;
                        PlayerListAssign[int.Parse(changedProps["Button"].ToString())].text=targetPlayer.NickName;
                    }
                }
            }
        }
    }
    public void UpdateText(GameObject buttonObj )
    {

       TMP_Text text1 =buttonObj.GetComponentInChildren<TMP_Text>(true);
       
       
        if(text1.text=="GoalKeeper"||text1.text=="Attacker1"||text1.text=="Attacker2"||text1.text=="Attacker3"||text1.text=="MidFielder1"||text1.text=="MidFielder2"||text1.text=="MidFielder3"||text1.text=="Defender1"||text1.text=="Defender2"||text1.text=="Defender3")
            {

                    int i=int.Parse(PhotonNetwork.LocalPlayer.CustomProperties["Button"].ToString());
                        PlayerListAssign[i].text=copyPlayerListAssign[i].text;
                        var hash = new Hashtable();
                        hash.Add("Button",int.Parse(buttonObj.name));
                        PhotonNetwork.SetPlayerCustomProperties(hash);
                        PlayerListAssign[int.Parse(buttonObj.name)].text=PhotonNetwork.NickName;


            }

    }
}
