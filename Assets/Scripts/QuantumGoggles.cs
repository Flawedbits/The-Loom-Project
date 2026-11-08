using Fragsurf.Movement;
using System;
using UnityEngine;

[DefaultExecutionOrder(-100)]
[DisallowMultipleComponent]
[RequireComponent(typeof(SurfCharacter))]
public class QuantumGoggles : MonoBehaviour
{
    private const string RealityALayerName = "RealityA";
    private const string RealityBLayerName = "RealityB";
    private const string RealityBPlayerLayerName = "RealityBPlayer";

    private Camera playerCamera;
    private Transform cameraHolder;
    private SurfCharacter movementController;
    private SurfCharacter parallelMovementController;
    private GameObject parallelBody;
    private Transform parallelHoldArea;
    private Vector3 cameraHolderLocalPosition;
    private Quaternion cameraHolderLocalRotation;
    private int realityACullingMask;
    private int realityBCullingMask;
    private int realityALayer;
    private int realityBLayer;
    private int realityBPlayerLayer;
    private int originalPlayerLayer;
    private bool previousRealityACollision;
    private bool previousRealityBPlayerCollision;
    private bool previousCrossRealityCollision;
    private bool previousPlayerAcrossRealityCollision;
    private bool movementWasEnabled;
    private bool initialized;
    private bool parallelBodyNeedsReactivation;

    public bool GogglesOn { get; private set; }
    public Transform ParallelBody => parallelBody != null ? parallelBody.transform : null;
    public static bool IsRealityBActive { get; private set; }
    public static event Action<bool> RealityChanged;

    private void Awake()
    {
        movementController = GetComponent<SurfCharacter>();
        playerCamera = GetComponentInChildren<Camera>(true);

        if (movementController == null)
        {
            Debug.LogError("QuantumGoggles requires a SurfCharacter movement controller.", this);
            enabled = false;
            return;
        }

        if (playerCamera == null)
        {
            Debug.LogError("QuantumGoggles could not find a Camera on the player or its children.", this);
            enabled = false;
            return;
        }

        PlayerAiming playerAiming = playerCamera.GetComponentInParent<PlayerAiming>();
        if (playerAiming == null)
        {
            Debug.LogError("QuantumGoggles could not find PlayerAiming on the camera hierarchy.", this);
            enabled = false;
            return;
        }

        cameraHolder = playerAiming.transform;
        cameraHolderLocalPosition = cameraHolder.localPosition;
        cameraHolderLocalRotation = cameraHolder.localRotation;
        realityALayer = LayerMask.NameToLayer(RealityALayerName);
        realityBLayer = LayerMask.NameToLayer(RealityBLayerName);
        realityBPlayerLayer = LayerMask.NameToLayer(RealityBPlayerLayerName);
        if (realityALayer < 0 || realityBLayer < 0 || realityBPlayerLayer < 0)
        {
            Debug.LogError(
                $"QuantumGoggles requires the '{RealityALayerName}', '{RealityBLayerName}', and " +
                $"'{RealityBPlayerLayerName}' layers.",
                this);
            enabled = false;
            return;
        }

        originalPlayerLayer = gameObject.layer;
        gameObject.layer = realityALayer;
        CreateParallelBody();
        ConfigureRealityCollisions();

        int realityALayerMask = 1 << realityALayer;
        int realityBLayerMask = 1 << realityBLayer;
        int realityBPlayerLayerMask = 1 << realityBPlayerLayer;
        int sharedLayers = playerCamera.cullingMask &
                           ~(realityALayerMask | realityBLayerMask | realityBPlayerLayerMask);
        realityACullingMask = sharedLayers | realityALayerMask;
        realityBCullingMask = sharedLayers | realityBLayerMask;

        movementWasEnabled = movementController.enabled;
        initialized = true;

        PickUpController pickupController = playerCamera.GetComponent<PickUpController>();
        if (pickupController != null)
        {
            pickupController.ConfigureParallelReality(
                this,
                parallelHoldArea,
                realityALayer,
                realityBLayer);
        }
        else
        {
            Debug.LogError("QuantumGoggles requires a PickUpController on the player camera.", this);
        }

        SetGoggles(false);
    }

    private void Update()
    {
        KeyCode toggleKey = CustomInputManager.IM != null
            ? CustomInputManager.IM.googles
            : KeyCode.F;
        if (Time.timeScale > 0f && Input.GetKeyDown(toggleKey))
        {
            ToggleGoggles();
        }
    }

    private void LateUpdate()
    {
        if (parallelBody == null)
        {
            return;
        }

        parallelBody.transform.rotation = transform.rotation;

        parallelHoldArea.SetPositionAndRotation(
            GetParallelInteractionOrigin() +
            playerCamera.transform.forward * 2.79f,
            playerCamera.transform.rotation);

        if (parallelMovementController.collider != null)
        {
            parallelMovementController.collider.gameObject.name = "ParallelPlayerCollider";
        }
    }

    private void OnEnable()
    {
        if (!initialized || !parallelBodyNeedsReactivation)
        {
            return;
        }

        cameraHolder.SetParent(transform, false);
        cameraHolder.localPosition = cameraHolderLocalPosition;
        cameraHolder.localRotation = cameraHolderLocalRotation;
        playerCamera.cullingMask = realityACullingMask;
        parallelBody.SetActive(true);
        parallelBodyNeedsReactivation = false;
        movementController.enabled = movementWasEnabled;
        parallelMovementController.enabled = movementWasEnabled;
    }

    public void ToggleGoggles()
    {
        SetGoggles(!GogglesOn);
    }

    public Vector3 GetParallelInteractionOrigin()
    {
        if (parallelBody == null)
        {
            Debug.LogError("The parallel body has not been initialized.", this);
            return playerCamera.transform.position;
        }

        return GetInteractionOrigin(parallelBody.transform);
    }

    public Vector3 GetRealityAInteractionOrigin()
    {
        return GetInteractionOrigin(transform);
    }

    private Vector3 GetInteractionOrigin(Transform targetBody)
    {
        Transform sourceBody = GogglesOn && parallelBody != null
            ? parallelBody.transform
            : transform;
        Vector3 cameraOffset = Quaternion.Inverse(sourceBody.rotation) *
                               (playerCamera.transform.position - sourceBody.position);
        return targetBody.position + targetBody.rotation * cameraOffset;
    }

    public void SetGoggles(bool gogglesOn)
    {
        if (!initialized)
        {
            Debug.LogError("QuantumGoggles cannot change state before initialization.", this);
            return;
        }

        GogglesOn = gogglesOn;
        IsRealityBActive = gogglesOn;
        cameraHolder.SetParent(gogglesOn ? parallelBody.transform : transform, false);
        cameraHolder.localPosition = cameraHolderLocalPosition;
        cameraHolder.localRotation = cameraHolderLocalRotation;
        playerCamera.cullingMask = gogglesOn ? realityBCullingMask : realityACullingMask;

        bool movementEnabled = movementWasEnabled && !gogglesOn;
        movementController.enabled = movementEnabled;
        parallelMovementController.enabled = movementEnabled;
        RealityChanged?.Invoke(gogglesOn);
    }

    private void CreateParallelBody()
    {
        parallelBody = new GameObject("Reality B Body");
        parallelBody.SetActive(false);
        parallelBody.layer = realityBPlayerLayer;
        parallelBody.transform.SetPositionAndRotation(transform.position, transform.rotation);

        parallelMovementController = parallelBody.AddComponent<SurfCharacter>();
        parallelMovementController.colliderSize = movementController.colliderSize;
        parallelMovementController.weight = movementController.weight;
        parallelMovementController.rigidbodyPushForce = movementController.rigidbodyPushForce;
        parallelMovementController.solidCollider = movementController.solidCollider;
        parallelMovementController.viewTransform = cameraHolder;
        parallelMovementController.playerRotationTransform = parallelBody.transform;
        parallelMovementController.crouchingHeightMultiplier = movementController.crouchingHeightMultiplier;
        parallelMovementController.crouchingSpeed = movementController.crouchingSpeed;
        parallelMovementController.crouchingEnabled = movementController.crouchingEnabled;
        parallelMovementController.slidingEnabled = movementController.slidingEnabled;
        parallelMovementController.laddersEnabled = movementController.laddersEnabled;
        parallelMovementController.supportAngledLadders = movementController.supportAngledLadders;
        parallelMovementController.useStepOffset = movementController.useStepOffset;
        parallelMovementController.stepOffset = movementController.stepOffset;
        parallelMovementController.movementConfig = movementController.movementConfig;

        GameObject holdAreaObject = new GameObject("ParallelHoldArea");
        holdAreaObject.layer = realityBPlayerLayer;
        parallelHoldArea = holdAreaObject.transform;
        parallelHoldArea.SetParent(parallelBody.transform, false);

        parallelBody.SetActive(true);
    }

    private void ConfigureRealityCollisions()
    {
        previousRealityACollision = Physics.GetIgnoreLayerCollision(realityALayer, realityALayer);
        previousRealityBPlayerCollision =
            Physics.GetIgnoreLayerCollision(realityBPlayerLayer, realityBLayer);
        previousCrossRealityCollision = Physics.GetIgnoreLayerCollision(realityALayer, realityBLayer);
        previousPlayerAcrossRealityCollision =
            Physics.GetIgnoreLayerCollision(realityALayer, realityBPlayerLayer);

        Physics.IgnoreLayerCollision(realityALayer, realityALayer, false);
        Physics.IgnoreLayerCollision(realityBPlayerLayer, realityBLayer, false);
        Physics.IgnoreLayerCollision(realityALayer, realityBLayer, true);
        Physics.IgnoreLayerCollision(realityALayer, realityBPlayerLayer, true);
    }

    private void OnDisable()
    {
        if (!initialized)
        {
            return;
        }

        GogglesOn = false;
        IsRealityBActive = false;
        if (gameObject.activeInHierarchy)
        {
            cameraHolder.SetParent(transform, false);
            cameraHolder.localPosition = cameraHolderLocalPosition;
            cameraHolder.localRotation = cameraHolderLocalRotation;
        }
        else if (parallelBody != null)
        {
            parallelBody.SetActive(false);
            parallelBodyNeedsReactivation = true;
        }

        playerCamera.cullingMask = realityACullingMask;
        movementController.enabled = movementWasEnabled;
        if (parallelMovementController != null)
        {
            parallelMovementController.enabled = movementWasEnabled;
        }

        RealityChanged?.Invoke(false);
    }

    private void OnDestroy()
    {
        if (realityALayer >= 0)
        {
            Physics.IgnoreLayerCollision(realityALayer, realityALayer, previousRealityACollision);
        }

        if (realityBPlayerLayer >= 0 && realityBLayer >= 0)
        {
            Physics.IgnoreLayerCollision(
                realityBPlayerLayer,
                realityBLayer,
                previousRealityBPlayerCollision);
        }

        if (realityALayer >= 0 && realityBLayer >= 0)
        {
            Physics.IgnoreLayerCollision(
                realityALayer,
                realityBLayer,
                previousCrossRealityCollision);
        }

        if (realityALayer >= 0 && realityBPlayerLayer >= 0)
        {
            Physics.IgnoreLayerCollision(
                realityALayer,
                realityBPlayerLayer,
                previousPlayerAcrossRealityCollision);
        }

        if (initialized)
        {
            gameObject.layer = originalPlayerLayer;
        }

        if (parallelBody != null)
        {
            Destroy(parallelBody);
        }
    }
}
