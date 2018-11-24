using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {

    public abstract class UIListElementBase<T> : UIView<T> where T : class, UIModelInterface {
        public UIListBase<T> parentList;

        public override void CreateView(T dataSource) {
            base.CreateView(dataSource);
        }

        public override void DeleteView() {
            base.DeleteView();
            parentList.listElements.Remove(this);
        }

    }

}
