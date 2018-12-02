using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHealthBar : UI.UIView<CombatUnit> {

    public Slider slider;

    public override void CreateView(CombatUnit dataSource) {
        base.CreateView(dataSource);
        dataSource.RegisterDamagedListener(UpdateFill);
        dataSource.RegisterHealedListener(UpdateFill);
        UpdateFill();
    }

    public override void UpdateView() {
        UpdateFill();
    }

    public void UpdateFill() {
        slider.value = 100*(float)dataSource.currentHealth / dataSource.maxHealth;
    }

}
