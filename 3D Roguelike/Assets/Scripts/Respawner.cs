using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 18)
            return;

        Vector3 playerPos = GameManager.gameManager.PlayerScript.Area.transform.position;
        Vector3 myPos = transform.position;

        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffZ = Mathf.Abs(playerPos.z - myPos.z);

        float inputX = GameManager.gameManager.PlayerScript.HorizontalAxis < 0 ? -1 : 1;
        float inputZ = GameManager.gameManager.PlayerScript.VerticalAxis < 0 ? -1 : 1;

        switch(gameObject.tag)
        {
            case "Ground":
                if(diffX > diffZ)
                {
                    gameObject.transform.Translate(Vector3.right * inputX * 100f);
                    return;
                }
                else if(diffX < diffZ)
                {
                    gameObject.transform.Translate(new Vector3(0, 0, 1) * inputZ * 100f);
                    return;
                }
                break;
            case "Enermy":
                if(gameObject.layer == 8)
                {
                    gameObject.transform.position = (playerPos + (GameManager.gameManager.PlayerScript.MoveVec * 25f) + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f)));                  
                    return;
                }          
                break;
        }
    }
}
