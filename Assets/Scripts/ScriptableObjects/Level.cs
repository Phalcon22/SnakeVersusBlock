using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace svb
{
    [CreateAssetMenu(fileName = "New Level", menuName = "New ScriptableObject/Level")]
    public class Level : ScriptableObject
    {
        public ColorSet colorSet;
        public int length = 50;
    }
}
