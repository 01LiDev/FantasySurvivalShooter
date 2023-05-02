using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] monsterPrefabs;
    [SerializeField] private Terrain terrain;
    [SerializeField] private int maxMonsters;
    [SerializeField] private int maxWaves;
    private int currentWave;
    private int monstersAlive;
    public Text waveText;
    public Text timerText;
    public Text monstersAliveText;
    public bool PlayerDead = false;
    private float elapsedTime;
    public PlayerHealthManager playerHealthManager;
    public Animator playerAnimator;

    private void Start()
    {
        currentWave = 1;
        InitialWave();
        UpdateWaveText();
        playerHealthManager = FindObjectOfType<PlayerHealthManager>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Get the Animator component from the player GameObject
        if (player != null)
        {
            playerAnimator = player.GetComponent<Animator>();
        }
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        UpdateTimerText();
        if (monstersAlive == 0 && currentWave <= maxWaves)
        {
            currentWave++;
            if (currentWave > maxWaves)
            {
                Debug.Log("All waves completed!");
                return;
            }
            ConcurrentWave();
            UpdateWaveText();
        }
        CheckPlayerHealth();
    }
    private void CheckPlayerHealth()
    {
        if (playerHealthManager.currentHealth <= 0 && !PlayerDead)
        {
            PlayerDead = true;
            playerAnimator.SetTrigger("Dead");
            StartCoroutine(EndGameAfterDelay(4f));
        }
    }
    private IEnumerator EndGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 0; // Stop the game
    }
    public void InitialWave()
    {
        SpawnMonsters(Random.Range(1, maxMonsters + 1));
        UpdateMonstersAliveText();
    }

    public void ConcurrentWave()
    {
        int waveMaxMonsters = Mathf.Min(maxMonsters * currentWave);
        SpawnMonsters(waveMaxMonsters);
        FindObjectOfType<PickUpManager>().RespawnPickUps(); // Call RespawnPickUps() on the PickUpManager
    }


    private void SpawnMonsters(int numMonsters)
    {
        for (int i = 0; i < numMonsters; i++)
        {
            GameObject monsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
            float x = Random.Range(terrain.transform.position.x, terrain.transform.position.x + terrain.terrainData.size.x);
            float z = Random.Range(terrain.transform.position.z, terrain.transform.position.z + terrain.terrainData.size.z);
            float y = terrain.SampleHeight(new Vector3(x, 0, z)) + terrain.transform.position.y;

            Vector3 spawnPosition = new Vector3(x, y, z);

            GameObject newMonster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            newMonster.GetComponent<Enemy>().gameManager = this;
            monstersAlive++;
            UpdateMonstersAliveText();
        }
    }


    public void DecreaseMonsterCount()
    {
        monstersAlive--;
        UpdateMonstersAliveText();
    }
    private void UpdateWaveText()
    {
        if (waveText != null)
        {
            waveText.text = $"Wave: {currentWave}";
        }
    }
    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60F);
            int seconds = Mathf.FloorToInt(elapsedTime - minutes * 60);
            timerText.text = $"Timer: {minutes:00}:{seconds:00}";
        }
    }
    private void UpdateMonstersAliveText()
    {
        if (monstersAliveText != null)
        {
            monstersAliveText.text = $"Monsters Alive: {monstersAlive}";
        }
    }
}
