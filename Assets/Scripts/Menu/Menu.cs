using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace svb
{
    public class Menu : MonoBehaviour
    {
        protected GameObject holder;

        void Start()
        {
            holder = transform.GetChild(0).gameObject;
            OnStart();
        }

        protected virtual void OnStart() { }

        public void ShowUp()
        {
            holder.SetActive(true);
            OnShowUp();
        }

        protected virtual void OnShowUp() { }

        public void Hide()
        {
            holder.SetActive(false);
            OnHide();
        }

        protected virtual void OnHide() { }

        public void GoTo(Menu menu)
        {
            Hide();
            menu.ShowUp();
        }

        protected bool Hidden()
        {
            return !holder.activeInHierarchy;
        }
    }
}
