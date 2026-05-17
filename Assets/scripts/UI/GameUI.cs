using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerProtection playerProtection;

    [Header("UI Texts")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI killText;
    [SerializeField] private TextMeshProUGUI protectionText;

    private float timer;
    private int kills;

    private void Update()
    {
        timer += Time.deltaTime;

        UpdateHealthText();
        UpdateTimerText();
        UpdateKillText();
        UpdateProtectionText();
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
        if (protectionText == null || playerProtection == null) return;

        if (playerProtection.IsProtected)
        {
            protectionText.gameObject.SetActive(true);
            protectionText.text = "Protection Active";
        }
        else
        {
            protectionText.gameObject.SetActive(false);
        }
    }

    public void AddKill()
    {
        kills++;
    }
}