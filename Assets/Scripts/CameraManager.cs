using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace svb
{

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
            float dist = pos.z - snake.GetPos().z;

            if (dist >= GameManager.m.rules.cameraMaxDistance)
                waitMode = true;

            if (waitMode)
            {
                if (Mathf.Abs(dist) < GameManager.m.rules.cameraSyncDist)
                {
                    waitMode = false;
                    Vector3 newPos = pos;
                    newPos.z = snake.GetPos().z;
                    rb.position = newPos;
                }
            }
            else
            {
                var translation = new Vector3(0, 0, GameManager.m.rules.verticalSpeed) * Time.deltaTime;
                rb.MovePosition(rb.position + translation);
            }
        }
    }
}
