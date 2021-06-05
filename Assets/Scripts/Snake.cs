using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    List<SnakePart> snakeParts = new List<SnakePart>();

    public SnakeHead headPrefab;
    public SnakePart partPrefab;

    public static float verticalSpeed = 8f;
    public static float horizontalSpeed = 8f;

    public static int followers = 20;

    void Start()
    {
        SnakeHead head = Instantiate(headPrefab, Vector3.zero, Quaternion.identity);

        head.Init();
        snakeParts.Add(head);
        head.transform.SetParent(transform);

        for (int i = 0; i < followers; i++)
            AddPart((i + 1) * 0.03f);

    }

    int i_ = 0;
    void Update()
    {
        if (i_++ < 10000f * Time.deltaTime)
            return;

        snakeParts[0].GetComponent<SnakeHead>().UpdateMouse();
        Move();
    }

    void FixedUpdate()
    {
    }

    void AddPart(float delay)
    {
        SnakePart newPart = Instantiate(partPrefab, snakeParts[0].transform.position, snakeParts[0].transform.rotation);
        newPart.Init(delay);

        newPart.transform.SetParent(transform);
        snakeParts.Add(newPart);
    }

    public Vector3 GetPos()
    {
        if (snakeParts.Count == 0)
            return Vector3.zero;

        return snakeParts[0].transform.position;
    }

    public Vector3 GetVelocity()
    {
        if (snakeParts.Count == 0)
            return Vector3.zero;

        return snakeParts[0].GetComponent<Rigidbody>().velocity;

    }

    void RemovePart()
    {
        if (snakeParts.Count == 1)
        {
            //loose
            return;
        }

        SnakePart toRemove = snakeParts[1];

        SnakePart first = snakeParts[0];

        first.transform.position = toRemove.transform.position;

        snakeParts.RemoveAt(1);
    }

    void Move()
    {
        bool moveZ = snakeParts[0].GetComponent<SnakeHead>().Move();
        for (int i = 1; i <= followers; i++)
        {
            snakeParts[i].tmp(snakeParts[0].GetComponent<SnakeHead>(), i - 1);
        }
        for (int i = 1; i < snakeParts.Count; i++)
        {

            /*
            var cur = snakeParts[i].GetComponent<Rigidbody>();
            Vector3 prev = snakeParts[i - 1].transform.position;

            Vector3 newPos = prev;

            if (!moveZ)
                newPos.z = cur.position.z;

            float dist = Vector3.Distance(prev, cur.position);

            float T = dist / 0.1f * verticalSpeed * Time.deltaTime;

            if (T > 0.5f)
                T = 0.5f;

            cur.MovePosition(Vector3.Slerp(cur.position, newPos, T));*/
        }
    }
}
