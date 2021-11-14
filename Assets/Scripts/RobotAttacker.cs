using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAttacker : MonoBehaviour
{
    public GameObject player;
    public CharacterController character;
    public Transform statusOrb;
    public Material materialDanger;
    public Material materialGood;
    private bool isHostile = false;

    private void Start()
    {
        statusOrb.GetComponent<MeshRenderer>().material = materialGood;
    }

    public void BecomeHostile()
    {
        statusOrb.GetComponent<MeshRenderer>().material = materialDanger;
        isHostile = true;
    }

    private void Update()
    {
        if(isHostile)
        {
            float turnAngle = Vector3.SignedAngle(transform.forward, player.transform.position - transform.position, transform.up);
            //transform.Rotate(transform.up, turnAngle);
            if (Mathf.Abs(turnAngle) > 0.5f)
            {
                transform.Rotate(transform.up, turnAngle);
            }
            character.Move(transform.forward);
        }
    }
}
