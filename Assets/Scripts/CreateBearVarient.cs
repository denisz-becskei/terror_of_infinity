using System.Collections.Generic;
using UnityEngine;

public class CreateBearVarient : MonoBehaviour
{
    [SerializeField] Material blackMat;
    [SerializeField] List<Material> eyeColor;
    [SerializeField] List<Vector3> colors;
    [SerializeField] uint eyeMatIndex;

    public bool possessed = false;

    private void Start()
    {
        int toAdd = Mathf.RoundToInt(eyeColor.Count / 2);
        for (int i = 0; i < toAdd; i++)
        {
            eyeColor.Add(blackMat);
        }

        MeshRenderer[] teddyObjects = GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer teddy in teddyObjects)
        {
            if (teddy.gameObject.CompareTag("Bow")) continue;
            int selectedIndex = Random.Range(0, eyeColor.Count);
            Material selected = eyeColor[selectedIndex];
            if (selected != blackMat)
            {
                possessed = true;
                GetComponent<AttuneLightSource>().Generate(colors[selectedIndex]);
            } else
            {
                Destroy(GetComponent<AttuneLightSource>());
                Destroy(GetComponentInChildren<Light>().gameObject);
            }
            var mats = teddy.materials;
            mats[eyeMatIndex] = selected;
            teddy.materials = mats;
        }
    }
}
