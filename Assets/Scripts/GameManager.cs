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
    }
}
