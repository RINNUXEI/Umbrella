using UnityEngine;
using UnityEngine.UI;

namespace Umbrella.Ranking
{
    public class SampleRankingDataView : MonoBehaviour
    {
        [SerializeField] private Text _order;
        [SerializeField] private Text _playerName;
        [SerializeField] private Text _score;

        public void SetView(string order, string playerName, string score)
        {
            _order.text = order;
            _playerName.text = playerName;
            _score.text = score;
        }
    }
}
