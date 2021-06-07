using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace svb
{
    public class Wall : MonoBehaviour
    {
        void Start()
        {
            GetComponentInChildren<MeshRenderer>().material.color = LevelGenerator.m.level.colorSet.walls;
        }
    }
}
