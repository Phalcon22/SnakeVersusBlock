using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace svb
{
    public class WinMenu : Menu
    {
        protected override void OnShowUp()
        {
            StartCoroutine(DelayCoroutine());
        }

        IEnumerator DelayCoroutine()
        {
            yield return new WaitForSeconds(2f);

            Hide();

            int unlocked = Save.GetUnlocked();
            int choosed = Save.GetChoosed();
            if (choosed >= unlocked)
                Save.IncrementUnlocked();

            Save.IncrementChoosed();

            SceneManager.LoadScene("Level");
        }
    }
}
