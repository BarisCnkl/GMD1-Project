using UnityEngine;

public class StoneDamage : MonoBehaviour
{
    private StoneState stoneState;

    private void Awake()
    {
        stoneState = GetComponent<StoneState>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (stoneState == null) return;
        if (!stoneState.IsThrown) return;

        EnemyMovement enemy = collision.gameObject.GetComponentInParent<EnemyMovement>();

        if (enemy != null)
        {
            GameUI gameUI = FindFirstObjectByType<GameUI>();
            if (gameUI != null)
            {
                gameUI.AddKill();
            }

            Destroy(enemy.gameObject);
            Destroy(gameObject);
        }
    }
}