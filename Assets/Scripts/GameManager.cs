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

        public bool started { get; private set; }

        public void Init(int level)
        {
            snake_ = Instantiate(snakePrefab, Vector3.zero, Quaternion.identity);
            LevelGenerator.m.Init(snake_, level);
            Camera.main.GetComponent<CameraManager>().Init();
        }

        public void Play()
        {
            started = true;
        }

        public void Stop()
        {
            started = false;
        }
    }
}
