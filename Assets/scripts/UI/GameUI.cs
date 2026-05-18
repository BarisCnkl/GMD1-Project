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
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject == null)
        {
            Debug.LogWarning("[GameUI] No object with Player tag found.");
            return;
        }

        player = playerObject;

        playerHealth = player.GetComponent<PlayerHealth>();
        playerProtection = player.GetComponent<PlayerProtection>();

        if (playerHealth == null)
        {
            Debug.LogWarning("[GameUI] PlayerHealth not found on " + player.name);
        }

        if (playerProtection == null)
        {
            Debug.LogWarning("[GameUI] PlayerProtection not found on " + player.name);
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