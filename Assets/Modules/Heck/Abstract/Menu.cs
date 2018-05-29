using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Heck {

    public abstract class Menu : MonoBehaviour {

        protected BaseEventData baseEventData = new BaseEventData(EventSystem.current);

        public void Start() {
            MapNavigationLayout();
        }

        protected void MapNavigationLayout() {
            foreach (NavigationElement element in navigationLayout)
                navigationLayoutMapping[element.element] = element;
        }

        public void Show() {
            gameObject.SetActive(true);
            OnShow();
        }

        public void Hide() {
            OnHide();
            gameObject.SetActive(false);
        }

        protected Character user;
        public void SetUser(Character user) {
            this.user = user;
        }


        public virtual void InputUp() {
            GameObject next = GetNextNavigationElement(NavigationDirection.Up);
            FocusNavigationElement(next);
        }
        public virtual void InputDown() {
            GameObject next = GetNextNavigationElement(NavigationDirection.Down);
            FocusNavigationElement(next);
        }
        public virtual void InputLeft() {
            GameObject next = GetNextNavigationElement(NavigationDirection.Left);
            FocusNavigationElement(next);
        }
        public virtual void InputRight() {
            GameObject next = GetNextNavigationElement(NavigationDirection.Right);
            FocusNavigationElement(next);
        }

        public virtual void InputAction() {
            ExecuteEvents.Execute(currentNavigationElement, baseEventData, ExecuteEvents.submitHandler);
        }
        public virtual void InputBack() {
            Close();
        }
        public virtual void InputAlt() {

        }

        public virtual void OnShow() {
            if (baseNavigationElement == null && navigationLayout.Length > 0)
                baseNavigationElement = navigationLayout[0].element;
            if (currentNavigationElement == null)
                currentNavigationElement = baseNavigationElement;
        }
        public virtual void OnHide() {

        }

        public void Open() {
            if(!gameObject.activeInHierarchy)
                Open(this);
        }
        public void Close() {
            if (gameObject.activeInHierarchy)
                Close(this);
        }

        public static void Open(Menu menu) {
            currentMenu = menu;
            menuStack.Push(menu);
            menu.Show();
        }

        public static void Close(Menu menu) {
            menuStack.Pop().Hide();
            if (menuStack.Count > 0) {
                currentMenu = menuStack.Peek();
                currentMenu.RecoverFocus();
            } else
                currentMenu = null;
        }

        public static Stack<Menu> menuStack = new Stack<Menu>();
        public static Menu currentMenu;
        
        // when menu abbove this one is unstacked (closed)
        public virtual void RecoverFocus() {

        }

        public void Update() {
            if (this == currentMenu) {
                if (UnityEngine.Input.GetKeyDown(KeyCode.Return))
                    InputAction();
                if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.Backspace))
                    InputBack();
                if (UnityEngine.Input.GetKeyDown(KeyCode.LeftAlt) || UnityEngine.Input.GetKeyDown(KeyCode.RightAlt))
                    InputAlt();
                if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
                    InputUp();
                if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
                    InputDown();
                if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
                    InputLeft();
                if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
                    InputRight();
            }
        }

        public GameObject baseNavigationElement = null;
        protected GameObject currentNavigationElement = null;
        public NavigationElement[] navigationLayout = new NavigationElement[0];
        protected Dictionary<GameObject, NavigationElement> navigationLayoutMapping = new Dictionary<GameObject, NavigationElement>();

        public GameObject GetNextNavigationElement(NavigationDirection direction) {
            NavigationElement nav = navigationLayoutMapping[currentNavigationElement];
            GameObject next = null;
            NavigationElement tryNext = nav;
            switch (direction) {
                case NavigationDirection.Down:
                    if (tryNext.down != null && tryNext.down.activeInHierarchy) {
                        next = tryNext.down;
                    } else next = currentNavigationElement;
                    break;
                case NavigationDirection.Left:
                    if (tryNext.left != null && tryNext.left.activeInHierarchy) {
                        next = tryNext.left;
                    } else next = currentNavigationElement;
                    break;
                case NavigationDirection.Right:
                    if (tryNext.right != null && tryNext.right.activeInHierarchy) {
                        next = tryNext.right;
                    } else next = currentNavigationElement;
                    break;
                case NavigationDirection.Up:
                    if (tryNext.up != null && tryNext.up.activeInHierarchy) {
                        next = tryNext.up;
                    } else next = currentNavigationElement;
                    break;
            }
            return next != null ? next : currentNavigationElement;
        }

        public void FocusNavigationElement(GameObject element) {
            currentNavigationElement = element;
            element.GetComponent<Selectable>().Select();
        }

    }

    public enum NavigationDirection {
        Up, Down, Left, Right
    }

    [System.Serializable]
    public class NavigationElement {
        public GameObject element;
        public GameObject left = null;
        public GameObject right = null;
        public GameObject up = null;
        public GameObject down = null;
    }
}
