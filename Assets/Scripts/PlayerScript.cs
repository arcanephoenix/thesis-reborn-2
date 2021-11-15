using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;

/*
 * Manages player movement
 */
public class PlayerScript : MonoBehaviour
{
    public float speed = 2.5f;
    private float g = -9.81f;
    public float gMultiplier = 0.25f;
    public Image fader;

    Vector3 gravity;
    CharacterController controller;

    //public Image fader;  


    private static bool canPlayerMove = true;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controller = GetComponent<CharacterController>();
        fader.CrossFadeAlpha(0, 2, false);
        StartCoroutine(MovementPause());
    }
    /*
     * stops movement at the start as part of intro
     */
    IEnumerator MovementPause()
    {
        canPlayerMove = false;
        fader.CrossFadeAlpha(0, 1, false);
        yield return new WaitForSeconds(1);
        canPlayerMove = true;
    }
    
    private void Update()
    {
        if (AudioListener.volume < 1)
        {
            AudioListener.volume += 0.5f * Time.deltaTime;
        }

        /* Player Movement Code. */
        if(canPlayerMove)
        { 
            if (controller.isGrounded && gravity.y < 0)
            {
                gravity.y = -2f;
            }

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 move = transform.right * horizontal + transform.forward * vertical;
            controller.Move(move * speed * Time.deltaTime);

            
        }
        gravity.y += g * gMultiplier;

        controller.Move(gravity * Time.deltaTime);
    }


    /*
     * utility static function
     * enables/disables player movement
     */
    public static void TogglePlayerMovement(bool canMove)
    {
        canPlayerMove = canMove;
    }
}
