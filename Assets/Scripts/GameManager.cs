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

        [SerializeField]
        GameObject prefab;

        public Rules rules
        {
            get { return rules_; }
        }

        [SerializeField]
        Snake snakePrefab;
        int level = 1;

        [SerializeField]
        Transform stage;

        Snake snake_;

        public Snake snake
        {
            get { return snake_; }
        }

        void Start()
        {
            GenerateLevel();
            snake_ = Instantiate(snakePrefab, Vector3.zero, Quaternion.identity);
        }

        void GenerateLevel()
        {
            Vector3 pos = Vector3.zero;
            pos.z = 10;

            for (int i = 0; i < 10; i++)
            {
                Instantiate(prefab, pos, Quaternion.identity, stage);
                pos.z += 1.5f * 5;
            }
        }
    }
}
