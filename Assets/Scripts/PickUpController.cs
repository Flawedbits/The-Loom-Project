using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    [Header("Pickup Settings")]
    public static Transform holdArea;
    public static Transform holdAreaNormal;
    [SerializeField] Transform holdAreaProblem;
    [SerializeField] Transform player;
    [SerializeField] LayerMask pickupLayerMask; // Define the layer mask
    private static GameObject heldObj;
    private static Rigidbody heldObjRB;

    [Header("Pickup Parameters")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 10.0f;
    [SerializeField] private float maxThrowingForce = 100.0f;
    public float droppingDistance = 1f;
    Vector3 localForward;
    Vector3 localUp;
    public static bool beingHeld = false;
    public static bool playHard = false;
    public static bool DONT = false;
    public float maxVelocityMagnitude = 80.0f;

    private void Start()
    {
        holdArea = holdAreaNormal;
    }

    private void Update()
    {
        if (Input.GetKeyDown(CustomInputManager.IM.grab))
        {
            if (heldObj == null)
            {
                RaycastHit hit;
                // Use layer mask to exclude the player's layer
                if (Physics.Raycast(transform.position, transform.forward, out hit, pickupRange, pickupLayerMask))
                {
                    PickupObject(hit.transform.gameObject);
                }
            }
            else
            {
                DropObject();
            }
        }
    }

    private void FixedUpdate()
    {
        if (heldObj != null)
        {
            MoveObject();
        }
    }

    void MoveObject()
    {
        if (DONT)
        {
            if (Vector3.Distance(player.position, heldObjRB.transform.position) > 1.1f)
            {
                DONT = false;
            }
        }
        if (!DONT)
        {
            if (Vector3.Distance(player.position, heldObjRB.transform.position) < 0.85f)
            {
                holdArea = holdAreaProblem;
            }
        }

        Vector3 forward = transform.TransformDirection(localForward);
        Vector3 up = transform.TransformDirection(localUp);
        heldObj.transform.rotation = Quaternion.LookRotation(forward, up);

        Vector3 diff = holdArea.position - heldObjRB.position;
        Vector3 velocity = diff / Time.fixedDeltaTime;

        if (velocity.magnitude > maxVelocityMagnitude)
        {
            velocity = velocity.normalized * maxVelocityMagnitude; // Clamp velocity magnitude
        }

        heldObjRB.linearVelocity = velocity;

        if (Vector3.Distance(holdArea.position, heldObjRB.transform.position) > droppingDistance)
        {
            DropObject();
        }
    }

    void PickupObject(GameObject pickObj)
    {
        if (pickObj.GetComponent<Rigidbody>())
        {
            heldObjRB = pickObj.GetComponent<Rigidbody>();
            heldObjRB.useGravity = false;
            heldObjRB.linearDamping = 10;
            heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;
            pickObj.layer = 5;
            beingHeld = true;

            heldObj = pickObj;
            localForward = transform.InverseTransformDirection(heldObj.transform.forward);
            localUp = transform.InverseTransformDirection(heldObj.transform.up);
            Physics.IgnoreLayerCollision(3, 5, true);
        }
    }

    public static void DropObject()
    {
        if(!DONT){
            heldObjRB.useGravity = true;
            heldObjRB.linearDamping = 1;
            heldObjRB.constraints = RigidbodyConstraints.None;
            heldObj.layer = 0;
            beingHeld = false;

            heldObj = null;
            holdArea = holdAreaNormal;
            Physics.IgnoreLayerCollision(3, 5, false);
        }
    }
}