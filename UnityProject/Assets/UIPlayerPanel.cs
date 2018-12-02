using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerPanel : UI.UIView<CombatUnit> {

    public UIPlayerHealthBar healthBar;

    public CombatUnit source;

    private void Start() {
        if(source != null) {
            CreateView(source);
        }
        ShowView();
    }

    public override void CreateView(CombatUnit dataSource) {
        base.CreateView(source);
        healthBar.CreateView(source);
    }

    public override void UpdateView() {

    }

}
