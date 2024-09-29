using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera currentCamera;

    [SerializeField] private LayerMask placementLayer, enemyLayer;
    private Vector3 lastPosition;
    ControlsPlayers inputMap;
    InputAction _mousePosition;

    public event Action OnClicked, OnExit;
    // Start is called before the first frame update
    void Awake()
    {
        inputMap = new ControlsPlayers();
    }
    private void OnEnable()
    {
        inputMap.PlayerActionMap.Enable();
        inputMap.PlayerActionMap.MouseClick.performed += EnterBuildingMode;
        inputMap.PlayerActionMap.Exit.performed += ExitBuildingMode;
    }

    private void EnterBuildingMode(InputAction.CallbackContext context)
    {
        OnClicked?.Invoke();
    }

    private void ExitBuildingMode(InputAction.CallbackContext context)
    {
        OnExit?.Invoke();
    }

    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();
    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = inputMap.PlayerActionMap.MousePosition.ReadValue<Vector2>();
        //The near clipping plane is nearest point of the Camera's view frustum. The Camera cannot see geometry that is closer than this distance.
        mousePos.z = currentCamera.nearClipPlane;

        Ray ray = currentCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100, placementLayer))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }

    public Vector3 GetAttackMousePosition()
    {
        Vector3 mousePos = inputMap.PlayerActionMap.MousePosition.ReadValue<Vector2>();
        //The near clipping plane is nearest point of the Camera's view frustum. The Camera cannot see geometry that is closer than this distance.
        mousePos.z = currentCamera.nearClipPlane;

        Ray ray = currentCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, enemyLayer))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }

}
