using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace svb
{
    [CreateAssetMenu(fileName = "New ColorSet", menuName = "New ScriptableObject/ColorSet")]
    public class ColorSet : ScriptableObject
    {
        public Color font;
        public Color image;
        public Color background;
        public Color walls;

        public Color[] blocks;
    }
}
