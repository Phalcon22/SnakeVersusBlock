using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    List<SnakePart> snakeParts = new List<SnakePart>();

    public SnakeHead headPrefab;
    public SnakePart partPrefab;

    public static float verticalSpeed = 0.24f;
    public static float horizontalSpeed = 0.60f;

    void Start()
    {
        SnakeHead head = Instantiate(headPrefab, Vector3.zero, Quaternion.identity);

        head.Init();
        snakeParts.Add(head);
        head.transform.SetParent(transform);

        for (int i = 0; i < 10; i++)
            AddPart();
    }

    void Update()
    {
        snakeParts[0].GetComponent<SnakeHead>().UpdateMouse();
    }

    void FixedUpdate()
    {
        Move();
    }

    void AddPart()
    {
        SnakePart newPart = Instantiate(partPrefab, snakeParts[snakeParts.Count - 1].transform.position, snakeParts[snakeParts.Count - 1].transform.rotation);

        newPart.transform.SetParent(transform);
        snakeParts.Add(newPart);
    }

    public Vector3 GetPos()
    {
        if (snakeParts.Count == 0)
            return Vector3.zero;

        return snakeParts[0].GetComponent<Rigidbody>().position;
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

        for (int i = 1; i < snakeParts.Count; i++)
        {
            Transform cur = snakeParts[i].transform;
            Transform prev = snakeParts[i - 1].transform;

            if (i - 1 == 0)
            {
                prev = snakeParts[i - 1].GetComponent<Rigidbody>().transform;
            }

            Vector3 newPos = prev.position;

            /*if (!moveZ)
                newPos.z = cur.position.z;*/

            float dist = Vector3.Distance(prev.position, cur.position);

            float T = dist / 0.1f * verticalSpeed;

            if (T > 0.5f)
                T = 0.5f;

            cur.position = Vector3.Slerp(cur.position, newPos, T);
        }
    }
}
