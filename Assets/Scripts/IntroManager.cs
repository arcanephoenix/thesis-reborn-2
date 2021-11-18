using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Handles pre-gameplay intro
 */
public class IntroManager : MonoBehaviour
{
    public Camera cam;
    public float camSpeed = 5;
    public float introTimer = 10;
    private bool isCamMoving = false;
    public Image fader;
    public AudioListener audioListenerPlayerCamera;
    //public Light bigLight;
    public Image aimImage;

    private void Start()
    {
        fader.enabled = true;
        fader.CrossFadeAlpha(0, 1, false);
        audioListenerPlayerCamera.enabled = false;
        aimImage.enabled = false;
        StartCoroutine(CameraIntro());
    }

    IEnumerator CameraIntro()
    {
        isCamMoving = true;
        yield return new WaitForSeconds(introTimer);
        isCamMoving = false;
        GameStarter();
    }

    private void Update()
    {
        if(isCamMoving)
        {
            cam.transform.Translate(transform.forward * camSpeed * Time.deltaTime);
            AudioListener.volume -= 0.2f * Time.deltaTime;
        }
    }

    IEnumerator FadeOut()
    {
        fader.CrossFadeAlpha(1, 1, false);
        yield return new WaitForSeconds(2);
        audioListenerPlayerCamera.enabled = true;
        aimImage.enabled = true;
        Destroy(gameObject);
    }

    public void GameStarter()
    {
        StartCoroutine(FadeOut()); 
    }
}
