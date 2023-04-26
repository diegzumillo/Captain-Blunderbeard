using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class GrabbingHandTrigger : MonoBehaviour
{
    
    
    public bool rightHand = false;
    public bool grabTrigger = false;
    public float springForce = 100.0f;
    InputDevice hand;
    
    private SpringJoint joint;
    public Rigidbody objectHeld;
    private Vector3 grabbingPoint; //in objectheld's local space
    

    void Start()
    {

    }


    void Update()
    {
        // needs to be in Update
        if(rightHand)
            hand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        else
            hand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        // left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        // more https://docs.unity3d.com/ScriptReference/XR.XRNode.html

        // assigns button value to out variable, if expecting Vector3 replace bool
        hand.TryGetFeatureValue(CommonUsages.triggerButton, out grabTrigger);
        
        if(grabTrigger){
            if(objectHeld != null){
                //update anchor point                
                // joint.connectedAnchor = transform.position;
                // joint.anchor = joint.transform.InverseTransformPoint(transform.position);
                Vector3 grabbingPointWorld = objectHeld.transform.TransformPoint(grabbingPoint);
                Vector3 force = springForce * (transform.position - grabbingPointWorld );
                objectHeld.AddForceAtPosition(force, grabbingPointWorld);
                
            }
        }else{
            //if holding an object, release it
            // Remove the FixedJoint component from the grabbed object
            if(objectHeld != null){
                Destroy(joint);                

                // Reset the grabbedObject and fixedJoint variables
                objectHeld = null;
                joint = null;
            }
            
        }

    }        


    private void OnTriggerStay(Collider other)
    {
        if(grabTrigger && objectHeld == null){
            print(other.gameObject.name);
            // Check if the other collider has a Rigidbody component
            Rigidbody otherRb = other.GetComponent<Rigidbody>();
            if (otherRb != null)
            {
                print("grabbing an object");
                // Set the grabbedObject to the other Rigidbody
                objectHeld = otherRb;

                // Create a new FixedJoint component on the grabbed object
                //joint = objectHeld.gameObject.AddComponent<SpringJoint>();

                // // Set the anchor position of the joint to the position of the hand                
                // joint.anchor = other.transform.InverseTransformPoint(transform.position);
                // joint.connectedAnchor = transform.position;
                // joint.autoConfigureConnectedAnchor = true;

                grabbingPoint = other.transform.InverseTransformPoint(transform.position);
                
            }
        }

        
    }

    
}
