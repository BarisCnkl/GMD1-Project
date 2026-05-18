using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player;

    [Header("UI Texts")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI killText;
    [SerializeField] private TextMeshProUGUI protectionText;

    private PlayerHealth playerHealth;
    private PlayerProtection playerProtection;

    private float timer;
    private int kills;

    private void Start()
    {
        FindPlayerComponents();

        if (protectionText != null)
        {
            protectionText.gameObject.SetActive(true);
            protectionText.text = "";
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        FindPlayerComponents();

        UpdateHealthText();
        UpdateTimerText();
        UpdateKillText();
        UpdateProtectionText();
    }

    private void FindPlayerComponents()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject;
            }
        }

        if (player != null)
        {
            if (playerHealth == null)
            {
                playerHealth = player.GetComponent<PlayerHealth>();
            }

            if (playerProtection == null)
            {
                playerProtection = player.GetComponent<PlayerProtection>();
            }
        }

        // Extra fallback, in case PlayerProtection is on another player object
        if (playerProtection == null)
        {
            playerProtection = FindFirstObjectByType<PlayerProtection>();
        }
    }

    private void UpdateHealthText()
    {
        if (playerHealth == null || healthText == null) return;

        healthText.text = $"Health: {Mathf.CeilToInt(playerHealth.currentHealth)} / {Mathf.CeilToInt(playerHealth.maxHealth)}";
    }

    private void UpdateTimerText()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void UpdateKillText()
    {
        if (killText == null) return;

        killText.text = $"Kills: {kills}";
    }

    private void UpdateProtectionText()
    {
        if (protectionText == null) return;

        protectionText.gameObject.SetActive(true);

        if (playerProtection != null && playerProtection.IsProtected)
        {
            protectionText.text = "Protection Active";
        }
        else
        {
            protectionText.text = "";
        }
    }

    public void AddKill()
    {
        kills++;
    }
}