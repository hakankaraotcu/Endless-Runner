using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public Transform playerTransform;
    public float moveSpeed = 70f;
    CollectibleMove collectibleMoveScript;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        collectibleMoveScript = gameObject.GetComponent<CollectibleMove>();
    }

    void Update()
    {
        transform.Rotate(60 * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerManager.GetInstance().collectibleCount += 1;
            if (!PlayerManager.GetInstance().stopIncrease)
            {
                PlayerManager.GetInstance().powerCount += 1;
            }
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "CollectibleDetector")
        {
            collectibleMoveScript.enabled = true;
        }
    }
}