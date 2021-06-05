using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace svb
{

    public class SnakePart : MonoBehaviour
    {
        int i = 0;

        bool started = false;
        bool pause = true;
        float delay;

        protected Rigidbody rb;

        SnakeHead head;

        public void Init(SnakeHead head, float delay)
        {
            rb = GetComponent<Rigidbody>();

            this.head = head;
            this.delay = delay;
            i = head.posHistory.Count - 1;
        }

        public int GetMoveIndex()
        {
            return i;
        }

        public void Move(int index, float moveZ)
        {
            if (!started)
            {
                StartCoroutine(InitPauseCoroutine(delay));
                started = true;
            }

            MoveX(index);
            MoveZ(moveZ);
        }

        public void MoveX(int index)
        {
            if (pause)
                return;

            if (i < 0)
            {
                i++;
                return;
            }

            float delta = Time.deltaTime;
            Vector3 pos;
            while (delta >= head.deltasHistory[i][index])
            {
                pos = head.posHistory[i];
                pos.z = rb.position.z;

                rb.MovePosition(pos);
                delta -= head.deltasHistory[i][index];
                i++;
            }

            pos = head.posHistory[i];
            pos.z = rb.position.z;

            rb.MovePosition(Vector3.Lerp(rb.position, pos, delta / head.deltasHistory[i][index]));
            head.deltasHistory[i][index] -= delta;
        }

        public void MoveZ(float moveZ)
        {
            rb.MovePosition(rb.position + new Vector3(0, 0, moveZ));
        }

        IEnumerator InitPauseCoroutine(float seconds)
        {
            pause = true;
            yield return new WaitForSeconds(seconds);

            i = head.posHistory.Count - 1;
            for (float timer = 0; i >= 0 && timer <= delay; i--)
                timer += head.deltasHistory[i][0];

            pause = false;
        }
    }

}
