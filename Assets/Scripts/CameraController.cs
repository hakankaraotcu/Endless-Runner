using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /*
    public Transform target;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, offset.z + target.position.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, 10 * Time.deltaTime);
    }*/

    public Transform player;
    public Vector3 offset2;
    void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, player.position.x, 10 * Time.deltaTime), transform.position.y, offset2.z + player.position.z);
    }
}
