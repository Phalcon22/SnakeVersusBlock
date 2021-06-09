using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace svb
{
    public class RatioForcer : MonoBehaviour
    {
        float newAspectRatio = 0.5625f;

        void Start()
        {
            var camera = GetComponent<Camera>();
            var variance = newAspectRatio / camera.aspect;
            if (variance < 1.0)
                camera.rect = new Rect((1 - variance) / 2f, 0, variance, 1);
            else
            {
                variance = 1.0f / variance;
                camera.rect = new Rect(0, (1 - variance) / 2f, 1, variance);
            }
        }
    }
}
