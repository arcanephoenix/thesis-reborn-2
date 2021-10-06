using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public Material material;
    public Color colorGreen;
    public float intensity = 0.01f;
    private void Start()
    {
        //ColorUtility.TryParseHtmlString("#00600003", out colorGreen);
        material.EnableKeyword("_EMISSION");
    }
    private void Update()
    {
        material.SetColor("_EmissionColor", colorGreen * intensity);
    }
    
}
