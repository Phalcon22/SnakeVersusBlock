using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace svb
{
    [CreateAssetMenu(fileName = "New SnakeColors", menuName = "New ScriptableObject/SnakeColors")]
    public class SnakeColors : ScriptableObject
    {
        public Color[] colors;
    }
}
