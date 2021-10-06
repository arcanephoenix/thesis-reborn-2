using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotGenerator : MonoBehaviour
{
    public bool startGeneration = false;
    public GameObject robotPrefab;
    private GameObject newRobot;

    IEnumerator GenStart()
    {
        while(true)
        {
            while(startGeneration)
            {
                newRobot = Instantiate(robotPrefab, transform);
                yield return new WaitForSeconds(10f);
            }
        }
    }

    private void Start()
    {
        StartCoroutine(GenStart());
    }
}
