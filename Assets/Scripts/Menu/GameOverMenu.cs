using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace svb
{
    public class GameOverMenu : Menu
    {
        void Update()
        {
            if (Hidden())
                return;

            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("Level");
            }
        }
    }
}
