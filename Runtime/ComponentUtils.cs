using UnityEngine;

namespace bnj.utility_toolkit.Runtime.Components
{
    // TODO: this might be completely irrelevant with auto fields?
    /// <summary>
    /// Extension methods on <see cref="Component"/> for retrieving child GameObjects and components by transform path.
    /// </summary>
    public static class ComponentUtils
    {
        /// <summary>Returns the <see cref="GameObject"/> at <paramref name="path"/> relative to <paramref name="parent"/>.</summary>
        public static GameObject GetChildGameObject(this Component parent, string path) =>
            parent.transform.Find(path).gameObject;

        /// <summary>Returns the first component of type <typeparamref name="T"/> on the child at <paramref name="path"/>.</summary>
        public static T GetComponentFromChild<T>(this Component parent, string path) where T : Component =>
            parent.transform.Find(path).GetComponent<T>();

        /// <summary>Returns all components of type <typeparamref name="T"/> on or under the child at <paramref name="path"/>.</summary>
        public static T[] GetComponentsInChildrenFromChild<T>(this Component parent, string path) where T : Component =>
            parent.transform.Find(path).GetComponentsInChildren<T>();
    }
}