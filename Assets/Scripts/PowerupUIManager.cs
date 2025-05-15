using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PowerupUIManager : MonoBehaviour
{
    public Transform iconHolder; // Assign the PowerupUI layout group
    public GameObject iconPrefab; // Assign the PowerupIcon prefab

    public Sprite doubleDamageSprite;
    public Sprite speedBoostSprite;
    public Sprite infiniteAmmoSprite;
    public Sprite timeFreezeSprite;

    private Dictionary<PowerupType, GameObject> activeIcons = new Dictionary<PowerupType, GameObject>();

    public void ShowPowerup(PowerupType type, float duration)
    {
        if (activeIcons.ContainsKey(type))
        {
            // Reset timer if already active
            Destroy(activeIcons[type]);
            activeIcons.Remove(type);
        }

        GameObject icon = Instantiate(iconPrefab, iconHolder);

        //  Set timer text (on a child)
        TextMeshProUGUI timerText = icon.GetComponentInChildren<TextMeshProUGUI>();
        if (timerText != null)
            timerText.text = duration.ToString("F0");

        //  Set the icon sprite (on the root)
        Image image = icon.GetComponent<Image>();
        if (image != null)
            image.sprite = GetSpriteFor(type);

        activeIcons[type] = icon;
        StartCoroutine(UpdateTimer(type, icon, duration));
    }

    IEnumerator UpdateTimer(PowerupType type, GameObject icon, float duration)
    {
        float time = duration;
        TextMeshProUGUI timerText = icon.GetComponentInChildren<TextMeshProUGUI>();

        while (time > 0)
        {
            time -= Time.deltaTime;
            if (timerText != null)
                timerText.text = Mathf.Ceil(time).ToString();
            yield return null;
        }

        if (activeIcons.ContainsKey(type))
        {
            Destroy(activeIcons[type]);
            activeIcons.Remove(type);
        }
    }

    Sprite GetSpriteFor(PowerupType type)
    {
        switch (type)
        {
            case PowerupType.DoubleDamage: return doubleDamageSprite;
            case PowerupType.SpeedBoost: return speedBoostSprite;
            case PowerupType.InfiniteAmmo: return infiniteAmmoSprite;
            case PowerupType.TimeFreeze: return timeFreezeSprite;
            default: return null;
        }
    }
}
