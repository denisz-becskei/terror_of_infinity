using UnityEngine;

[CreateAssetMenu(fileName = "SurfaceSound", menuName = "ScriptableObjects/SurfaceSoundScriptableObject")]
public class SurfaceSoundScriptableObject : ScriptableObject
{
    public Material material;

    [Header("StepSound")]
    public AudioClip stepSound;
    [Header("LandingSound")]
    public AudioClip landingSound1;
    public AudioClip landingSound2;
    public AudioClip landingSound3;
}
