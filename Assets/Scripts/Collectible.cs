using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(60 * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
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