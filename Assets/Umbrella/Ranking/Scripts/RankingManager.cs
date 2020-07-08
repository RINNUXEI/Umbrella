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
            var defaultRankingRequest = _settings.GetDefaultRankingRequestDataInstance();
            var requestData = new SendScoreRequestData
            {
                Score = score,
                RankingName = defaultRankingRequest.RankingName,
                TopRankingListSettings = defaultRankingRequest.TopRankingListSettings,
                AroundMeRankingListSettings = defaultRankingRequest.AroundMeRankingListSettings
            };
            return requestData;
        }

        /// <summary>
        /// Create a RankingRequestData object using the default setting.
        /// </summary>
        /// <returns></returns>
        public RankingRequestData CreateDefaultRankingRequest() => _settings.GetDefaultRankingRequestDataInstance();

        /// <summary>
        /// Send multiple scores to Google sheets.
        /// You can use yield to wait until the process completes.
        /// </summary>
        /// <param name="requestDataList">List of request data.</param>
        /// <param name="responseHandler">Method that will be called to handle response.</param>
        /// <returns></returns>
        public CustomYieldInstruction SendScoresAsync(IList<SendScoreRequestData> requestDataList, Action<IList<RankingResponseData>> responseHandler = null)
        {
            var data = new Dictionary<string, object>();
            data[Const.PlayerId] = LocalSaveDataHelper.GetUserID();
            data[Const.PlayerName] = LocalSaveDataHelper.GetUserName();
            data[Const.RankingRequest] = requestDataList.Select(r => SendScoreRequestDataSerializer.Serialize(r)).ToArray();
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
            var data = new Dictionary<string, object>();
            data[Const.PlayerId] = LocalSaveDataHelper.GetUserID();
            data[Const.RankingRequest] = requestDataList.Select(r => RankingRequestDataSerializer.Serialize(r)).ToArray();
            return SendRequestAsync(Const.GetRankingMethod, data, response => responseHandler?.Invoke(ParseResponse(response)));
        }

        private IList<RankingResponseData> ParseResponse(object response)
        {
            var responseDataList = new List<RankingResponseData>();
            foreach (IDictionary data in (IList)response)
            {
                responseDataList.Add(RankingResponseDataDeserializer.Deserialize(data));
            }
            return responseDataList;
        }
    }
}
