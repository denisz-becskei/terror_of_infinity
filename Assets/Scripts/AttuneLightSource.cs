using System.Collections.Generic;
using UnityEngine;

public class AttuneLightSource : MonoBehaviour
{ 
    [SerializeField] List<Vector3> colors;
    [SerializeField] Light lightSource;


    public void Generate(Vector3 color)
    {
        lightSource.color = new Color(color.x, color.y, color.z, 1);
    }
}
