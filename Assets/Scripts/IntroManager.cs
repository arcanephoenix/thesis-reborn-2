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
    public GameObject playerObject;
    public Light bigLight;
    public Image aimImage;

    private void Start()
    {
        fader.CrossFadeAlpha(0, 1, false);
        playerObject.SetActive(false);
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
        fader.CrossFadeAlpha(1, 2, false);
        yield return new WaitForSeconds(2);
        playerObject.SetActive(true);
        aimImage.enabled = true;
        Destroy(gameObject);
    }

    public void GameStarter()
    {
        StartCoroutine(FadeOut()); 
    }
}
