using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePart : MonoBehaviour
{
    int i = 0;

    float delay;

    protected Rigidbody rb;

    SnakeHead head;

    public void Init(SnakeHead head, float delay)
    {
        this.head = head;
        rb = GetComponent<Rigidbody>();
        this.delay = delay;
    }

    public void Move(int index, float moveZ)
    {
        if (delay > Time.deltaTime / 2)
        {
            delay -= Time.deltaTime;
            return;
        }

        delay = 0;
        float delta = Time.deltaTime;
        while (delta >= head.deltasHistory[i][index])
        {
            Vector3 pos = head.posHistory[i];
            pos.z = rb.position.z;

            rb.MovePosition(pos);
            delta -= head.deltasHistory[i][index];
            i++;
        }

        if (delta > 0)
        {
            Vector3 pos = head.posHistory[i];
            pos.z = rb.position.z;

            rb.MovePosition(Vector3.Lerp(rb.position, pos, delta / head.deltasHistory[i][index]));
            head.deltasHistory[i][index] -= delta;
        }
        rb.MovePosition(rb.position + new Vector3(0,0, moveZ));
    }
}
