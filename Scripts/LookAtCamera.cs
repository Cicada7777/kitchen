using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode
    {
        Lookat,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted,
    }

    [SerializeField] private Mode mode;
    private void LateUpdate() //after the regular update 
    {
        switch(mode)
        {
            case Mode.Lookat:
                transform.LookAt(Camera.main.transform); //make this transform look at another transform or another point 
                break;
            case Mode.LookAtInverted:
                //the direction Vector from the camera to this object 
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position +dirFromCamera);
                break;
            case Mode.CameraForward: //regarde droit 
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}
