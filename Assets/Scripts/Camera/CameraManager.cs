using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace svb
{
    public class CameraManager : MonoBehaviour
    {
        Snake snake;

        Rigidbody rb;

        bool waitMode;

        [SerializeField]
        Rigidbody leftBorder;
        
        [SerializeField]
        Rigidbody rightBorder;

        public void Init()
        {
            waitMode = false;
            snake = GameManager.m.snake;
            rb = GetComponent<Rigidbody>();

            Camera.main.backgroundColor = LevelGenerator.m.level.colorSet.background;
        }

        void Update()
        {
            if (!GameManager.m.started)
                return;

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
                float coef = 1;
                if (dist >= 0.5f)
                    coef = 0.5f;

                float speed = GameManager.m.rules.verticalSpeed;

                if (GameManager.m.snake.turbo)
                    speed *= 2;

                var translation = new Vector3(0, 0, speed) * Time.deltaTime * coef;
                rb.MovePosition(rb.position + translation);
            }

            Vector3 border = leftBorder.position;
            border.z = rb.position.z;
            leftBorder.position = border;

            border = rightBorder.position;
            border.z = rb.position.z;
            rightBorder.position = border;
        }
    }
}
