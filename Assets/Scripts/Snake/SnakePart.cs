using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace svb
{
    public class SnakePart : MonoBehaviour
    {
        int i = 0;

        public int deltaIndex { get; private set; }

        Rigidbody rb;

        SnakeHead head;

        public void Init(SnakeHead head, SnakePart tail, int deltaIndex)
        {
            rb = GetComponent<Rigidbody>();

            this.deltaIndex = deltaIndex;
            this.head = head;

            GetComponent<MeshRenderer>().material.color = GameManager.m.snake.GetNewColor();

            i = tail.GetMoveIndex();
            if (tail == head)
                i = head.posHistory.Count - 2;
            
            for (; i > 0;  i--)
            {
                if (head.posHistory[i].z <= tail.GetComponent<Rigidbody>().position.z - GetComponent<BoxCollider>().bounds.size.z *0.9f)
                {
                    break;
                }
            }

            rb.position = head.posHistory[i];
        }

        public int GetMoveIndex()
        {
            return i;
        }

        public void Move(float speed)
        {
            if (i < 0)
            {
                i++;
                return;
            }

            while (i < head.posHistory.Count && head.speedHistory[i] == 0)
                i++;
            if (i >= head.posHistory.Count)
                return;

            float compensativeSpeed = speed / head.speedHistory[i];
            float delta = Time.deltaTime;
            float deltaXSpeed = delta * compensativeSpeed;
            while (i < head.posHistory.Count && deltaXSpeed >= head.deltasHistory[i][deltaIndex])
            {
                rb.MovePosition(head.posHistory[i]);
                deltaXSpeed -= head.deltasHistory[i][deltaIndex];
                i++;

                deltaXSpeed /= compensativeSpeed;
                delta = deltaXSpeed;
                while (i < head.posHistory.Count && head.speedHistory[i] == 0)
                    i++;
                if (i >= head.posHistory.Count || i < 0)
                    return;

                compensativeSpeed = speed / head.speedHistory[i];
                deltaXSpeed *= compensativeSpeed;
            }

            deltaXSpeed /= compensativeSpeed;
            delta = deltaXSpeed;
            while (i < head.posHistory.Count && head.speedHistory[i] == 0)
                i++;
            if (i >= head.posHistory.Count || i < 0)
                return;

            compensativeSpeed = speed / head.speedHistory[i];
            deltaXSpeed *= compensativeSpeed;


            if (deltaXSpeed > 0)
            {
                rb.MovePosition(Vector3.Lerp(rb.position, head.posHistory[i], deltaXSpeed / head.deltasHistory[i][deltaIndex]));
                head.deltasHistory[i][deltaIndex] -= deltaXSpeed;
            }
        }
    }
}
