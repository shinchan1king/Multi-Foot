using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;
using Photon.Voice.PUN;

public class pushtotalk : MonoBehaviourPun
{
     KeyCode PushButton=KeyCode.U;
     KeyCode PushButton2=KeyCode.O;
    public Recorder VoiceRecorder;
    private PhotonView view;
    // Start is called before the first frame update
    void Start()
    {
        view = photonView;
        VoiceRecorder.TransmitEnabled=false;
    }

    
    void Update()
    {
         if (Input.GetKeyDown(PushButton))
        {
            if (view.IsMine)
            {
                VoiceRecorder.TransmitEnabled = true;
            }
        }
         if (Input.GetKeyDown(PushButton2))
        {
            if (view.IsMine)
            {
                VoiceRecorder.TransmitEnabled = false;
            }
        }  
    }
}
