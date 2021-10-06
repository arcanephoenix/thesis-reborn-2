using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * mouselook script
 */
public class PlayerLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    float xRotation = 0f;
    public Transform playerBody;
    private static bool isCamMoving = true;
    //public static bool canMouseMove = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
    }
    private void Update()
    {
        if(isCamMoving)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90, 90);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
    public static void ToggleCamMovement(bool canMoveCam)
    {
        isCamMoving = canMoveCam;
    }

    /*
     * utility function
     * enables/disables mouse movement
     */
    public static void SetMouseMover(bool canMouseMove)
    {
        if(canMouseMove) // player can move
        {
            PlayerScript.TogglePlayerMovement(true);
            ToggleCamMovement(true);
            Cursor.lockState = CursorLockMode.Locked;
        }
        else // player cannot move, mouse can move
        {
            PlayerScript.TogglePlayerMovement(false);
            ToggleCamMovement(false);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}
