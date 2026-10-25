using UnityEngine;
using Fragsurf.Movement;

public class PickUpController : MonoBehaviour
{
    [SerializeField] private Transform holdArea;
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private float pickupRange = 5f;
    [SerializeField, Min(0.1f)] private float maxHoldDistance = 3.5f;
    [SerializeField, Min(0f)] private float maxHoldDistanceGracePeriod = 0.75f;

    private static Rigidbody heldObject;
    private static PhysicsFollow heldObjectFollow;
    private static float holdDistanceExceededTime;
    private static Transform heldPlayerRoot;
    private static bool overlapCheckWarningLogged;

    public static bool beingHeld { get; private set; }

    private void Update()
    {
        if (!Input.GetKeyDown(CustomInputManager.IM.grab))
        {
            return;
        }

        if (heldObject != null)
        {
            DropObject();
            return;
        }

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, pickupRange, pickupLayerMask))
        {
            Rigidbody targetRigidbody = hit.rigidbody;
            if (targetRigidbody != null)
            {
                PickupObject(targetRigidbody);
            }
        }
    }

    private void FixedUpdate()
    {
        if (heldObject == null)
        {
            if (beingHeld)
            {
                DropObject();
            }

            return;
        }

        Vector3 playerPosition = transform.root.position;
        if (holdArea == null ||
            (heldObject.worldCenterOfMass - playerPosition).sqrMagnitude > maxHoldDistance * maxHoldDistance)
        {
            holdDistanceExceededTime += Time.fixedDeltaTime;
            if (holdDistanceExceededTime >= maxHoldDistanceGracePeriod)
            {
                DropObject();
            }
        }
        else
        {
            holdDistanceExceededTime = 0f;
        }
    }

    private void PickupObject(Rigidbody targetRigidbody)
    {
        if (holdArea == null)
        {
            Debug.LogError("PickUpController requires a hold area to follow.", this);
            return;
        }

        heldObject = targetRigidbody;
        heldPlayerRoot = transform.root;
        heldObjectFollow = heldObject.gameObject.AddComponent<PhysicsFollow>();
        heldObjectFollow.Initialize(
            holdArea,
            heldObject,
            heldPlayerRoot.GetComponentsInChildren<Collider>(true));
        holdDistanceExceededTime = 0f;
        overlapCheckWarningLogged = false;
        beingHeld = true;
    }

    public static void DropObject()
    {
        if (IsOverlappingPlayer())
        {
            return;
        }

        if (heldObjectFollow != null)
        {
            heldObjectFollow.StopFollowing();
            Destroy(heldObjectFollow);
        }

        heldObjectFollow = null;
        heldObject = null;
        holdDistanceExceededTime = 0f;
        heldPlayerRoot = null;
        beingHeld = false;
    }

    private static bool IsOverlappingPlayer()
    {
        if (heldObject == null || heldPlayerRoot == null)
        {
            return true;
        }

        Physics.SyncTransforms();
        SurfCharacter playerCharacter = heldPlayerRoot.GetComponentInChildren<SurfCharacter>(true);
        Collider playerCharacterCollider = playerCharacter != null ? playerCharacter.collider : null;
        Collider[] playerColliders = heldPlayerRoot.GetComponentsInChildren<Collider>(true);
        Collider[] objectColliders = heldObject.GetComponentsInChildren<Collider>(true);
        bool foundPlayerCollider = IsUsableCollider(playerCharacterCollider);
        bool foundObjectCollider = false;

        foreach (Collider playerCollider in playerColliders)
        {
            if (IsUsableCollider(playerCollider) && !playerCollider.isTrigger)
            {
                foundPlayerCollider = true;
                break;
            }
        }

        foreach (Collider objectCollider in objectColliders)
        {
            if (IsUsableCollider(objectCollider) && !objectCollider.isTrigger)
            {
                foundObjectCollider = true;
                break;
            }
        }

        if (!foundPlayerCollider || !foundObjectCollider)
        {
            if (!overlapCheckWarningLogged)
            {
                Debug.LogError("Cannot safely drop the held object because its solid collider or the player's solid collider could not be found.");
                overlapCheckWarningLogged = true;
            }

            return true;
        }

        if (IsUsableCollider(playerCharacterCollider) &&
            ColliderOverlapsObject(playerCharacterCollider, objectColliders))
        {
            return true;
        }

        foreach (Collider playerCollider in playerColliders)
        {
            if (!IsUsableCollider(playerCollider) || playerCollider.isTrigger ||
                playerCollider == playerCharacterCollider)
            {
                continue;
            }

            if (ColliderOverlapsObject(playerCollider, objectColliders))
            {
                return true;
            }
        }

        return false;
    }

    private static bool ColliderOverlapsObject(Collider playerCollider, Collider[] objectColliders)
    {
        foreach (Collider objectCollider in objectColliders)
        {
            if (!IsUsableCollider(objectCollider) || objectCollider.isTrigger)
            {
                continue;
            }

            if (playerCollider.bounds.Intersects(objectCollider.bounds) ||
                Physics.ComputePenetration(
                    playerCollider,
                    playerCollider.transform.position,
                    playerCollider.transform.rotation,
                    objectCollider,
                    objectCollider.transform.position,
                    objectCollider.transform.rotation,
                    out _,
                    out _))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsUsableCollider(Collider collider)
    {
        return collider != null && collider.enabled && collider.gameObject.activeInHierarchy;
    }
}
