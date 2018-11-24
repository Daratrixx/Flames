using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
    public abstract class UIView<T> : MonoBehaviour where T : class, UIModelInterface {

        internal T dataSource;

        public virtual void CreateView(T dataSource) {
            this.dataSource = dataSource;
            dataSource.RegisterUpdateListener(UpdateView);
            dataSource.RegisterDeleteListener(DeleteView);
            UpdateView();
        }

        public abstract void UpdateView();

        public virtual void DeleteView() {
            dataSource.UnregisterUpdateListener(UpdateView);
            dataSource.UnregisterDeleteListener(DeleteView);
            Destroy(this);
        }

        public void ShowView() {
            gameObject.SetActive(true);
        }

        public void HideView() {
            gameObject.SetActive(false);
        }

        public void ToggleView() {
            gameObject.SetActive(!gameObject.activeSelf);
        }

    }
}
