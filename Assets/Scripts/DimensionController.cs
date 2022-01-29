using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DimensionController handles the dimension-switching mechanic.
/// Its design is more procedural than object-oriented, both for readability and as a demonstration
/// of moderated object-oriented design. Instead of splitting this single mechanic accross multiple
/// objects and files, its various parts are organized here and carried out in an orderly fashion.
/// 
/// The only responsibility left to other scripts is for level elements to extend their colliders
/// when the ExtendColliderFlag is set, and I may change that as well.
/// </summary>

public class DimensionController : MonoBehaviour
{
    // Singleton instance
    public static DimensionController DC { get; private set; }

    // Flag for level elements
    public bool ExtendColliderFlag { get; private set; } = false;

    // Key binding
    [SerializeField] KeyCode FlipKey = KeyCode.F;

    // GameObject References
    [SerializeField] private GameObject CombinedAvatar;
    [SerializeField] private GameObject LightAvatar;
    [SerializeField] private GameObject DarkAvatar;
    [SerializeField] private GameObject MainCamera; // Camera doesn't need its own script anymore.
    [SerializeField] private GameObject Mirror; // The object that the mirror level is parented to

    // Camera Settings
    [SerializeField] private Vector3 _2DViewPosition = new Vector3(0, 0, -10);
    [SerializeField] private Vector3 _2DViewAngle = Vector3.zero;
    [SerializeField] private Vector3 _3DViewPosition = new Vector3(-5, 10, -10);
    [SerializeField] private Vector3 _3DViewAngle = new Vector3(30, 15, 0);
    [SerializeField] private float CameraMoveSpeed = 0.05f;
    [SerializeField] private float CameraRotateSpeed = 0.15f;

    // Swinging mirror level part
    [SerializeField] private float MirrorUnfoldAngle = 0;
    [SerializeField] private float MirrorFoldAngle = -160;
    //[SerializeField] private int MirrorSwingFrames = 60; // Number of FixedUpdates it takes for the mirror to swing
    [SerializeField] private float MirrorSwingSpeed = 90f;

    // Precomputed Camera target Quaternions
    private Quaternion _2DViewRotation;
    private Quaternion _3DViewRotation;

    // Camera target
    private Vector3 camTargetPos; //target position
    private Quaternion camTargetRot; //target rotation

    // Control flags
    private bool thirdDimension = true; //true when we are in the 3D view.
    private bool transition = false; //true while in the middle of a transition between dimensions.
    private bool canFlip = true; //false if any condition prevents switching dimensions.

    // Set/enforce singleton
    private void Awake()
    {
        if (DC != null) Destroy(DC);
        DC = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Null-check all required objects
        if (CombinedAvatar == null || LightAvatar == null || DarkAvatar == null || MainCamera == null || Mirror == null)
            Debug.LogError("DimensionController is missing one or more object references. Check settings!");

        // Precompute camera target quaternions
        _2DViewRotation = Quaternion.Euler(_2DViewAngle);
        _3DViewRotation = Quaternion.Euler(_3DViewAngle);

        // Confirm correct avatars enabled/disabled and flag is set
        ExtendColliderFlag = false;
        CombinedAvatar.SetActive(true);
        LightAvatar.SetActive(false);
        DarkAvatar.SetActive(false);

        // Confirm correct start position of mirror and camera
        Mirror.transform.rotation = Quaternion.Euler(MirrorFoldAngle, 0 , 0);
        MainCamera.transform.position = _3DViewPosition;
        MainCamera.transform.rotation = _3DViewRotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if we can flip at the moment
        canFlip = !transition; //for now, we can flip as long as we aren't in the middle of flipping already.

        // Check for control input
        if (Input.GetKeyDown(FlipKey) && canFlip)
        {
            Debug.Log("Beginning dimension flip.");
            if (thirdDimension) StartCoroutine(Transition2D()); // Switch to 2D
            else StartCoroutine(Transition3D()); // Switch to 3D
        }
    }

    // Perform transition to 2D over a number of frames.
    // Sets or clears transition and thirdDimension flags.
    IEnumerator Transition2D()
    {
        // Begin Transition
        transition = true;

        // Swing Camera
        camTargetPos = _2DViewPosition;
        camTargetRot = _2DViewRotation;

        while (Quaternion.Angle(MainCamera.transform.rotation, camTargetRot) > 1f || Vector3.Distance(MainCamera.transform.position, camTargetPos) > 0.01)
        {
            // Move and rotate camera toward target
            MainCamera.transform.rotation = Quaternion.RotateTowards(MainCamera.transform.rotation, camTargetRot, CameraRotateSpeed * Time.deltaTime);
            MainCamera.transform.position = Vector3.MoveTowards(MainCamera.transform.position, camTargetPos, CameraMoveSpeed * Time.deltaTime);
            Debug.Log("Moving camera.");
            yield return null;
        }

        // Lower Mirror
        while (Quaternion.Angle(Mirror.transform.rotation, Quaternion.Euler(MirrorUnfoldAngle,Mirror.transform.rotation.y,Mirror.transform.rotation.z)) > 1f)
        {
            // Swing mirror toward its target
            Mirror.transform.rotation = Quaternion.RotateTowards(Mirror.transform.rotation,
                Quaternion.Euler(MirrorUnfoldAngle, Mirror.transform.rotation.y,Mirror.transform.rotation.z),
                MirrorSwingSpeed*Time.deltaTime);
            Debug.Log("Moving mirror.");
            yield return null;
        }

        // Extend Colliders
        ExtendColliderFlag = true;

        // Split Avatars
        LightAvatar.transform.position = CombinedAvatar.transform.position;
        DarkAvatar.transform.position = new Vector3(
            CombinedAvatar.transform.position.x,
            -CombinedAvatar.transform.position.y,
            CombinedAvatar.transform.position.z);
        CombinedAvatar.SetActive(false);
        LightAvatar.SetActive(true);
        DarkAvatar.SetActive(true);

        // End Transition
        thirdDimension = false;
        transition = false;
        Debug.Log("Transition to 2D complete.");
    }

    IEnumerator Transition3D()
    {
        // Begin Transition
        transition = true;

        // Combine Avatars
        CombinedAvatar.transform.position = LightAvatar.transform.position;
        CombinedAvatar.SetActive(true);
        LightAvatar.SetActive(false);
        DarkAvatar.SetActive(false);

        // Retract Colliders
        ExtendColliderFlag = false;

        // Raise Mirror
        while (Quaternion.Angle(Mirror.transform.rotation, Quaternion.Euler(MirrorFoldAngle, Mirror.transform.rotation.y, Mirror.transform.rotation.z)) > 1f)
        {
            // Swing mirror toward its target
            Mirror.transform.rotation = Quaternion.RotateTowards(Mirror.transform.rotation,
                Quaternion.Euler(MirrorFoldAngle, Mirror.transform.rotation.y, Mirror.transform.rotation.z),
                MirrorSwingSpeed * Time.deltaTime);
            yield return null;
        }

        // Return Camera
        camTargetPos = _3DViewPosition;
        camTargetRot = _3DViewRotation;

        while (Quaternion.Angle(MainCamera.transform.rotation, camTargetRot) > 0.1f || Vector3.Distance(MainCamera.transform.position, camTargetPos) > 0.01)
        {
            MainCamera.transform.rotation = Quaternion.RotateTowards(MainCamera.transform.rotation, camTargetRot, CameraRotateSpeed * Time.deltaTime);
            MainCamera.transform.position = Vector3.MoveTowards(MainCamera.transform.position, camTargetPos, CameraMoveSpeed * Time.deltaTime);
            yield return null;
        }

        // End Transition
        thirdDimension = true;
        transition = false;
        Debug.Log("Transition to 3D complete.");
    }
}
