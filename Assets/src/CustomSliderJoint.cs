using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSliderJoint : MonoBehaviour
{

    public AnimationCurve potentialCurve;
    public AnimationCurve forceCurve;
    public Vector3 pointA;
    public Vector3 pointB;
    public float forceScale;
    public Vector3 axis;
    private Vector3 initialPosition;
    public float positionOnAxis;
    Rigidbody rb;

    public static AnimationCurve DifferentiateAnimationCurve(AnimationCurve curve, float delta)
    {
        AnimationCurve derivativeCurve = new AnimationCurve();

        for (int i = 0; i < curve.length - 1; i++)
        {
            float time = curve.keys[i].time;
            float value = curve.Evaluate(time);

            float dx = delta;

            float value1 = curve.Evaluate(time - dx);
            float value2 = curve.Evaluate(time + dx);

            float slope = (value2 - value1) / (2 * dx);

            Keyframe keyframe = new Keyframe(time, slope);
            derivativeCurve.AddKey(keyframe);
        }

        return derivativeCurve;
    }



    void CalculatePositionOnAxis(){
        // Vector3 curPos = Vector3.Project( transform.position, axis.normalized );
        // positionOnAxis = Vector3.Distance(curPos, initialPosition);
        positionOnAxis = Mathf.InverseLerp(0f, (pointB-pointA).magnitude, Vector3.Dot(rb.position - pointA, axis));

    }

    

    void Start()
    {
        forceCurve = DifferentiateAnimationCurve(potentialCurve, 0.1f);
        axis = pointB - pointA;

        rb = this.GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        initialPosition = Vector3.Project( transform.position, axis.normalized );
        
    }

    void FixedUpdate()
    {

        CalculatePositionOnAxis();
        float forceMag = -forceScale*forceCurve.Evaluate(positionOnAxis);
        rb.AddForce( forceMag*axis.normalized );        
        rb.velocity = Vector3.Project(rb.velocity, axis.normalized); //I didn't know vector3 had this function
    }
}

