using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Umbrella.Ranking
{
    public class SamplePresenter : MonoBehaviour
    {
        [SerializeField] private SampleView _view;
        [SerializeField] private Transform _topRankingDataContainer;
        [SerializeField] private Transform _aroundMeRankingDataContainer;
        [SerializeField] private SampleRankingDataView _rankingDataViewBase;

        private void Start()
        {
            var rankingRequest = RankingManager.Instance.CreateDefaultRankingRequest();
            _view.OnClickSendScoreButton.AddListener(() => StartCoroutine(CT_SendScore(rankingRequest)));
            _view.OnClickGetRankingButton.AddListener(() => StartCoroutine(CT_GetRankingLists(rankingRequest)));
            _view.RankingName = rankingRequest.RankingName;
            _view.PlayerName = RankingManager.Instance.UserName;
        }

        private IEnumerator CT_SendScore(RankingRequestData rankingRequest)
        {
            RankingManager.Instance.UserName = _view.PlayerName;

            var sendScoreRequest = new SendScoreRequestData(_view.PlayerScore, rankingRequest);

            UnityEngine.Debug.Log($"Sending {rankingRequest.RankingName} ranking data ...");

            // Wait until the ranking data has been retrieved.
            yield return RankingManager.Instance.SendScoresAsync(new List<SendScoreRequestData> { sendScoreRequest }, UpdateRankingLists);

            UnityEngine.Debug.Log($"{rankingRequest.RankingName} ranking data updated.");
        }

        private IEnumerator CT_GetRankingLists(RankingRequestData rankingRequest)
        {
            rankingRequest.RankingName = _view.RankingName;

            UnityEngine.Debug.Log($"Retrieving {rankingRequest.RankingName} ranking data ...");

            // Wait until the ranking data has been retrieved.
            yield return RankingManager.Instance.GetRankingListsAsync(new List<RankingRequestData> { rankingRequest }, UpdateRankingLists);

            UnityEngine.Debug.Log($"{rankingRequest.RankingName} ranking data updated.");
        }

        private void UpdateRankingLists(IList<RankingResponseData> responseDataList)
        {
            // Since we only deal with one single ranking.
            var responseData = responseDataList[0];

            UpdateRankingList(_topRankingDataContainer, responseData.TopRankingList);
            UpdateRankingList(_aroundMeRankingDataContainer, responseData.AroundMeRankingList);

            void UpdateRankingList(Transform rankingListContainer, IList<RankingData> rankingDataList)
            {
                foreach (Transform child in rankingListContainer) Destroy(child.gameObject);
                foreach (var rankingData in rankingDataList)
                {
                    var view = Instantiate(_rankingDataViewBase, rankingListContainer);
                    view.SetView(rankingData.PlayerPosition.ToString(), rankingData.PlayerName, rankingData.PlayerScore.ToString());
                }
            }
        }
    }
}
