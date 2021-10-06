using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAttacker : MonoBehaviour
{
    public GameObject player;
    public CharacterController character;

    private void Update()
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
