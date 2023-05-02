using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameManager gameManager;

    private void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.DecreaseMonsterCount();
        }
    }
}
