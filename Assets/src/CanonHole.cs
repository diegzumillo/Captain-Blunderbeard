using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonHole : MonoBehaviour
{
    //canon hole does two things: it receives a canon ball, if it's empty. 
    //it fires a ball (must be called externally)
    //
    // The most pressing matter with this code is the loaded ball. It's squashed because it just gets
    // reparented to a scaled object. It looks ugly but meh..


    public bool loaded = false;
    public float recoilForce = 30.0f; //could make this proportional to ball's mass.
    private Rigidbody ball;
    public Rigidbody cannonBody;
    public HingeJoint cannonTrigger;

    public float triggerAngleOffset = 0;
    public float triggerCurAngle = 0.0f;

    public float test = 0.0f;
    public float testx = 0.0f;
    public float testy = 0.0f;
    public float testz = 0.0f;

    void Start(){
        //a hack to get around a bug in unity, where hinge angle is not relative to its neutral state
        triggerAngleOffset = Vector3.Angle(transform.up, cannonTrigger.transform.forward) ;
    }


    // Update is called once per frame
    void Update()
    {
        //for testing purposes only.
        if(Input.GetKey(KeyCode.Space ) && loaded){
            Fire();
        }
        test = Vector3.Angle(transform.up, cannonTrigger.transform.forward) -triggerAngleOffset ;
        testx = transform.localEulerAngles.x;
        testy = transform.localEulerAngles.y;
        testz = transform.localEulerAngles.z;

        if(loaded && test < -25.0f){
            Fire();
        }
        
    }

    void Load(Collider other){
        print("Canon loaded, captain");
        loaded = true;
        ball = other.GetComponent<Rigidbody>();
        ball.isKinematic = true; //stop moving and being a physical object for a moment, please.
        ball.transform.parent = transform; //also follow me
    }

    public void Fire(){
        print("FIREEEEEE!");
        loaded = false;
        ball.transform.parent = null; //BE FREE MY SON
        //a small distance ahead so it doesn't get reloaded
        ball.transform.position = transform.position + transform.forward * 1.0f; 
        ball.isKinematic = false;        
        ball.velocity = transform.forward * 10.0f;
        ball = null;

        //apply recoil to canon:
        cannonBody.AddForce( - transform.forward * recoilForce, ForceMode.Impulse );
        //Debug.Break();

    }

    private void OnTriggerEnter(Collider other){
        if(!loaded && other.tag == "Ball"){
            Load(other);
        }
    }

}
