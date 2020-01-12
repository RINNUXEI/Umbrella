using UnityEngine;
using Umbrella.GSSDataService;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Umbrella.Ranking
{
    /// <summary>
    /// A singleton class manages sending and getting ranking data.
    /// </summary>
    public class RankingManager : MonoBehaviour
    {
        [SerializeField] private RankingSettings _rankingSettings;
        private GSSDataHub _dataHandler;

        /// <summary>
        /// Singleton class instance.
        /// </summary>
        public static RankingManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Send score to Google sheets.
        /// You can use yield to wait until the process completes.
        /// </summary>
        /// <param name="name">Name of the player. Player name can be changed each time you send a score.</param>
        /// <param name="score">Score of the player.</param>
        /// <param name="handleResponse">Method that will be called to handle response.</param>
        /// <param name="rankingRequestIndex">Index of the ranking request settings array.</param>
        /// <returns></returns>
        public CustomYieldInstruction SendScoreAsync(string name, float score, Action<List<RankingData>> handleResponse, int rankingRequestIndex = 0)
        {
            var setting = GetRankingRequestSetting(rankingRequestIndex);
            if (setting == null) return new WaitUntil(() => true);

            if (_dataHandler == null) _dataHandler = new GSSDataHub(_rankingSettings.AppURL);

            var data = new Dictionary<string, object>();
            data[RankingDataConsts.PlayerId] = Helpers.GetUserID();
            data[RankingDataConsts.PlayerName] = name;
            data[RankingDataConsts.PlayerScore] = score;
            data[RankingDataConsts.OrderBy] = setting.OrderBy.ToString();
            data[RankingDataConsts.RequestRankingNumber] = setting.RankingNumber;

            return _dataHandler.SendDataAsync(this, RankingDataConsts.SaveScoreMethod, setting.RankingName, data, response =>
            {
                if (handleResponse == null) return;

                var rankingDataList = ParseResponse(response);
                handleResponse.Invoke(rankingDataList);
            });
        }

        /// <summary>
        /// Get ranking data list from Google sheets.
        /// </summary>
        /// <param name="handleResponse">Method that will be called to handle response.</param>
        /// <param name="rankingRequestIndex">Index of the ranking request settings array.</param>
        /// <returns></returns>
        public CustomYieldInstruction GetRankingListAsync(Action<List<RankingData>> handleResponse, int rankingRequestIndex = 0)
        {
            var setting = GetRankingRequestSetting(rankingRequestIndex);
            if (setting == null) return new WaitUntil(() => true);

            if (_dataHandler == null) _dataHandler = new GSSDataHub(_rankingSettings.AppURL);

            var data = new Dictionary<string, object>();
            data[RankingDataConsts.OrderBy] = setting.OrderBy.ToString();
            data[RankingDataConsts.RequestRankingNumber] = setting.RankingNumber;

            return _dataHandler.SendDataAsync(this, RankingDataConsts.GetRankingMethod, setting.RankingName, data, response =>
            {
                if (handleResponse == null) return;

                var rankingDataList = ParseResponse(response);
                handleResponse.Invoke(rankingDataList);
            });
        }

        private RankingRequestSetting GetRankingRequestSetting(int index)
        {
            var settings = _rankingSettings.RankingRequestSettings;
            if (settings == null || settings.Length == 0)
            {
                Debug.LogError("<color=blue>[Ranking]</color>Ranking request setting has not been set. Add at least one setting in RankingSettings.asset.");
                return null;
            }

            if (index < 0 || index >= settings.Length)
            {
                Debug.LogError("<color=blue>[Ranking]</color>Ranking request setting index out of range. Check RankingSettings.asset.");
                return null;
            }

            return settings[index];
        }

        private List<RankingData> ParseResponse(object response)
        {
            var results = (IList)response;
            if (results == null || results.Count == 0) return null;

            var rankingDataList = new List<RankingData>();
            foreach (IDictionary result in results)
            {
                var playerId = result[RankingDataConsts.PlayerId].ToString();
                var playerName = result[RankingDataConsts.PlayerName].ToString();
                var playerScore = result[RankingDataConsts.PlayerScore].ToString();
                rankingDataList.Add(new RankingData(playerId, playerName, playerScore));
            }
            return rankingDataList;
        }
    }

    /// <summary>
    /// Ranking data object holds information to be displayed via UI.
    /// </summary>
    public class RankingData
    {
        public string PlayerId { get; private set; }
        public string PlayerName { get; private set; }
        public string PlayerScore { get; private set; }

        /// <summary>
        /// Does this ranking data belong to the player him/herself.
        /// </summary>
        /// <returns></returns>
        public bool IsSelf => PlayerId == Helpers.GetUserID();

        public RankingData(string playerId, string playerName, string playerScore)
        {
            PlayerId = playerId;
            PlayerName = playerName;
            PlayerScore = playerScore;
        }
    }
}
