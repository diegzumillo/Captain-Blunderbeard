using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class GrabbingHandTrigger : MonoBehaviour
{
    
    //Possible Improvements:
    //
    //  Add a cutoff distance for dropping items as well as max force. If the distance between heldobject 
    //  and this hand is bigger than 
    //  a predetermined value, it drops the object. Feels natural, as it will happen with fast movements or heavy objects.
    //
    //  Apply torque to keep object at same rotation as hand. (easy hack is to add two more forces around the same point)
    //  At the moment the player can still manipulate the rotation using his second hand naturally.

    public bool rightHand = false;
    public bool grabTrigger = false;
    public float springForce = 100.0f;
    public float springDamp = 1.0f;
    InputDevice hand;
    
    private SpringJoint joint;
    public Rigidbody objectHeld;
    private Vector3 grabbingPoint; //in objectheld's local space
    

    void Start()
    {
        //not needed yet
    }


    void Update()
    {
        // needs to be in Update because of reasons
        if(rightHand)
            hand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        else
            hand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        
        
        hand.TryGetFeatureValue(CommonUsages.gripButton, out grabTrigger);
        
        if(grabTrigger){
            if(objectHeld != null){
                //update anchor point                
                // joint.connectedAnchor = transform.position;
                // joint.anchor = joint.transform.InverseTransformPoint(transform.position);
                Vector3 grabbingPointWorld = objectHeld.transform.TransformPoint(grabbingPoint);
                Vector3 force = springForce * (transform.position - grabbingPointWorld );
                 //real spring damp would use relative velocity but this works fine here
                force -= springDamp * objectHeld.velocity;
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
                // Set the objectHeld to the other Rigidbody
                objectHeld = otherRb;

                //I left out this old code commented for comparison. Dealing with joits can be
                //more trouble than its worth.
                
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
