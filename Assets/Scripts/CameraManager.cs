using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Snake snake;

    Rigidbody rb;

    bool waitMode = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Vector3 startPos = snake.GetPos();
        startPos.y = rb.position.y;
        rb.position = startPos;
    }

    void Update()
    {
        Vector3 pos = rb.position;
        float ahead = pos.z - snake.GetPos().z;

        if (ahead >= 2)
            waitMode = true;

        if (waitMode)
        {
            if (Mathf.Abs(ahead) < 0.1f)
            {
                waitMode = false;
                Vector3 newPos = pos;
                newPos.z = snake.GetPos().z;
                rb.position = newPos;
            }
        }
        else
        {
            var translation = new Vector3(0, 0, Snake.verticalSpeed) * Time.deltaTime;
            rb.MovePosition(rb.position + translation);
        }
    }
}
