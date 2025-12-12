using UnityEngine;

namespace bnj.utility_toolkit.Runtime
{
    public static class ComponentUtils
    {
        public static GameObject GetChildGameObject(this Component parent, string path) =>
            parent.transform.Find(path).gameObject;

        public static T GetComponentFromChild<T>(this Component parent, string path) where T : Component =>
            parent.transform.Find(path).GetComponent<T>();

        public static T[] GetComponentsInChildrenFromChild<T>(this Component parent, string path) where T : Component =>
            parent.transform.Find(path).GetComponentsInChildren<T>();
    }
}