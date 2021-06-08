using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChooseLevelButton : MonoBehaviour
{
    int level;

    [SerializeField]
    Text text;

    public void Init(int level, bool unlocked)
    {
        this.level = level;
        text.text = (level + 1).ToString();
        GetComponent<Button>().interactable = unlocked;
    }

    public void OnClick()
    {
        Save.SetChoosed(level);
        SceneManager.LoadScene("Level");
    }
}
