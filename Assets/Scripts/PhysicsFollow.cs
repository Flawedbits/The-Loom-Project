using System.Collections.Generic;
using UnityEngine;

public class PhysicsFollow : MonoBehaviour
{
    [SerializeField] private float positionSpring = 120f;
    [SerializeField] private float positionDamping = 22f;
    [SerializeField] private float maxAcceleration = 100f;
    [SerializeField] private float rotationSpring = 80f;
    [SerializeField] private float rotationDamping = 18f;
    [SerializeField] private float maxAngularAcceleration = 100f;
    [SerializeField] private float maxTargetSpeed = 25f;
    [SerializeField] private float maxTargetAngularSpeed = 15f;
    private readonly List<CollisionIgnoreState> ignoredCollisions = new List<CollisionIgnoreState>();

    private Transform follow;
    private Rigidbody rb;
    private RigidbodyInterpolation previousInterpolation;
    private float previousMaxAngularVelocity;
    private Vector3 previousTargetPosition;
    private Quaternion previousTargetRotation;

    private struct CollisionIgnoreState
    {
        public Collider playerCollider;
        public Collider objectCollider;
        public bool wasIgnored;
    }

    public void Initialize(Transform target, Rigidbody targetRigidbody, Collider[] playerColliders)
    {
        follow = target;
        rb = targetRigidbody;
        previousTargetPosition = follow.position;
        previousTargetRotation = follow.rotation;
        previousInterpolation = rb.interpolation;
        previousMaxAngularVelocity = rb.maxAngularVelocity;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.maxAngularVelocity = maxTargetAngularSpeed;

        Collider[] objectColliders = rb.GetComponentsInChildren<Collider>(true);
        foreach (Collider playerCollider in playerColliders)
        {
            if (playerCollider == null)
            {
                continue;
            }

            foreach (Collider objectCollider in objectColliders)
            {
                if (objectCollider == null)
                {
                    continue;
                }

                bool wasIgnored = Physics.GetIgnoreCollision(playerCollider, objectCollider);
                ignoredCollisions.Add(new CollisionIgnoreState
                {
                    playerCollider = playerCollider,
                    objectCollider = objectCollider,
                    wasIgnored = wasIgnored
                });
                Physics.IgnoreCollision(playerCollider, objectCollider, true);
            }
        }
    }

    private void FixedUpdate()
    {
        if (rb == null || follow == null)
        {
            return;
        }

        float fixedDeltaTime = Time.fixedDeltaTime;
        Vector3 targetVelocity = Vector3.ClampMagnitude(
            (follow.position - previousTargetPosition) / fixedDeltaTime,
            maxTargetSpeed);
        Vector3 positionError = follow.position - rb.worldCenterOfMass;
        Vector3 acceleration = positionError * positionSpring -
                               (rb.linearVelocity - targetVelocity) * positionDamping -
                               Physics.gravity;
        rb.AddForce(Vector3.ClampMagnitude(acceleration, maxAcceleration), ForceMode.Acceleration);

        Quaternion targetRotationDelta = follow.rotation * Quaternion.Inverse(previousTargetRotation);
        Vector3 targetAngularVelocity = Vector3.ClampMagnitude(
            GetAngularVelocity(targetRotationDelta, fixedDeltaTime),
            maxTargetAngularSpeed);
        Quaternion rotationDelta = follow.rotation * Quaternion.Inverse(rb.rotation);
        Vector3 rotationError = GetAngularVelocity(rotationDelta, 1f);
        Vector3 angularAcceleration = rotationError * rotationSpring -
                                      (rb.angularVelocity - targetAngularVelocity) * rotationDamping;
        rb.AddTorque(Vector3.ClampMagnitude(angularAcceleration, maxAngularAcceleration), ForceMode.Acceleration);

        previousTargetPosition = follow.position;
        previousTargetRotation = follow.rotation;
    }

    public void StopFollowing()
    {
        if (rb != null)
        {
            rb.interpolation = previousInterpolation;
            rb.maxAngularVelocity = previousMaxAngularVelocity;
        }

        foreach (CollisionIgnoreState state in ignoredCollisions)
        {
            if (state.playerCollider != null && state.objectCollider != null)
            {
                Physics.IgnoreCollision(state.playerCollider, state.objectCollider, state.wasIgnored);
            }
        }

        ignoredCollisions.Clear();
        follow = null;
        rb = null;
        enabled = false;
    }

    private void OnDestroy()
    {
        StopFollowing();
    }

    private static Vector3 GetAngularVelocity(Quaternion rotationDelta, float deltaTime)
    {
        rotationDelta.ToAngleAxis(out float angle, out Vector3 axis);
        if (angle > 180f)
        {
            angle -= 360f;
        }

        if (Mathf.Approximately(angle, 0f) || deltaTime <= 0f)
        {
            return Vector3.zero;
        }

        return axis * (angle * Mathf.Deg2Rad / deltaTime);
    }
}
