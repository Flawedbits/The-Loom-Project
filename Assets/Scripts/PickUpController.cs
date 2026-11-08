using UnityEngine;

public class PickUpController : MonoBehaviour
{
    [SerializeField] private Transform holdArea;
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private float pickupRange = 5f;
    [SerializeField, Min(0.1f)] private float maxHoldDistance = 3.5f;
    [SerializeField, Min(0f)] private float maxHoldDistanceGracePeriod = 0.75f;

    private static HeldObject heldRealityA = new HeldObject();
    private static HeldObject heldRealityB = new HeldObject();

    private QuantumGoggles quantumGoggles;
    private Transform parallelHoldArea;
    private int realityALayer = -1;
    private int realityBLayer = -1;

    public static bool beingHeld => heldRealityA.Rigidbody != null || heldRealityB.Rigidbody != null;

    private sealed class HeldObject
    {
        public Rigidbody Rigidbody;
        public PhysicsFollow Follow;
        public Transform PlayerRoot;
        public float DistanceExceededTime;
    }

    public void ConfigureParallelReality(
        QuantumGoggles goggles,
        Transform realityBHoldArea,
        int realityALayer,
        int parallelRealityLayer)
    {
        quantumGoggles = goggles;
        parallelHoldArea = realityBHoldArea;
        this.realityALayer = realityALayer;
        realityBLayer = parallelRealityLayer;
    }

    private void Update()
    {
        if (!Input.GetKeyDown(CustomInputManager.IM.grab))
        {
            return;
        }

        bool realityBFirst = quantumGoggles != null && quantumGoggles.GogglesOn;
        if (TryInteractWithReality(realityBFirst))
        {
            return;
        }

        if (beingHeld)
        {
            DropObject();
        }
    }

    private void FixedUpdate()
    {
        UpdateHeldObject(heldRealityA, holdArea, transform.root);
        UpdateHeldObject(heldRealityB, parallelHoldArea, quantumGoggles != null
            ? quantumGoggles.ParallelBody
            : null);
    }

    private bool TryInteractWithReality(bool realityBFirst)
    {
        if (realityBFirst)
        {
            if (TryRaycastRealityB(transform.position, transform.forward, out Rigidbody target))
            {
                ToggleHeldObject(target, true);
                return true;
            }

            return TryRaycastRealityA(quantumGoggles.GetRealityAInteractionOrigin(), out target) &&
                   ToggleHeldObject(target, false);
        }

        if (TryRaycastRealityA(transform.position, out Rigidbody realityATarget))
        {
            ToggleHeldObject(realityATarget, false);
            return true;
        }

        if (quantumGoggles != null &&
            TryRaycastRealityB(
                quantumGoggles.GetParallelInteractionOrigin(),
                transform.forward,
                out Rigidbody realityBTarget))
        {
            ToggleHeldObject(realityBTarget, true);
            return true;
        }

        return false;
    }

    private bool TryRaycastRealityA(Vector3 origin, out Rigidbody targetRigidbody)
    {
        targetRigidbody = null;
        int realityAMask = pickupLayerMask.value;
        int ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");
        if (ignoreRaycastLayer >= 0)
        {
            realityAMask &= ~(1 << ignoreRaycastLayer);
        }

        if (realityALayer >= 0)
        {
            realityAMask |= 1 << realityALayer;
        }
        if (realityBLayer >= 0)
        {
            realityAMask &= ~(1 << realityBLayer);
        }

        if (!Physics.Raycast(
                origin,
                transform.forward,
                out RaycastHit hit,
                pickupRange,
                realityAMask,
                QueryTriggerInteraction.Ignore))
        {
            return false;
        }

        targetRigidbody = hit.rigidbody;
        return targetRigidbody != null;
    }

    private bool TryRaycastRealityB(Vector3 origin, Vector3 direction, out Rigidbody targetRigidbody)
    {
        targetRigidbody = null;
        if (realityBLayer < 0 ||
            !Physics.Raycast(
                origin,
                direction,
                out RaycastHit hit,
                pickupRange,
                1 << realityBLayer,
                QueryTriggerInteraction.Ignore))
        {
            return false;
        }

        targetRigidbody = hit.rigidbody;
        return targetRigidbody != null;
    }

    private bool ToggleHeldObject(Rigidbody targetRigidbody, bool isRealityBObject)
    {
        HeldObject heldObject = isRealityBObject ? heldRealityB : heldRealityA;
        if (heldObject.Rigidbody == targetRigidbody)
        {
            DropHeldObject(heldObject);
            return true;
        }

        if (heldObject.Rigidbody != null)
        {
            DropHeldObject(heldObject);
        }

        Transform targetHoldArea = isRealityBObject ? parallelHoldArea : holdArea;
        Transform targetPlayerRoot = isRealityBObject && quantumGoggles != null
            ? quantumGoggles.ParallelBody
            : transform.root;
        if (targetHoldArea == null || targetPlayerRoot == null)
        {
            Debug.LogError("PickUpController requires a hold area and player body for the target reality.", this);
            return true;
        }

        heldObject.Rigidbody = targetRigidbody;
        heldObject.PlayerRoot = targetPlayerRoot;
        heldObject.Follow = targetRigidbody.gameObject.AddComponent<PhysicsFollow>();
        heldObject.Follow.Initialize(
            targetHoldArea,
            targetRigidbody,
            targetPlayerRoot.GetComponentsInChildren<Collider>(true));
        heldObject.DistanceExceededTime = 0f;
        return true;
    }

    private void UpdateHeldObject(HeldObject heldObject, Transform targetHoldArea, Transform playerRoot)
    {
        if (heldObject.Rigidbody == null)
        {
            ClearHeldObject(heldObject);
            return;
        }

        Transform distanceReference = heldObject.PlayerRoot != null ? heldObject.PlayerRoot : playerRoot;
        if (targetHoldArea == null ||
            distanceReference == null ||
            (heldObject.Rigidbody.worldCenterOfMass - distanceReference.position).sqrMagnitude >
            maxHoldDistance * maxHoldDistance)
        {
            heldObject.DistanceExceededTime += Time.fixedDeltaTime;
            if (heldObject.DistanceExceededTime >= maxHoldDistanceGracePeriod)
            {
                DropHeldObject(heldObject);
            }
        }
        else
        {
            heldObject.DistanceExceededTime = 0f;
        }
    }

    public static void DropObject()
    {
        DropHeldObject(heldRealityA);
        DropHeldObject(heldRealityB);
    }

    private static void DropHeldObject(HeldObject heldObject)
    {
        if (heldObject.Rigidbody == null)
        {
            ClearHeldObject(heldObject);
            return;
        }

        if (heldObject.Follow != null)
        {
            heldObject.Follow.StopFollowing();
            Destroy(heldObject.Follow);
        }
        ClearHeldObject(heldObject);
        ClearHeldObject(heldObject);
    }

    private static void ClearHeldObject(HeldObject heldObject)
    {
        heldObject.Rigidbody = null;
        heldObject.Follow = null;
        heldObject.PlayerRoot = null;
        heldObject.DistanceExceededTime = 0f;
    }

}
