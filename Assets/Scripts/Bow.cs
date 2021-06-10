using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public Camera camera;
    public Transform leftHand;
    public Transform RightHand;
    public Transform BowString;
    public Transform BowMain;
    private Vector3 location;
    private Vector3 rotation;
    private Animator animator;
    public GameObject arrowPrefab;
    private GameObject arrow;
    private System.DateTime AimTime;
    // Start is called before the first frame update
    void Start()
    {
        location = BowString.transform.localPosition;
        rotation = BowString.transform.localEulerAngles;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void GrabString()
    {
        BowString.parent = RightHand;
        AimArrow();
    }

    public void ConcelAiming()
    {
        BowString.parent = BowMain;
        BowString.transform.localPosition = location;
        BowString.transform.localEulerAngles = rotation;
        if(arrow != null) Destroy(arrow);
    }

    public void ReleaseString()
    {
        BowString.parent = BowMain;
        BowString.transform.localPosition = location;
        BowString.transform.localEulerAngles = rotation;
        animator.SetBool("Shoot", false);
        ShootArrow();
    }

    public void GetArrow()
    {
        arrow = Instantiate(arrowPrefab) as GameObject;
        arrow.transform.parent = RightHand;
        arrow.transform.localPosition = new Vector3(-0.00124f, 0.0109f, 0.00029f);
        arrow.transform.localEulerAngles = new Vector3(-96.224f, 90, -90);
        arrow.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        
    }

    public void AimArrow()
    {
        //arrow.transform.LookAt(leftHand.up*10);
        AimTime = System.DateTime.UtcNow;
    }

    public void ShootArrow()
    {
        RaycastHit hit;

        arrow.transform.parent = null;
        arrow.transform.position += transform.parent.transform.forward;
        Rigidbody rb = arrow.AddComponent<Rigidbody>();
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 100f))
        {
            rb.velocity = CalculateTrajectoryVelocity(arrow.transform.position, hit.point, hit.distance/50f);
        }
        else
        {
            rb.velocity = camera.transform.forward * Mathf.Max(50, (System.DateTime.UtcNow - AimTime).Milliseconds / 1000f * 10f);
            arrow.transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        
        //arrow.transform.localPosition = new Vector3(0f, 0f, 0f);
        //arrow.transform.localEulerAngles = new Vector3(0, 0, 0);
        arrow.AddComponent<Arrow>();
        arrow.GetComponent<TrailRenderer>().enabled = true;
        arrow = null;
    }

    Vector3 CalculateTrajectoryVelocity(Vector3 origin, Vector3 target, float t)
    {
        float vx = (target.x - origin.x) / t;
        float vz = (target.z - origin.z) / t;
        float vy = ((target.y - origin.y) - 0.5f * Physics.gravity.y * t * t) / t;
        return new Vector3(vx, vy, vz);
    }

    /*public static Vector3 FirstOrderIntercept
(
    Vector3 shooterPosition,
    Vector3 shooterVelocity,
    float shotSpeed,
    Vector3 targetPosition,
    Vector3 targetVelocity
)
    {
        Vector3 targetRelativePosition = targetPosition - shooterPosition;
        Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
        float t = FirstOrderInterceptTime
        (
            shotSpeed,
            targetRelativePosition,
            targetRelativeVelocity
        );
        return targetPosition + t * (targetRelativeVelocity);
    }
    //first-order intercept using relative target position
    public static float FirstOrderInterceptTime
    (
        float shotSpeed,
        Vector3 targetRelativePosition,
        Vector3 targetRelativeVelocity
    )
    {
        float velocitySquared = targetRelativeVelocity.sqrMagnitude;
        if (velocitySquared < 0.001f)
            return 0f;

        float a = velocitySquared - shotSpeed * shotSpeed;

        //handle similar velocities
        if (Mathf.Abs(a) < 0.001f)
        {
            float t = -targetRelativePosition.sqrMagnitude /
            (
                2f * Vector3.Dot
                (
                    targetRelativeVelocity,
                    targetRelativePosition
                )
            );
            return Mathf.Max(t, 0f); //don't shoot back in time
        }

        float b = 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
        float c = targetRelativePosition.sqrMagnitude;
        float determinant = b * b - 4f * a * c;

        if (determinant > 0f)
        { //determinant > 0; two intercept paths (most common)
            float t1 = (-b + Mathf.Sqrt(determinant)) / (2f * a),
                    t2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);
            if (t1 > 0f)
            {
                if (t2 > 0f)
                    return Mathf.Min(t1, t2); //both are positive
                else
                    return t1; //only t1 is positive
            }
            else
                return Mathf.Max(t2, 0f); //don't shoot back in time
        }
        else if (determinant < 0f) //determinant < 0; no intercept path
            return 0f;
        else //determinant = 0; one intercept path, pretty much never happens
            return Mathf.Max(-b / (2f * a), 0f); //don't shoot back in time
    }*/
}
