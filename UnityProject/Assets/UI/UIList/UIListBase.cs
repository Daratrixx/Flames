using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UI {

    public abstract class UIListBase<T> : UIView<UIModelList<T>> where T : class, UIModelInterface {

        public GameObject listElementPrefab;
        public RectTransform listContainer;

        public List<UIListElementBase<T>> listElements = new List<UIListElementBase<T>>();

        public override void UpdateView() {
            RebuildList();
        }

        public void RebuildList() {
            foreach (var e in dataSource) {
                if (listElements.Where(x => x.dataSource == e).Count() == 0) {
                    UIListElementBase<T> obj;
                    listElements.Add(obj = Instantiate(listElementPrefab, listContainer).GetComponent<UIListElementBase<T>>());
                    obj.parentList = this;
                    obj.CreateView(e);
                }
            }
        }

    }

}
