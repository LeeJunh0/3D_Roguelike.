using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    public GameObject Player;
    Camera Camera;
    public float Ypos = 10f;
    public float Zpos = -5f;
    void Start()
    {
        Camera = GetComponent<Camera>();
    }

    void Update()
    {
        transform.position = Player.transform.position + new Vector3(0, Ypos, Zpos);
        if (Time.timeScale != 0)
            MousePosition();

    }

    public void MousePosition()
    {
        Ray MousePos = Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit Rayhit;

        if (Physics.Raycast(MousePos, out Rayhit))
        {
            Vector3 newPos = new Vector3(Rayhit.point.x, Player.transform.position.y, Rayhit.point.z) - Player.transform.position;
            Player.transform.forward = newPos;
        }
    }
}
