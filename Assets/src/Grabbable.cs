using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class Grabbable : MonoBehaviour
{
    InputDevice left;
    InputDevice right;

    void Start()
    {

    }


    void Update()
    {
        // needs to be in Update
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        // left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        // more https://docs.unity3d.com/ScriptReference/XR.XRNode.html

        // assigns button value to out variable, if expecting Vector3 replace bool
        right.TryGetFeatureValue(CommonUsages.triggerButton, out bool isPressed);

        Debug.Log(isPressed);
    }
        

}
