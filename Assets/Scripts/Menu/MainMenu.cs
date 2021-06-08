using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace svb
{
    public class MainMenu : Menu
    {
        [SerializeField]
        Text levelText;

        [SerializeField]
        GameObject infinityObj;

        protected override void OnStart()
        {
            ShowUp();

            int level = Save.GetChoosed();

            if (level == -1)
                infinityObj.SetActive(true);
            else
            {
                levelText.gameObject.SetActive(true);
                levelText.text = "Level " + (level + 1).ToString();
            }

            GameManager.m.Init(level);

            foreach (var text in GetComponentsInChildren<Text>())
            {
                text.color = LevelGenerator.m.level.colorSet.font;
            }

            foreach (var image in GetComponentsInChildren<Image>())
            {
                image.color = LevelGenerator.m.level.colorSet.image;
            }

        }

        public void StartButton()
        {
            GameManager.m.Play();
            Hide();
        }

        public void InfinityButton()
        {
            Save.SetChoosed(-1);
            SceneManager.LoadScene("Level");
            Hide();
        }
    }
}
