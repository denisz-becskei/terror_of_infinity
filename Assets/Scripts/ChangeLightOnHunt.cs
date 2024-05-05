using UnityEngine;

public class ChangeLightOnHunt : MonoBehaviour
{
    [SerializeField] Color onHuntColor;
    private EnemyController enemy;
    private Color originalColor;
    private void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
        originalColor = GetComponent<Light>().color;
    }

    private void Update()
    {
        if(enemy.enemyState == EnemyController.EnemyState.Hunt)
        {
            GetComponent<Light>().color = onHuntColor;
        }
        else
        {
            GetComponent<Light>().color = originalColor;
        }
    }
}
