using System;
using UnityEngine;
using UnityEngine.UI;

namespace Umbrella.Ranking
{
    public class SampleView : MonoBehaviour
    {
        [SerializeField] private InputField _rankingNameInput;
        [SerializeField] private InputField _playerNameInput;
        [SerializeField] private InputField _scoreInput;
        [SerializeField] private Button _sendScoreButton;
        [SerializeField] private Button _getRankingButton;

        public Button.ButtonClickedEvent OnClickSendScoreButton => _sendScoreButton.onClick;

        public Button.ButtonClickedEvent OnClickGetRankingButton => _getRankingButton.onClick;

        public string RankingName
        {
            get => _rankingNameInput.text;
            set => _rankingNameInput.text = value;
        }

        public string PlayerName
        {
            get => _playerNameInput.text;
            set => _playerNameInput.text = value;
        }

        public int PlayerScore => Int32.Parse(_scoreInput.text);
    }
}
