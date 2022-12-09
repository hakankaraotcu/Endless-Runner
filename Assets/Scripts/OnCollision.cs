using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour
{
    public PlayerController player;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player" || collision.transform.tag == "Ground")
        {
            return;
        }
        player.OnCharacterColliderHit(collision.collider);
    }
}
