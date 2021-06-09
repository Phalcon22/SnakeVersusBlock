using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace svb
{
    public class Block : MonoBehaviour
    {
        [SerializeField]
        Text text;
        [SerializeField]
        Image star;

        public int amount { get; private set; }

        bool turbo;

        void Start()
        {
            text.color = LevelGenerator.m.level.colorSet.background;
            star.color = LevelGenerator.m.level.colorSet.background;
        }

        public void Consume()
        {
            amount--;
            text.text = amount.ToString();
            UpdateColor();

            if (amount <= 0 || GameManager.m.snake.turbo)
            {
                if (turbo)
                {
                    GameManager.m.ActivateTurbo();
                }
                gameObject.SetActive(false);
            }
        }

        public void SetAmount(int amount)
        {
            this.amount = amount;
            text.text = amount.ToString();

            UpdateColor();

            if (amount == GameManager.m.turbo)
            {
                turbo = true;
            }
            else
            {
                turbo = false;
                star.gameObject.SetActive(false);
            }
        }

        public void UpdateColor()
        {
            var colors = LevelGenerator.m.level.colorSet.blocks;
            float trunc = 51f / (colors.Length - 1);
            var t = amount / trunc;
            int a = (int)t;
            int b = a + 1;
            GetComponentInChildren<MeshRenderer>().material.color = Color.Lerp(colors[a], colors[b], (t - a));
        }
    }
}
