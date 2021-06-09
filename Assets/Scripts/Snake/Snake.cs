using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace svb
{
    public class Snake : MonoBehaviour
    {
        SnakeHead head = null;
        public List<SnakePart> snakeParts = new List<SnakePart>();

        [SerializeField]
        SnakeHead headPrefab;
        [SerializeField]
        SnakePart partPrefab;

        int followers;

        [SerializeField]
        SnakeColors colors;

        System.Random rng = new System.Random();

        bool pause = true;

        public bool turbo { get; private set; }

        public void Init()
        {
            turbo = false;

            followers = 0;

            head = Instantiate(headPrefab, Vector3.zero, Quaternion.identity);

            head.Init(this);

            for (int i = 0; i < GameManager.m.rules.defaultFollowerAmount; i++)
                AddPart();

            head.nbPartsText.text = snakeParts.Count.ToString();
            head.nbPartsText.color = LevelGenerator.m.level.colorSet.walls;

            StartCoroutine(PauseCoroutine(1));
        }

        void Update()
        {
            if (!GameManager.m.started)
                return;

            head.UpdateMouse();

            if (pause)
                return;

            Move();
        }

        public int GetFollower()
        {
            return followers;
        }

        public void AddPart(int amount)
        {
            for (int i = 0; i < amount; i++)
                AddPart();
        }

        int deltaIndex = 0;
        public void AddPart()
        {
            SnakePart prev = head;
            if (snakeParts.Count > 0)
                prev = snakeParts[snakeParts.Count - 1];

            Vector3 prevPos = prev.GetComponent<Rigidbody>().position;
            prevPos.z -= prev.GetComponent<BoxCollider>().bounds.extents.z * 2;

            followers++;
            SnakePart newPart = Instantiate(partPrefab);
            newPart.transform.SetParent(transform);
            newPart.Init(head, prev, ++deltaIndex);

            snakeParts.Add(newPart);

            head.nbPartsText.text = snakeParts.Count.ToString();
        }

        bool reMove = false;
        public void RemovePart()
        {
            if (snakeParts.Count == 0)
            {
                if (!GameManager.m.rules.godMode)
                {
                    GameManager.m.Stop();
                    head.GetComponent<MeshRenderer>().enabled = false;
                    GameObject.Find("GameOverMenu").GetComponent<GameOverMenu>().ShowUp();
                }
                return;
            }

            followers--;
            reMove = true;
        }

        void Move()
        {
            var speed = head.Move();

            if (reMove)
            {
                speed = head.Move();
                reMove = false;
            }


            for (int i = 0; i < followers; i++)
            {
                snakeParts[i].Move(speed / GameManager.m.rules.verticalSpeed);
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

        public Color GetNewColor()
        {
            int index = rng.Next(0, colors.colors.Length);

            return colors.colors[index];
        }

        Coroutine turbo_co;
        public void ActivateTurbo()
        {
            if (turbo)
            {
                StopCoroutine(turbo_co);
            }

            turbo_co = StartCoroutine(TurboCoroutine(8));
        }

        IEnumerator TurboCoroutine(float seconds)
        {
            head.ActivateTurbo();
            
            turbo = true;
            yield return new WaitForSeconds(seconds);
            turbo = false;

            head.DeactivateTurbo();
        }
    }
}
