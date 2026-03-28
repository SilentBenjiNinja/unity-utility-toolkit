using UnityEngine;

namespace bnj.utility_toolkit.Runtime.Debug
{
    /// <summary>Editor and runtime debugging helpers.</summary>
    public static class DebugUtils
    {
        /// <summary>
        /// Returns the full hierarchy path of <paramref name="gameObject"/> from the scene root,
        /// e.g. <c>"Player/Visuals/Mesh"</c>.
        /// </summary>
        public static string GetFullHierarchyName(this GameObject gameObject)
        {
            if (gameObject.transform.parent == null) return gameObject.name;
            return $"{gameObject.transform.parent.gameObject.GetFullHierarchyName()}/{gameObject.name}";
        }
    }
}
