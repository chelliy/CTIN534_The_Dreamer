using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractControl : MonoBehaviour
{
    // Start is called before the first frame update
    public enum InteractState { GeneralView, SpecificAreaView, InspectObject, ObjectSelected, Pause }
    public InteractState currentState;
    public InteractState pauseSaveState;
    public InteractControl interactControl;
    public KeyCode selectKey = KeyCode.Mouse0;
    public KeyCode backKey = KeyCode.Mouse1;

    public GameObject currentCamera;

    public GameObject inspectPosition;

    public float rotationSensitivity = 5f;

    public GameObject targetInteractable = null;
    public Transform originalTransform = null;


    public RaycastHit[] debug;

    void Awake()
    {
        interactControl = this;
        currentState = InteractState.GeneralView;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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
    private void HandleGeneralViewState()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("SpecificArea"));
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
            Debug.Log("Happened");
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

        }
        else
        {
            //almost same with general view
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Interactable"));
            debug = hits;
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
                    originalTransform = targetInteractable.transform;
                    targetInteractable.transform.parent = inspectPosition.transform;
                    targetInteractable.transform.position = inspectPosition.transform.position;
                    currentState = InteractState.InspectObject;

                }
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
                targetInteractable.transform.position = originalTransform.position;
                targetInteractable.transform.rotation = originalTransform.rotation;
                targetInteractable.transform.localScale = originalTransform.localScale;
                targetInteractable = null;
                originalTransform = null;
            }
            currentState = InteractState.SpecificAreaView;

        }
        else
        {
            //inspection logic here
            targetInteractable.transform.position = inspectPosition.transform.position;
            float XaxisRotation = Input.GetAxis("Mouse X") * rotationSensitivity;
            float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSensitivity;
            //rotate the object depending on mouse X-Y Axis
            targetInteractable.transform.Rotate(Vector3.down, XaxisRotation);
            targetInteractable.transform.Rotate(Vector3.right, YaxisRotation);
        }
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
