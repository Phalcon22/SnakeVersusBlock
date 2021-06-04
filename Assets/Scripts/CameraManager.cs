using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Snake snake;

    bool waitMode = false;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 startPos = snake.GetPos();
        startPos.y = rb.position.y;
        rb.position = startPos;
    }

    void FixedUpdate()
    {
        float ahead = rb.position.z - snake.GetPos().z;
        
        if (waitMode)
        {
            if (Mathf.Abs(ahead) < 0.1f)
            {
                waitMode = false;
                Vector3 startPos = snake.GetPos();
                startPos.x = rb.position.x;
                startPos.y = rb.position.y;
                rb.position = startPos;
            }
            else
            {
                Vector3 velo = snake.GetVelocity() * 0.5f;
                velo.x = 0;
                rb.velocity = velo;
            }
        }
        else if (ahead >= 1)
        {
            waitMode = true;
        }
        else
        {
            /*Vector3 startPos = snake.GetPos();
            startPos.x = rb.position.x;
            startPos.y = rb.position.y;
            rb.position = startPos;*/
            Vector3 velo = snake.GetVelocity();
            velo.x = 0;
            rb.velocity = velo;
        }
    }
}
