using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
public class PlayerListItem : MonoBehaviourPunCallbacks
{
     Launcher launcher;
   [SerializeField] TMP_Text text;
   Player player;
   TMP_Text[] PlayerButton;
   TMP_Text[] CopyPlayerButton;
   public Hashtable playerButtons = new Hashtable();
  // PlayerButton=Launcher.PlayerListAssign;
   public void SetUp(Player _player)
   {
      player=_player;
      text.text=_player.NickName;
   }
   void Update()
   {
       //PlayerButton=launcher.PlayerListAssign;
        //CopyPlayerButton=launcher.copyPlayerListAssign;
        //playerAssignedButtons=launcher.playerButtons;
   }
   public override void OnPlayerLeftRoom(Player otherPlayer)
   {
       
              
      if(player==otherPlayer)
       {
          
               Destroy(gameObject);
       }
   }
   public override void OnLeftRoom()
   {
      
               Destroy(gameObject);
   }
}
