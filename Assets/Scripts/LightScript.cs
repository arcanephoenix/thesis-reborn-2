using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    public bool areLightsFlickering = true;
    public bool isBlinking;
    public bool isIntensitySet;
    [Range(0f, 1f)]
    public float intensityMinimum = 0.85f;
    [Range(1f, 5f)]
    public float intensityMaximum = 1.2f;
    private float currentIntensity;
    
    private Light thisLight;

    private void Start()
    {
        thisLight = GetComponent<Light>();
        currentIntensity = thisLight.intensity;
        if(thisLight == null)
        {
            Debug.LogError("no light attached to this object " + gameObject.name);
        }
        StartCoroutine(LightOnOff());
    }

    IEnumerator LightOnOff()
    {
        while (true)
        {
            if(areLightsFlickering)
            {
                float onTime = Random.Range(0.2f, 2.0f);
                //float lightIntensity = currentIntensity;
                thisLight.enabled = !thisLight.enabled;
                if(!isIntensitySet) thisLight.intensity = Random.Range(intensityMinimum, intensityMaximum) * currentIntensity;
                yield return new WaitForSeconds(onTime);
            }
            yield return null;
        }
    }

    IEnumerator LightBlink()
    {
        while(true)
        {
            if(isBlinking)
            {
                thisLight.enabled = !thisLight.enabled;
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}
