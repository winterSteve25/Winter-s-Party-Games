using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utils.Data;

namespace Utils.EasterEggs
{
    public class UsernameBasedSprite : MonoBehaviour
    {
        [SerializeField] private string[] names;
        [SerializeField] private Sprite easterEggSprite;
        [SerializeField] private Image image;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] private bool ignoreCase;

        private void OnEnable()
        {
            if (!names.Contains(PlayerPrefs.GetString(GameConstants.PlayerPrefs.Username, string.Empty), ignoreCase ? StringComparer.CurrentCultureIgnoreCase : StringComparer.CurrentCulture)) return;
            if (image != null) image.sprite = easterEggSprite;
            if (spriteRenderer != null) spriteRenderer.sprite = easterEggSprite;
        }
    }
}