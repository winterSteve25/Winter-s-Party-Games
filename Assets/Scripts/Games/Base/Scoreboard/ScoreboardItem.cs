using System;
using Base;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Utils;
using Utils.Audio;

namespace Games.Base.Scoreboard
{
    public class ScoreboardItem : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;
        public PlayerLobbyItem player;
        private RectTransform _textTransform;
        
        private int _targetAmount;
        private int _amount;
        private Action _onComplete;

        private void Start()
        {
            _textTransform = scoreText.GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (_targetAmount == -1) return;
            
            scoreText.text = _amount.ToString();
            _textTransform.DOPunchScale(new Vector3(0.005f, 0.005f, 0.005f), 0.1f);
            _textTransform.DOShakeRotation(0.1f, new Vector3(0, 0, 2f));

            if (_amount == _targetAmount)
            {
                _targetAmount = -1;
                _onComplete?.Invoke();
                return;
            }
            
            _amount++;
        }

        public void SetScore(int amount, Action onComplete = null)
        {
            _targetAmount = amount;
            _amount = int.TryParse(scoreText.text, out var result) ? result : 0;
            _onComplete = onComplete;
        }

        public void SetAmount(int amount)
        {
            _amount = amount;
            scoreText.text = _amount.ToString();
        }
    }
}