using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Snake snake;

    bool waitMode = false;

    Transform tr;

    void Start()
    {
        tr = transform;
        Vector3 startPos = snake.GetPos();
        startPos.y = tr.position.y;
        tr.position = startPos;
    }

    void Update()
    {
        float ahead = tr.position.z - snake.GetPos().z;

        if (ahead >= 2)
            waitMode = true;

        if (waitMode)
        {
            if (Mathf.Abs(ahead) < 0.1f)
            {
                waitMode = false;
                Vector3 startPos = snake.GetPos();
                startPos.x = tr.position.x;
                startPos.y = tr.position.y;
                tr.position = startPos;
            }
        }
        else
        {
            var rb = GetComponent<Rigidbody>();
            var translation = new Vector3(0, 0, Snake.verticalSpeed) * Time.deltaTime;
            rb.MovePosition(rb.position + translation);
        }
    }
}
