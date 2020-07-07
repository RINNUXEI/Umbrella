using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Umbrella.GSSDataService;
using UnityEngine;

namespace Umbrella.Ranking
{
    /// <summary>
    /// A singleton class manages sending and getting ranking data.
    /// </summary>
    public class RankingManager : GSSDataSender<RankingManager>
    {
        [SerializeField] private RankingSettings _settings;

        protected override string AppURL => _settings.AppURL;

        /// <summary>
        /// Create a SendScoreRequestData object using the default setting.
        /// </summary>
        /// <param name="score">Score</param>
        /// <returns></returns>
        public SendScoreRequestData CreateDefaultSendScoreRequest(float score)
        {
            return new SendScoreRequestData(score, _settings.DefaultRankingRequestData);
        }

        /// <summary>
        /// Create a RankingRequestData object using the default setting.
        /// </summary>
        /// <returns></returns>
        public RankingRequestData CreateDefaultRankingRequest()
        {
            return new RankingRequestData(_settings.DefaultRankingRequestData);
        }

        /// <summary>
        /// Send multiple scores to Google sheets.
        /// You can use yield to wait until the process completes.
        /// </summary>
        /// <param name="requestDataList">List of request data.</param>
        /// <param name="responseHandler">Method that will be called to handle response.</param>
        /// <returns></returns>
        public CustomYieldInstruction SendScoresAsync(IList<SendScoreRequestData> requestDataList, Action<IList<RankingResponseData>> responseHandler = null)
        {
            var data = CreateSendingData(requestDataList);
            data[Const.PlayerName] = LocalSaveDataHelper.GetUserName();
            data[Const.PlayerScore] = requestDataList.Select(d => d.Score).ToArray();
            return SendRequestAsync(Const.SaveScoreMethod, data, response => responseHandler?.Invoke(ParseResponse(response)));
        }

        /// <summary>
        /// Get ranking data list from Google sheets.
        /// You can use yield to wait until the process completes.
        /// </summary>
        /// <param name="requestDataList">List of request data.</param>
        /// <param name="responseHandler">Method that will be called to handle response.</param>
        /// <returns></returns>
        public CustomYieldInstruction GetRankingListsAsync(IList<RankingRequestData> requestDataList, Action<IList<RankingResponseData>> responseHandler)
        {
            var data = CreateSendingData(requestDataList);
            return SendRequestAsync(Const.GetRankingMethod, data, response => responseHandler?.Invoke(ParseResponse(response)));
        }

        private IDictionary<string, object> CreateSendingData(IEnumerable<RankingRequestData> requestDataList)
        {
            var data = new Dictionary<string, object>();
            data[Const.PlayerId] = LocalSaveDataHelper.GetUserID();
            data[Const.RankingName] = requestDataList.Select(d => d.RankingName).ToArray();
            data[Const.RankingType] = requestDataList.Select(d => d.RankingType.ToString()).ToArray();
            data[Const.RankingNumber] = requestDataList.Select(d => d.RankingNumber).ToArray();
            data[Const.RankingOrderBy] = requestDataList.Select(d => d.OrderBy.ToString()).ToArray();
            return data;
        }

        private IList<RankingResponseData> ParseResponse(object response)
        {
            var responseDataList = new List<RankingResponseData>();
            foreach (IDictionary data in (IList)response)
            {
                var rankingName = data[Const.RankingName].ToString();
                var aroundMeRankingList = ParseRankingDataList(data[Const.AroundMeRanking]);
                var topRankingList = ParseRankingDataList(data[Const.TopRanking]);
                responseDataList.Add(new RankingResponseData(rankingName, aroundMeRankingList, topRankingList));
            }
            return responseDataList;

            IList<RankingData> ParseRankingDataList(object dataList)
            {
                var rankingDataList = new List<RankingData>();
                foreach (IDictionary data in (IList)dataList)
                {
                    var playerId = data[Const.PlayerId].ToString();
                    var playerName = data[Const.PlayerName].ToString();
                    var playerScore = float.Parse(data[Const.PlayerScore].ToString());
                    var playerPosition = int.Parse(data[Const.PlayerPosition].ToString());
                    rankingDataList.Add(new RankingData(playerId, playerName, playerScore, playerPosition));
                }
                return rankingDataList;
            }
        }
    }
}
