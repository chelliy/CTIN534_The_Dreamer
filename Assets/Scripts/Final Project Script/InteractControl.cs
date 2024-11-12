using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class InteractControl : MonoBehaviour
{
    // Start is called before the first frame update
    public enum InteractState 
    { 
        GeneralView, 
        SpecificAreaView, 
        InspectObject, 
        ObjectSelected, 
        Pause 
    }
    public InteractState currentState;
    public InteractState pauseSaveState;
    public static InteractControl interactControl;
    public static CinemachineBrain cinemachineBrain;
    public KeyCode selectKey = KeyCode.Mouse0;
    public KeyCode backKey = KeyCode.Mouse1;

    public GameObject currentCamera;

    public GameObject inspectPosition;

    public float rotationSensitivity = 20f;
    public float distanceFromCamera = 5f;

    public float keepBoxUI_XPos_InPercentage = 0.1f; // X position percentage of UI box
    public float keepBoxUI_YPos_InPercentage = 0.1f; // Y position percentage of UI box
    public float keepBoxUI_WidthPercentage = 0.3f; // Width percentage of UI box
    public float keepBoxUI_HeightPercentage = 0.3f; // Height percentage of UI box

    public float trashBoxUI_XPos_InPercentage = 0.6f; // X position percentage of UI box
    public float trashBoxUI_YPos_InPercentage = 0.1f; // Y position percentage of UI box
    public float trashBoxUI_WidthPercentage = 0.3f; // Width percentage of UI box
    public float trashBoxUI_HeightPercentage = 0.3f; // Height percentage of UI box

    //public Rect keepBoxUI;
    //public Rect currentObjectBoxUI;

    public GameObject targetInteractable = null;


    void Awake()
    {
        interactControl = this;
        cinemachineBrain = GetComponent<CinemachineBrain>();
        currentState = InteractState.GeneralView;
    }

    // Update is called once per frame
    void Update()
    {
        if (!cinemachineBrain.IsBlending)
        {
            switch (currentState)
            {
                case InteractState.GeneralView:
                    HandleGeneralViewState();
                    break;
                case InteractState.SpecificAreaView:
                    HandleSpecificAreaViewState();
                    break;
                case InteractState.InspectObject:
                    HandleInspectObjectState();
                    break;
                case InteractState.ObjectSelected:
                    HandleObjectSelectedState();
                    break;
                case InteractState.Pause:
                    HandlePauseState();
                    break;
            }
        }
    }
    private void HandleGeneralViewState()
    {
        //try to see if mouse on the specific area, if so, turn on highlight
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("SpecificArea"));
        if (hits.Length > 0)
        {
            //found area
            RaycastHit currentFoundInteractable = hits[hits.Length - 1];
            //check if need to switch, since we can find the same or different
            if (targetInteractable)
            {
                if (targetInteractable != currentFoundInteractable.transform.gameObject)
                {
                    targetInteractable.GetComponent<Outline>().enabled = false;
                    targetInteractable = currentFoundInteractable.transform.gameObject;
                    targetInteractable.GetComponent<Outline>().enabled = true;
                }
            }
            else
            {
                targetInteractable = currentFoundInteractable.transform.gameObject;
                targetInteractable.GetComponent<Outline>().enabled = true;
            }
        }
        else
        {
            //mouse on space, disable highlight
            if (targetInteractable)
            {
                targetInteractable.GetComponent<Outline>().enabled = false;
                targetInteractable = null;
            }
        }

        //if left click, go to the specific area camera
        if (Input.GetKeyDown(selectKey))
        {
            if (targetInteractable)
            {
                targetInteractable.GetComponent<Outline>().enabled = false;
                currentCamera = targetInteractable.GetComponent<SpecificArea>().relatedCamera;
                currentCamera.SetActive(true);
                targetInteractable = null;
                currentState = InteractState.SpecificAreaView;

            }
        }
    }

    private void HandleSpecificAreaViewState()
    {
        //if right click, go back to the previous camera: general camera
        if (Input.GetKeyDown(backKey))
        {
            if (targetInteractable)
            {
                targetInteractable.GetComponent<Outline>().enabled = false;
                targetInteractable = null;
            }
            currentCamera.SetActive(false);
            currentCamera = null;
            currentState = InteractState.GeneralView;
            return;

        }

        //almost same with general view
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Interactable"));
        if (hits.Length > 0)
        {
            RaycastHit currentFoundInteractable = hits[hits.Length - 1];
            if (targetInteractable)
            {
                if (targetInteractable != currentFoundInteractable.transform.gameObject)
                {
                    targetInteractable.GetComponent<Outline>().enabled = false;
                    targetInteractable = currentFoundInteractable.transform.gameObject;
                    targetInteractable.GetComponent<Outline>().enabled = true;
                }
            }
            else
            {
                targetInteractable = currentFoundInteractable.transform.gameObject;
                targetInteractable.GetComponent<Outline>().enabled = true;
            }
        }
        else
        {
            if (targetInteractable)
            {
                targetInteractable.GetComponent<Outline>().enabled = false;
                targetInteractable = null;
            }
        }

        if (Input.GetKeyDown(selectKey))
        {
            //difference here, let "pick up object"
            if (targetInteractable)
            {
                targetInteractable.GetComponent<Outline>().enabled = false;
                //targetInteractable.transform.parent = inspectPosition.transform;
                targetInteractable.transform.position = Camera.main.transform.position + Camera.main.transform.forward * distanceFromCamera;
                currentState = InteractState.InspectObject;

            }
        }

    }

    private void HandleInspectObjectState()
    {
        if (Input.GetKeyDown(backKey))
        {
            if (targetInteractable)
            {
                targetInteractable.GetComponent<InteractableObject>().ResetTransform();
                targetInteractable = null;
            }
            else
            {
                Debug.Log("Bug, no object selected");
                currentState = InteractState.Pause;
                return;

            }
            currentState = InteractState.SpecificAreaView;
            return;
        }
        if (Input.GetKeyDown(selectKey))
        {
            if (targetInteractable)
            {
                targetInteractable.GetComponent<InteractableObject>().ResetTransform();
                // Create a ray from the camera through the mouse position
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // Calculate the target position along this ray
                Vector3 targetPosition = ray.origin + ray.direction * distanceFromCamera;

                // Set the object's position to align with the calculated target position
                targetInteractable.transform.position = targetPosition;
            }
            else
            {
                Debug.Log("Bug, no object selected");
                currentState = InteractState.Pause;
                return;

            }
            currentState = InteractState.ObjectSelected;
            return;
        }
        //inspection logic here
        targetInteractable.transform.position = Camera.main.transform.position + Camera.main.transform.forward * distanceFromCamera;
        float XaxisRotation = Input.GetAxis("Mouse X") * rotationSensitivity;
        float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSensitivity;
        //rotate the object depending on mouse X-Y Axis
        // Clamp the vertical rotation to prevent flipping
        YaxisRotation = Mathf.Clamp(YaxisRotation, -90f, 90f);

        // Create quaternions for the rotations around each axis
        Quaternion rotationAroundY = Quaternion.AngleAxis(XaxisRotation, Vector3.up);
        Quaternion rotationAroundX = Quaternion.AngleAxis(YaxisRotation, Vector3.right);

        // Combine the rotations
        var currentRotation = rotationAroundY * rotationAroundX;

        // Apply the rotation to the object
        transform.rotation = currentRotation;
        targetInteractable.transform.Rotate(Vector3.up, XaxisRotation);
        targetInteractable.transform.Rotate(Vector3.right, YaxisRotation);


    }

    private void HandleObjectSelectedState()
    {
        if (Input.GetKeyUp(selectKey))
        {
            if (targetInteractable)
            {
                targetInteractable.transform.position = Camera.main.transform.position + Camera.main.transform.forward * distanceFromCamera;
            }
            else
            {
                Debug.Log("Bug, no object selected");
                currentState = InteractState.Pause;
                return;

            }
            currentState = InteractState.InspectObject;
            return;
        }        
        // Create a ray from the camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Calculate the target position along this ray
        Vector3 targetPosition = ray.origin + ray.direction * distanceFromCamera;

        // Set the object's position to align with the calculated target position
        targetInteractable.transform.position = targetPosition;
        if (CheckIfOverLapWithKeepBox())
        {
            targetInteractable.GetComponent<Outline>().enabled = true;
        }
        else
        {
            if (CheckIfOverLapWithTrashBox())
            {
                targetInteractable.GetComponent<Outline>().enabled = true;
            }
            else
            {
                targetInteractable.GetComponent<Outline>().enabled = false;
            }
        }
    }

    private void HandlePauseState()
    {
        throw new NotImplementedException();
    }

    private bool CheckIfOverLapWithKeepBox()
    {
        Vector2 minUIPoint = new Vector2(keepBoxUI_YPos_InPercentage, keepBoxUI_XPos_InPercentage);
        Vector2 maxUIPoint = new Vector2(keepBoxUI_YPos_InPercentage + keepBoxUI_HeightPercentage, keepBoxUI_XPos_InPercentage + keepBoxUI_WidthPercentage);

        //Vector2 tempx = new Vector2(minUIPoint.x*Screen.width, minUIPoint.y * Screen.height);
        //Vector2 tempy = new Vector2(maxUIPoint.x * Screen.width, maxUIPoint.y * Screen.height);
        //Debug.DrawLine(tempx, tempy, Color.red, 100000f);
        // Step 2: Calculate the 3D object's bounding box in screen space
        Bounds bounds = targetInteractable.GetComponent<MeshRenderer>().bounds;
        Vector3[] corners = new Vector3[8];
        corners[0] = bounds.min;
        corners[1] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        corners[2] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        corners[3] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
        corners[4] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        corners[5] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
        corners[6] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
        corners[7] = bounds.max;

        Vector3 minScreenPoint = Camera.main.WorldToScreenPoint(corners[0]);
        Vector3 maxScreenPoint = minScreenPoint;

        for (int i = 1; i < corners.Length; i++)
        {
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(corners[i]);
            minScreenPoint = Vector3.Min(minScreenPoint, screenPoint);
            maxScreenPoint = Vector3.Max(maxScreenPoint, screenPoint);
        }

        //Debug.DrawLine(minScreenPoint, maxScreenPoint, Color.green, 100000f);
        // Step 3: Convert min and max screen points to percentages of the screen size
        float objectMinXPercentage = minScreenPoint.x / Screen.width;
        float objectMinYPercentage = minScreenPoint.y / Screen.height;
        float objectMaxXPercentage = maxScreenPoint.x / Screen.width;
        float objectMaxYPercentage = maxScreenPoint.y / Screen.height;

        Vector2 minObjectPoint = new Vector2(objectMinXPercentage, objectMinYPercentage);
        Vector2 maxObjectPoint = new Vector2(objectMaxXPercentage, objectMaxYPercentage);

        if (IsAABBOverlap(minUIPoint, maxUIPoint, minObjectPoint, maxObjectPoint))
        {
            Debug.Log("The 3D object overlaps with the UI box!");
            return true;
        }
        else
        {
            Debug.Log("The 3D object does not overlap with the UI box.");
            return false;
        }
    }
    private bool CheckIfOverLapWithTrashBox()
    {
        // Step 1: Define the UI box in percentage-based screen space
        Vector2 minUIPoint = new Vector2 (trashBoxUI_YPos_InPercentage, trashBoxUI_XPos_InPercentage);
        Vector2 maxUIPoint = new Vector2(trashBoxUI_YPos_InPercentage + trashBoxUI_HeightPercentage, trashBoxUI_XPos_InPercentage + trashBoxUI_WidthPercentage);

        //Vector2 tempx = new Vector2(minUIPoint.x * Screen.width, minUIPoint.y * Screen.height);
        //Vector2 tempy = new Vector2(maxUIPoint.x * Screen.width, maxUIPoint.y * Screen.height);
        //Debug.DrawLine(tempx, tempy, Color.red, 100000f);

        // Step 2: Calculate the 3D object's bounding box in screen space
        Bounds bounds = targetInteractable.GetComponent<MeshRenderer>().bounds;
        Vector3[] corners = new Vector3[8];
        corners[0] = bounds.min;
        corners[1] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        corners[2] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        corners[3] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
        corners[4] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        corners[5] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
        corners[6] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
        corners[7] = bounds.max;

        Vector3 minScreenPoint = Camera.main.WorldToScreenPoint(corners[0]);
        Vector3 maxScreenPoint = minScreenPoint;

        for (int i = 1; i < corners.Length; i++)
        {
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(corners[i]);
            minScreenPoint = Vector3.Min(minScreenPoint, screenPoint);
            maxScreenPoint = Vector3.Max(maxScreenPoint, screenPoint);
        }

        // Step 3: Convert min and max screen points to percentages of the screen size
        //Debug.DrawLine(minScreenPoint, maxScreenPoint, Color.green, 100000f);
        float objectMinXPercentage = minScreenPoint.x / Screen.width;
        float objectMinYPercentage = minScreenPoint.y / Screen.height;
        float objectMaxXPercentage = maxScreenPoint.x / Screen.width;
        float objectMaxYPercentage = maxScreenPoint.y / Screen.height;

        Vector2 minObjectPoint = new Vector2(objectMinXPercentage, objectMinYPercentage);
        Vector2 maxObjectPoint = new Vector2(objectMaxXPercentage, objectMaxYPercentage);

        if (IsAABBOverlap(minUIPoint, maxUIPoint, minObjectPoint, maxObjectPoint))
        {
            Debug.Log("The 3D object overlaps with the trash box!");
            return true;
        }
        else
        {
            Debug.Log("The 3D object does not overlap with the trash box.");
            return false;
        }
    }

    private bool IsAABBOverlap(Vector2 UIMin, Vector2 UIMax, Vector2 ObjMin, Vector2 ObjMax)
    {
        return UIMin.x < ObjMax.x && UIMax.x > ObjMin.x &&
               UIMin.y < ObjMax.y && UIMax.y > ObjMin.y;
    }

}
