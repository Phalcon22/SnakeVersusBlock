using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace svb
{
    public class Block : MonoBehaviour
    {
        public Text text;

        public int amount { get; private set; }

        void Start()
        {
            text.color = LevelGenerator.m.level.colorSet.background;
        }

        public void Consume()
        {
            amount--;
            text.text = amount.ToString();

            if (amount <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        public void SetAmount(int amount)
        {
            this.amount = amount;
            text.text = amount.ToString();

            var colors = LevelGenerator.m.level.colorSet.blocks;
            float trunc = 50f / (colors.Length - 1);
            var t = amount / trunc;
            int a = (int)t;
            int b = Mathf.Clamp(a + 1, 0, colors.Length - 1);
            GetComponentInChildren<MeshRenderer>().material.color = Color.Lerp(colors[a], colors[b], (t - a) / trunc );
        }
    }
}
