using UnityEngine;

namespace bnj.utility_toolkit.Runtime.Debug
{
    public static class DebugUtils
    {
        public static string GetFullHierarchyName(this GameObject gameObject)
        {
            if (gameObject.transform.parent == null) return gameObject.name;
            return $"{gameObject.transform.parent.gameObject.GetFullHierarchyName()}/{gameObject.name}";
        }
    }
}
