using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{

    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference attack;
    CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        move.action.Enable();
        move.action.started += OnMove;
        move.action.performed += OnMove;
        move.action.canceled += OnMove;

        attack.action.Enable();
    }

    private void OnDisable()
    {
        move.action.Disable();
        move.action.started -= OnMove;
        move.action.performed -= OnMove;
        move.action.canceled -= OnMove;

        attack.action.Disable();
    }

    Vector2 rawMove = Vector2.zero;
    private void OnMove(InputAction.CallbackContext obj)
    {
        rawMove = obj.action.ReadValue<Vector2>();
        characterController.SetRawMove(rawMove);
    }
}
