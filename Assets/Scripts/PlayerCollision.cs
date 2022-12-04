using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            if (!PlayerController.GetInstance().skill)
            {
                PlayerManager.GetInstance().isGameOver = true;
            }
            else
            {
                Destroy(hit.gameObject);
                PlayerController.GetInstance().ResetPower();
            }
        }
    }

    
}