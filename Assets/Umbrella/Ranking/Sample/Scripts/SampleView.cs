using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Umbrella.Ranking
{
    public class SampleView : MonoBehaviour
    {
        [SerializeField] private InputField _nameInput;
        [SerializeField] private InputField _scoreInput;
        [SerializeField] private Dropdown _rankingSettingDropdown;
        [SerializeField] private Button _sendScoreButton;
        [SerializeField] private Button _getRankingButton;
        [SerializeField] private Text _myRanking;
        [SerializeField] private Button _clearButton;

        public Button.ButtonClickedEvent OnClickSendScoreButton => _sendScoreButton.onClick;
        public Button.ButtonClickedEvent OnClickGetRankingButton => _getRankingButton.onClick;
        public Button.ButtonClickedEvent OnClickClearButton => _clearButton.onClick;
        public string PlayerName => _nameInput.text;
        public int PlayerScore => Int32.Parse(_scoreInput.text);
        public string CurrentRankingName { get; private set; }

        public void SetMyRanking(string ranking)
        {
            _myRanking.text = ranking;
            _myRanking.gameObject.SetActive(true);
        }

        public void InitializeRankingSettingOptions(IEnumerable<string> rankingNames)
        {
            _rankingSettingDropdown.AddOptions(rankingNames.ToList());
            _rankingSettingDropdown.onValueChanged.AddListener(index => CurrentRankingName = _rankingSettingDropdown.options[index].text);
        }
    }
}
