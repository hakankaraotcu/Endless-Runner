using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public GameObject collectibleDetectorObj;

    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(ActivateCollectible());
            Destroy(transform.GetChild(0).gameObject);
        }
    }

    IEnumerator ActivateCollectible()
    {
        collectibleDetectorObj.SetActive(true);

        yield return new WaitForSeconds(10f);

        collectibleDetectorObj.SetActive(false);
    }
}