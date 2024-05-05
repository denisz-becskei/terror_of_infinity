using System;
using UnityEngine;

public class Bit : MonoBehaviour
{
    private GameObject player;
    private PlayerInformation pi;
    private int scoreValue;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pi = player.GetComponent<PlayerInformation>();
    }

    public void SetColor(string color)
    {
        GetComponent<Renderer>().material.color = WorldWideScripts.ToColor(color.ToLower());
        this.scoreValue = WorldWideScripts.GetValueOfColor(color.ToLower());
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject == player)
        {
            pi.currentScore += scoreValue;
            pi.UpdateScore();
            Destroy(this.gameObject);
        }
    }
}
