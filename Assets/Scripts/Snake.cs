using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace svb
{
    public class Snake : MonoBehaviour
    {
        SnakeHead head = null;
        List<SnakePart> snakeParts = new List<SnakePart>();

        public SnakeHead headPrefab;
        public SnakePart partPrefab;

        int followers = 0;

        bool pause = true;
        void Start()
        {
            head = Instantiate(headPrefab, Vector3.zero, Quaternion.identity);

            head.Init(this);

            for (int i = 0; i < GameManager.m.rules.defaultFollowerAmount; i++)
                AddPart();

            StartCoroutine(PauseCoroutine(1));
        }

        void Update()
        {
            head.UpdateMouse();

            if (pause)
                return;

            Move();
        }

        public void AddPart()
        {
            SnakePart prev = head;
            if (snakeParts.Count > 0)
                prev = snakeParts[snakeParts.Count - 1];

            Vector3 prevPos = prev.GetComponent<Rigidbody>().position;
            prevPos.z -= prev.GetComponent<BoxCollider>().bounds.extents.z * 2;

            followers++;
            SnakePart newPart = Instantiate(partPrefab, prevPos, head.transform.rotation);
            newPart.Init(head, followers * GameManager.m.rules.delayBetweenParts);

            newPart.transform.SetParent(transform);
            snakeParts.Add(newPart);
        }

        public void RemovePart()
        {
            if (snakeParts.Count == 0)
            {
                //loose
                return;
            }

            SnakePart toRemove = snakeParts[0];

            head.GetComponent<Rigidbody>().position = toRemove.GetComponent<Rigidbody>().position;

            int amount = head.posHistory.Count - (toRemove.GetMoveIndex() + 1);
            head.posHistory.RemoveRange(toRemove.GetMoveIndex() + 1, amount);
            head.deltasHistory.RemoveRange(toRemove.GetMoveIndex() + 1, amount);

            followers--;
            snakeParts.RemoveAt(0);
            Destroy(toRemove.gameObject);
        }

        void Move()
        {
            var moveZ = head.Move();

            for (int i = 0; i < followers; i++)
            {
                snakeParts[i].Move(i + 1, moveZ);
            }
        }

        public Vector3 GetPos()
        {
            if (head == null)
                return Vector3.zero;

            return head.GetPos();
        }

        IEnumerator PauseCoroutine(float seconds)
        {
            pause = true;
            yield return new WaitForSeconds(seconds);
            pause = false;
        }
    }
}
