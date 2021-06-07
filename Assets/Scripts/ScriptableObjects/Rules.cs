using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace svb
{
    [CreateAssetMenu(fileName = "New Rules", menuName = "New ScriptableObject/Rules")]
    public class Rules : ScriptableObject
    {
        [Header("Snake")]
        public float verticalSpeed = 8;
        public float horizontalSpeed = 8;

        public float delayBetweenParts = 0.03f;

        public int defaultFollowerAmount = 4;
        public float destructionDelay = 0.12f;

        [Header("Camera")]
        public float cameraMaxDistance = 2;
        public float cameraSyncDist = 0.1f;
    }
}
