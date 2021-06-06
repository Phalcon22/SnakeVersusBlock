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

        public void Consume()
        {
            SetAmount(amount - 1);

            if (amount <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        public void SetAmount(int amount)
        {
            this.amount = amount;
            text.text = amount.ToString();
        }
    }
}
