using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace svb
{
    public class LevelMenu : Menu
    {
        [SerializeField]
        Transform content;

        [SerializeField]
        ChooseLevelButton buttonPrefab;

        protected override void OnStart()
        {
            for (int i = 0; i < LevelGenerator.m.levels.Length; i++)
            {
                ChooseLevelButton button = Instantiate(buttonPrefab, content);
                button.Init(i, i <= Save.GetUnlocked());
            }
        }
    }
}
