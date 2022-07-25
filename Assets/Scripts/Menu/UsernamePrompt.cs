using UnityEngine;
using Utils;
using Utils.Audio;

namespace Menu
{
    public class UsernamePrompt : MonoBehaviour
    {
        [SerializeField] private CanvasGroup inputs;
        [SerializeField] private CanvasGroup infoText;

        private void Start()
        {
            if (PlayerPrefs.HasKey(GameConstants.PlayerPrefs.Username))
            {
                gameObject.SetActive(false);
                return;
            }

            infoText.alpha = 0;
            infoText.gameObject.SetActive(false);
        }

        public void ConfirmButtonClicked()
        {
            this.FadeOutAndDeactivate(inputs, 0.1f, () =>
            {
                this.FadeIn(infoText, 0.1f, null);
            });
        }

        public void AcknowledgedButtonClicked()
        {
            this.FadeOutAndDeactivate(GetComponent<CanvasGroup>(), 0.5f, null);
        }
    }
}