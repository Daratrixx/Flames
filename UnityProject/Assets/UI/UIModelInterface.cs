using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void EmptyDelegate();

public interface UIModelInterface {

    void FireUpdate();
    void FireDelete();

    void RegisterUpdateListener(EmptyDelegate del);
    void RegisterDeleteListener(EmptyDelegate del);

    void UnregisterUpdateListener(EmptyDelegate del);
    void UnregisterDeleteListener(EmptyDelegate del);

}

public abstract class UIModel : UIModelInterface {

    public EmptyDelegate OnUpdate;
    public EmptyDelegate OnDelete;

    public void FireUpdate() {
        if (OnUpdate != null)
            OnUpdate();
    }

    public void FireDelete() {
        if (OnDelete != null)
            OnDelete();
    }

    public void RegisterUpdateListener(EmptyDelegate del) {
        OnUpdate += del;
    }

    public void RegisterDeleteListener(EmptyDelegate del) {
        OnDelete += del;
    }

    public void UnregisterUpdateListener(EmptyDelegate del) {
        OnUpdate -= del;
    }

    public void UnregisterDeleteListener(EmptyDelegate del) {
        OnDelete -= del;
    }
}

[Serializable]
public abstract class UIModelList<T> : List<T>, UIModelInterface where T : UIModelInterface {

    public EmptyDelegate OnUpdate;
    public EmptyDelegate OnDelete;

    public new void Add(T item) {
        base.Add(item);
        FireUpdate();
    }

    /*public new void Remove(T item) {
        base.Remove(item);
        FireUpdate();
    }*/

    public void FireUpdate() {
        if (OnUpdate != null)
            OnUpdate();
    }

    public void FireDelete() {
        if (OnDelete != null)
            OnDelete();
    }

    public void RegisterUpdateListener(EmptyDelegate del) {
        OnUpdate += del;
    }

    public void RegisterDeleteListener(EmptyDelegate del) {
        OnDelete += del;
    }

    public void UnregisterUpdateListener(EmptyDelegate del) {
        OnUpdate -= del;
    }

    public void UnregisterDeleteListener(EmptyDelegate del) {
        OnDelete -= del;
    }
}