using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicConvoStart : MonoBehaviour
{
    public Light myLight;
    public static bool isLightBlinking = true;
    public Color[] colors;
    private int colorLength;
    private int currentColorIndex = 0;
    private void Start()
    {
        colorLength = colors.Length;
        StartCoroutine(LightBlinking());
    }

    IEnumerator LightBlinking()
    {
        while(true)
        {
            if (isLightBlinking)
            {
                currentColorIndex = (currentColorIndex + 1) % colorLength;
                myLight.color = colors[currentColorIndex];
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    //public void TurnBlinkingOff()
    //{
        //isLightBlinking = false;
    //}
}
