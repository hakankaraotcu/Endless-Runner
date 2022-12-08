using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    private void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, player.position.x, 10 * Time.deltaTime), transform.position.y, offset.z + player.position.z);
    }
}
