using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace svb
{
    public class PowerUp : MonoBehaviour
    {
        public int amount { get; private set; }

        [SerializeField]
        Text text;

        public void Init(int amount)
        {
            this.amount = amount;
            text.text = amount.ToString();
            text.color = LevelGenerator.m.level.colorSet.walls;
            GetComponentInChildren<MeshRenderer>().material.color = LevelGenerator.m.level.colorSet.walls;
        }
    }
}
