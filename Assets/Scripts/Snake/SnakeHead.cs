using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace svb
{
    public class SnakeHead : SnakePart
    {
        float startSnakeX;
        float startMouseX;

        float targetX;

        Snake snake;
        BoxCollider col_;
        Rigidbody rb_;

        string[] obstacleLayers = { "Wall", "Border", "Block" };

        [HideInInspector]
        public List<Vector3> posHistory = new List<Vector3>();
        [HideInInspector]
        public List<float> deltasHistory = new List<float>();
        public List<float> speedHistory = new List<float>();

        bool pauseY = false;

        bool turbo;

        public Text nbPartsText;

        public void Init(Snake snake)
        {
            turbo = false;

            this.snake = snake;
            rb_ = GetComponent<Rigidbody>();
            col_ = GetComponent<BoxCollider>();

            transform.SetParent(snake.transform);

            GetComponent<MeshRenderer>().material.color = snake.GetNewColor();

            float height = GameManager.m.rules.verticalSpeed * Time.deltaTime;
            Vector3 pos = rb_.position;
            for (int i = 0; i < 2500; i++)
            {
                pos.z -= height;
                posHistory.Insert(0, pos);
                deltasHistory.Insert(0, Time.deltaTime);
                speedHistory.Insert(0, 1);
            }
        }

        public void UpdateMouse()
        {
            if (Input.GetMouseButtonDown(0))
                OnButtonDown();
            else if (Input.GetMouseButton(0))
                OnButtonHoldDown();
        }

        void Translate(Vector3 translation)
        {
            rb_.MovePosition(rb_.position + translation);
        }

        public Vector3 GetPos()
        {
            return rb_.position;
        }

        public float Move()
        {
            Vector3 translation = new Vector3(GetXTranslation(), 0, GetYTranslation());
            Vector3 h = new Vector3(translation.x, 0, 0);
            Vector3 v = new Vector3(0, 0, translation.z);

            Translate(h);
            var horizontalColliders = Physics.OverlapBox(col_.bounds.center, col_.bounds.extents, Quaternion.identity, LayerMask.GetMask(obstacleLayers));
            if (horizontalColliders.Length > 0)
            {
                Translate(-h);
                targetX = rb_.position.x;
                translation.x = 0;
            }

            Translate(v);
            var verticalColliders = Physics.OverlapBox(col_.bounds.center, col_.bounds.extents, Quaternion.identity, LayerMask.GetMask(obstacleLayers));
            if (verticalColliders.Length > 0)
            {
                Translate(-v);
                translation.z = 0;
            }

            var powerUpColliders = Physics.OverlapBox(col_.bounds.center, col_.bounds.extents, Quaternion.identity, LayerMask.GetMask("PowerUp"));
            foreach (Collider powerUp in powerUpColliders)
            {
                snake.AddPart(powerUp.transform.parent.GetComponent<PowerUp>().amount);
                powerUp.transform.parent.gameObject.SetActive(false);
            }

            bool removePart = false;
            foreach (Collider col in verticalColliders)
            {
                if (LayerMask.LayerToName(col.gameObject.layer) == "Block")
                {
                    removePart = true;
                    col.transform.parent.GetComponent<Block>().Consume();
                    break;
                }
            }

            if (removePart && snake.snakeParts.Count > 0)
            {
                snake.RemovePart();

                SnakePart toRemove = snake.snakeParts[0];

                rb_.position = toRemove.GetComponent<Rigidbody>().position;

                int amount = (posHistory.Count - 1) - (toRemove.GetMoveIndex() - 1);
                posHistory.RemoveRange(toRemove.GetMoveIndex(), amount);
                deltasHistory.RemoveRange(toRemove.GetMoveIndex(), amount);
                speedHistory.RemoveRange(toRemove.GetMoveIndex(), amount);

                posHistory[toRemove.GetMoveIndex() - 1] = rb_.position;
                float newDelta = deltasHistory[deltasHistory.Count - 1] - toRemove.remainingDelta;
                deltasHistory[deltasHistory.Count - 1] = newDelta;

                GetComponent<MeshRenderer>().material.color = toRemove.GetComponent<MeshRenderer>().material.color;

                snake.snakeParts.RemoveAt(0);
                Destroy(toRemove.gameObject);

                nbPartsText.text = snake.snakeParts.Count.ToString();

                StartCoroutine(PauseCoroutine(GameManager.m.rules.destructionDelay));
            }
            else
            {
                posHistory.Add(rb_.position);
                deltasHistory.Add(Time.deltaTime);
                speedHistory.Add(translation.z / Time.deltaTime / GameManager.m.rules.verticalSpeed);
            }

            return translation.z / Time.deltaTime;
        }

        float GetMouseX()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.transform.position.y - rb_.position.y;
            return Camera.main.ScreenToWorldPoint(mousePos).x;
        }

        void OnButtonDown()
        {
            startMouseX = GetMouseX();
            startSnakeX = rb_.position.x;
        }

        void OnButtonHoldDown()
        {
            float endMouseX = GetMouseX();
            targetX = startSnakeX + (endMouseX - startMouseX);
        }

        float GetXTranslation()
        {
            Vector3 pos = rb_.position;

            float direction = (pos.x < targetX) ? 1 : -1;
            float translation = direction * GameManager.m.rules.horizontalSpeed * Time.deltaTime;

            if (direction < 0 && pos.x + translation < targetX || direction > 0 && pos.x + translation > targetX)
            {
                translation = targetX - pos.x;
            }

            return translation;
        }

        float GetYTranslation()
        {
            float y = GameManager.m.rules.verticalSpeed * Time.deltaTime;

            if (pauseY)
                y *= GameManager.m.rules.destructionSlowDown;

            if (turbo)
                y *= 2;

            return y;
        }

        IEnumerator PauseCoroutine(float seconds)
        {
            pauseY = true;
            yield return new WaitForSeconds(seconds);
            pauseY = false;
        }

        public void ActivateTurbo()
        {
            turbo = true;
        }

        public void DeactivateTurbo()
        {
            turbo = false;
        }
    }
}
