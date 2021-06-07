using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace svb
{
    [CreateAssetMenu(fileName = "New Level", menuName = "New ScriptableObject/Stats")]
    public class Stats : ScriptableObject
    {
        [Header("PowerUp")]
        public float powerUps = 7.5f;
        [Space(10)]
        public int powerUpsMinValue = 1;
        public int powerUpsMaxValue = 5;

        [Header("Block"), Space(20)]
        public int blocksMinValue = 1;
        public int blocksMaxValue = 50;
        [Space(10)]
        public float OneBlockPercent = 40;
        public float TwoBlockPercent = 38;
        public float FiveBlockPercent = 22;
        [Space(10)]
        public float blockFollowsWall = 79;

        [Header("Walls"), Space(20)]
        public float shortWallPercent = 50;
        [Space(10)]
        public float ZeroWallPercent = 32;
        public float OneWallPercent = 44;
        public float TwoWallPercent = 19;
        [Space(10)]
        public float wallFollowsBlock = 62;
    }
}
