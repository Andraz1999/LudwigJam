using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraPriority : MonoBehaviour
{
    private enum CameraP
    {
        narrow,
        wide
    }
    [SerializeField] CinemachineVirtualCamera narrowCam;
    [SerializeField] CinemachineVirtualCamera wideCam;

    [SerializeField] CameraP cameraP;
    

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            switch (cameraP)
            {
                case CameraP.narrow:
                narrowCam.Priority = 10;
                wideCam.Priority = 5;
                break;
                case CameraP.wide:
                narrowCam.Priority = 5;
                wideCam.Priority = 10;
                break;
            }
        }
    }
}
