using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace svb
{
    public class Arrival : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<SnakeHead>())
            {
                GameObject.Find("WinMenu").GetComponent<WinMenu>().ShowUp();
            }
        }
    }
}
