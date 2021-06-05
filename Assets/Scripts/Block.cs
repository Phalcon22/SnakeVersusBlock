using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace svb
{
    public class Block : MonoBehaviour
    {
        public Text text;

        public int amount;

        void Start()
        {
            text.text = amount.ToString();
        }

        public void Consume()
        {
            amount--;
            text.text = amount.ToString();

            if (amount <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

}
