using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace svb
{
    [CreateAssetMenu(fileName = "New ColorSet", menuName = "New ScriptableObject/ColorSet")]
    public class ColorSet : ScriptableObject
    {
        public Color image = Color.white;
        public Color background = Color.white;
        public Color walls = Color.white;

        public Color[] blocks;
    }
}
