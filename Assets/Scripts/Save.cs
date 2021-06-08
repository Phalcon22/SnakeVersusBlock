using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Save
{
    const string unlockedPref = "UnlockedLevel";
    const string choosedPref = "ChoosedLevel";

    public static int GetUnlocked()
    {
        if (!PlayerPrefs.HasKey(unlockedPref))
            PlayerPrefs.SetInt(unlockedPref, 0);

        return PlayerPrefs.GetInt(unlockedPref);
    }

    public static int GetChoosed()
    {
        if (!PlayerPrefs.HasKey(choosedPref))
            PlayerPrefs.SetInt(choosedPref, 0);

        return PlayerPrefs.GetInt(choosedPref);
    }

    public static void IncrementUnlocked()
    {
        PlayerPrefs.SetInt(unlockedPref, GetUnlocked() + 1);
    }

    public static void IncrementChoosed()
    {
        PlayerPrefs.SetInt(choosedPref, GetChoosed() + 1);
    }

    public static void SetChoosed(int value)
    {
        PlayerPrefs.SetInt(choosedPref, value);
    }
}
