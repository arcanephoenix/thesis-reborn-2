using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCSample : MonoBehaviour
{
    public TMP_Text clickHere;
    public Transform player;
    public Light consoleLight;

    private void Start()
    {
        clickHere.enabled = false;
    }
}
