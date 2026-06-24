using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{

    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference inventory;
    [SerializeField] InputActionReference interact; //abrir puertas o interactuar con otros elementos
    //[SerializeField] InputActionReference attack;
    CharacterController characterController;
    [SerializeField] GameObject inventoryCanvas;
    [SerializeField] AudioClip openInventory;
    [SerializeField] AudioClip openDoor;

    private GameObject currentDoor;

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
        inventory.action.Enable();
        inventory.action.performed += OnOpenInventory;
        interact.action.Enable();
        interact.action.performed += OnInteract;

        //attack.action.Enable();
    }

    

    private void OnDisable()
    {
        move.action.Disable();
        move.action.started -= OnMove;
        move.action.performed -= OnMove;
        move.action.canceled -= OnMove;
        inventory.action.Disable();
        inventory.action.performed -= OnOpenInventory;
        interact.action.Disable();
        interact.action.performed -= OnInteract;

        //attack.action.Disable();
    }

    Vector2 rawMove = Vector2.zero;
    private void OnMove(InputAction.CallbackContext obj)
    {
        rawMove = obj.action.ReadValue<Vector2>();
        characterController.SetRawMove(rawMove);
    }

    private void OnOpenInventory(InputAction.CallbackContext context)
    {
        bool isActive = inventoryCanvas.activeSelf;
        inventoryCanvas.SetActive(!isActive);
        AudioSource.PlayClipAtPoint(openInventory, transform.position);
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (currentDoor == null) return;

        bool isActive = currentDoor.activeSelf;
        currentDoor.SetActive(!isActive);
        AudioSource.PlayClipAtPoint(openDoor, transform.position);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Door"))
        {
            currentDoor = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Door"))
        {
            currentDoor = null;
        }
    }
}
