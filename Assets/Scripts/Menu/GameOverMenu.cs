using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace svb
{
    public class GameOverMenu : Menu
    {
        protected override void OnShowUp()
        {
            foreach (var text in GetComponentsInChildren<Text>())
            {
                text.color = LevelGenerator.m.level.colorSet.walls;
            }
        }

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
