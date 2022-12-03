using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform player;
    public Vector3 offset2;

    private void Start()
    {
        player = FindObjectOfType<Player>().transform;
    }
    private void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, player.position.x, 10 * Time.deltaTime), transform.position.y, offset2.z + player.position.z);
    }
}
