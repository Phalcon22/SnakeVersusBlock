using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace svb
{
    public class SnakePart : MonoBehaviour
    {
        int i = 0;

        Rigidbody rb;
        SnakeHead head;

        public float remainingDelta { get; private set; }
        public void Init(SnakeHead head, SnakePart tail)
        {
            rb = GetComponent<Rigidbody>();

            this.head = head;

            GetComponent<MeshRenderer>().material.color = GameManager.m.snake.GetNewColor();


            i = tail.GetMoveIndex();
            if (tail == head)
                i = head.posHistory.Count - 2;
            
            for (; i > 0;  i--)
            {
                if (head.posHistory[i].z <= tail.GetComponent<Rigidbody>().position.z - GetComponent<BoxCollider>().bounds.size.z * 0.75f)
                {
                    break;
                }
            }

            rb.position = head.posHistory[i];
            remainingDelta = head.deltasHistory[i];
        }

        public int GetMoveIndex()
        {
            return i;
        }

        public void DecrementMoveIndex()
        {
            i--;
        }

        bool error;
        public void Move(float speed)
        {
            if (error && i < head.posHistory.Count)
                remainingDelta = head.deltasHistory[i];

            while (i < head.posHistory.Count && head.speedHistory[i] == 0)
            {
                i++;
                if (i < head.posHistory.Count)
                    remainingDelta = head.deltasHistory[i];
            }
            if (i >= head.posHistory.Count)
            {
                remainingDelta = 0;
                error = true;
                return;
            }

            float compensativeSpeed = speed / head.speedHistory[i];
            float deltaXSpeed = Time.deltaTime * compensativeSpeed;
            while (i < head.posHistory.Count && deltaXSpeed >= remainingDelta)
            {
                rb.MovePosition(head.posHistory[i]);
                deltaXSpeed -= remainingDelta;

                i++;
                if (i < head.posHistory.Count)
                    remainingDelta = head.deltasHistory[i];

                while (i < head.posHistory.Count && head.speedHistory[i] == 0)
                {
                    i++;
                    if (i < head.posHistory.Count)
                        remainingDelta = head.deltasHistory[i];
                }
                if (i >= head.posHistory.Count)
                {
                    remainingDelta = 0;
                    error = true;
                    return;
                }

                deltaXSpeed /= compensativeSpeed;
                compensativeSpeed = speed / head.speedHistory[i];
                deltaXSpeed *= compensativeSpeed;
            }

            deltaXSpeed /= compensativeSpeed;
            compensativeSpeed = speed / head.speedHistory[i];
            deltaXSpeed *= compensativeSpeed;


            if (deltaXSpeed > 0)
            {
                rb.MovePosition(Vector3.Lerp(rb.position, head.posHistory[i], deltaXSpeed / remainingDelta));
                remainingDelta -= deltaXSpeed;
            }
        }
    }
}
