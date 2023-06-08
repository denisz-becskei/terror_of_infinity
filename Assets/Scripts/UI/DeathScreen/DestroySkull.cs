using UnityEngine;

public class DestroySkull : MonoBehaviour
{
    private AudioSource skullCrackAudio;

    private void Start()
    {
        skullCrackAudio = GameObject.FindGameObjectWithTag("YouDiedUIElement").GetComponent<AudioSource>();
    }

    public void Destroy(uint index)
    {
        Transform[] skulls = GetComponentsInChildren<Transform>();
        skullCrackAudio.Play();
        skulls[index].GetComponent<Explode2DSprite>().Explode();
    }
}
