using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Umbrella.Ranking
{
    public class SamplePresenter : MonoBehaviour
    {
        [SerializeField] private SampleView _view;
        [SerializeField] private Transform _rankingDataContainer;
        [SerializeField] private RankingDataView _rankingDataViewBase;
        [SerializeField] private RankingSettings _settings;

        private void Start()
        {
            _view.InitializeRankingSettingOptions(_settings.RankingRequestSettings.Select(s => s.RankingName));

            // コールバックでリスト表示更新するやりかた
            _view.OnClickSendScoreButton.AddListener(() => RankingManager.Instance.SendScoreAsync(_view.PlayerName, _view.PlayerScore, UpdateRankingListView, _view.CurrentRankingSettingIndex));

            // 送信の完了を待ってからリスト表示更新するやりかた
            _view.OnClickGetRankingButton.AddListener(() => StartCoroutine(CT_GetRankingList()));
        }

        private IEnumerator CT_GetRankingList()
        {
            yield return RankingManager.Instance.GetRankingListAsync(UpdateRankingListView, rankingRequestIndex: _view.CurrentRankingSettingIndex);
        }

        private void UpdateRankingListView(List<RankingData> rankingDataList)
        {
            if (rankingDataList == null || rankingDataList.Count == 0) return;

            foreach (Transform child in _rankingDataContainer) Destroy(child.gameObject);
            int myRanking = -1;
            for (int i = 0; i < rankingDataList.Count; i++)
            {
                var rankingData = rankingDataList[i];
                if (rankingData.IsSelf) myRanking = i + 1;
                var view = Instantiate(_rankingDataViewBase, _rankingDataContainer);
                view.SetView((i + 1).ToString(), rankingData.PlayerName, rankingData.PlayerScore.ToString());
                view.gameObject.SetActive(true);
            }

            _view.SetMyRanking(myRanking == -1 ? "圏外" : myRanking.ToString());
        }
    }
}
