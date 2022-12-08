using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleMove : MonoBehaviour
{
    Collectible collectibleScript;

    void Start()
    {
        collectibleScript = GetComponent<Collectible>();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, collectibleScript.playerTransform.position, collectibleScript.moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player Bubble")
        {
            PlayerManager.GetInstance().collectibleCount += 1;
            if (!PlayerManager.GetInstance().stopIncrease)
            {
                PlayerManager.GetInstance().powerCount += 1;
            }
            Destroy(gameObject);
        }
    }
}