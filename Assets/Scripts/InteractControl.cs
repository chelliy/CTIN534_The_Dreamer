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

    public GameObject currentCamera;

    public GameObject targetInteractable = null;

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
        }
        else
        {
            if (targetInteractable)
            {
                targetInteractable.GetComponent<Outline>().enabled = false;
                targetInteractable = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
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
        throw new NotImplementedException();
    }

    private void HandleInspectObjectState()
    {
        throw new NotImplementedException();
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
