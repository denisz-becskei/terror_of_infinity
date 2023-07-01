using System.Collections.Generic;
using UnityEngine;
using static IntVector2Init;

public static class GalaxyWideScripts
{
    public static T[] FindComponentsInChildrenWithTag<T>(this GameObject parent, string tag, bool forceActive = false) where T : Component
    {
        if (parent == null) { throw new System.ArgumentNullException(); }
        if (string.IsNullOrEmpty(tag) == true) { throw new System.ArgumentNullException(); }
        List<T> list = new List<T>(parent.GetComponentsInChildren<T>(forceActive));
        if (list.Count == 0) { return null; }

        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].CompareTag(tag) == false)
            {
                list.RemoveAt(i);
            }
        }
        return list.ToArray();
    }

    public static bool TryGetComponentInChildren<T>(this GameObject parent, out T? component) where T : Component
    {
        foreach(Transform tf in parent.transform)
        {
            if(tf.TryGetComponent(out T test))
            {
                if(test != null)
                {
                    component = test;
                    return true;
                } 
            }
        }
        component = null;
        return false;
    }

    public static float DistanceBetweenPoints(this IntVector2 vector1, IntVector2 vector2)
    {
        return Mathf.Sqrt((vector2.x - vector1.x) * (vector2.x - vector1.x) + (vector2.y - vector1.y) * (vector2.y - vector1.y));
    }
}
