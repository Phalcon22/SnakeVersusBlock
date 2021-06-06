using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace svb
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton

        public static GameManager m;

        void Awake()
        {
            m = this;
        }

        #endregion

        [SerializeField]
        Rules rules_;

        public Rules rules
        {
            get { return rules_; }
        }

        [SerializeField]
        Snake snakePrefab;

        Snake snake_;

        public Snake snake
        {
            get { return snake_; }
        }

        void Start()
        {
            snake_ = Instantiate(snakePrefab, Vector3.zero, Quaternion.identity);
            LevelGenerator.m.Init(snake_, 0);
        }
    }
}
