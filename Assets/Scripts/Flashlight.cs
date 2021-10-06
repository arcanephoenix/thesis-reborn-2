using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light flashlight;
    public int flashlightLife = 10;
    public KeyCode flashlightKey;
    

    private void Start()
    {
        StartCoroutine(PowerManager());
    }
    void Update()
    {
        if (Input.GetKeyDown(flashlightKey) && flashlightLife > 0)
        {
            flashlight.enabled = !flashlight.enabled;
        }
        if (flashlightLife == 0)
        {
            flashlight.enabled = false;
        }
    }
    IEnumerator PowerManager()
    {
        //yield return new WaitForSeconds(1);
        while(true)
        {
            if(flashlight.enabled && flashlightLife > 0)
            {
                flashlightLife--;
                Debug.Log(flashlightLife);
                
            }
            else if(!flashlight.enabled && flashlightLife <10)
            {
                flashlightLife++;
                Debug.Log(flashlightLife);
            }
            yield return new WaitForSeconds(1);
        }
    }
    
}
