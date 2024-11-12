using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    public float rotationSensitivity = 5f;

    public GameObject targetInteractable = null;


    void Awake()
    {
        interactControl = this;
        cinemachineBrain = GetComponent<CinemachineBrain>();
        currentState = InteractState.GeneralView;
    }

    void Start()
    {

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
                targetInteractable.transform.parent = inspectPosition.transform;
                targetInteractable.transform.position = inspectPosition.transform.position;
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
                targetInteractable.transform.parent = null;
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
                // Get the mouse position in screen space
                Vector3 mousePosition = Input.mousePosition;

                // Convert the mouse position to world space
                mousePosition.z = 5f;  // Set the z-position for 3D space
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

                // Update the object's position to follow the mouse
                transform.position = worldPosition;
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
        //inspection logic here
        targetInteractable.transform.position = inspectPosition.transform.position;
        float XaxisRotation = Input.GetAxis("Mouse X") * rotationSensitivity * Time.deltaTime;
        float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSensitivity * Time.deltaTime;
        //rotate the object depending on mouse X-Y Axis
        targetInteractable.transform.Rotate(Vector3.up, XaxisRotation);
        targetInteractable.transform.Rotate(Vector3.right, YaxisRotation);

    }

    private void HandleObjectSelectedState()
    {
        throw new NotImplementedException();
    }

    private void HandlePauseState()
    {
        throw new NotImplementedException();
    }

}
