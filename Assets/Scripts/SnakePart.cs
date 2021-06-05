using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePart : MonoBehaviour
{
    int i = 0;

    float delay;

    protected Rigidbody rb;

    public void Init(float delay)
    {
        rb = GetComponent<Rigidbody>();
        this.delay = delay;
    }

    public void tmp(SnakeHead head, int index)
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
            rb.MovePosition(head.posHistory[i]);
            delta -= head.deltasHistory[i][index];
            i++;
        }

        if (delta > 0)
        {
            rb.MovePosition(Vector3.Lerp(rb.position, head.posHistory[i], delta / head.deltasHistory[i][index]));
            head.deltasHistory[i][index] -= delta;
        }
    }
}
