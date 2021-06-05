using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        this.head = head;
        rb = GetComponent<Rigidbody>();
        this.delay = delay;
    }

    public int GetMoveIndex()
    {
        return i;
    }

    public void Move(int index, float moveZ)
    {
        if (!started)
        {
            StartCoroutine(PauseCoroutine(delay));
            started = true;
        }

        if (pause)
            return;

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


        rb.MovePosition(rb.position + new Vector3(0,0, moveZ));
    }

    IEnumerator PauseCoroutine(float seconds)
    {
        pause = true;
        yield return new WaitForSeconds(seconds);
        pause = false;
    }
}
