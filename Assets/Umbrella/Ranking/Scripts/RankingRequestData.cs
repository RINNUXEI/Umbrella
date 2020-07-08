using System;
using System.Collections.Generic;
using UnityEngine;

namespace Umbrella.Ranking
{
    /// <summary>
    /// Ranking request data.
    /// </summary>
    [Serializable]
    public class RankingRequestData
    {
        [SerializeField] private string _rankingName;
        [SerializeField] private RankingRequestListSettings _topRankingListSettings;
        [SerializeField] private RankingRequestListSettings _aroundMeRankingListSettings;

        /// <summary>
        /// The name of this ranking, also defines the name of the sheet used to store ranking data.
        /// </summary>
        public string RankingName
        {
            get => _rankingName;
            set => _rankingName = value;
        }

        /// <summary>
        /// Top ranking list settings.
        /// </summary>
        public RankingRequestListSettings TopRankingListSettings
        {
            get => _topRankingListSettings;
            set => _topRankingListSettings = value;
        }

        /// <summary>
        /// Around me ranking list settings.
        /// </summary>
        public RankingRequestListSettings AroundMeRankingListSettings
        {
            get => _aroundMeRankingListSettings;
            set => _aroundMeRankingListSettings = value;
        }
    }

    /// <summary>
    /// Ranking request data serializer.
    /// </summary>
    public static class RankingRequestDataSerializer
    {
        /// <summary>
        /// Serialize RankingRequestData to make it compatible with MiniJSON.
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns></returns>
        public static IDictionary<string, object> Serialize(RankingRequestData data)
        {
            return new Dictionary<string, object>
            {
                {Const.RankingName, data.RankingName},
                {Const.TopRankingSettings, new Dictionary<string, object>
                {
                    {Const.RankingOrderBy, data.TopRankingListSettings.OrderBy.ToString()},
                    {Const.RankingTakeNumber, data.TopRankingListSettings.TakeNumber},
                }},
                {Const.AroundMeRankingSettings, new Dictionary<string, object>
                {
                    {Const.RankingOrderBy, data.AroundMeRankingListSettings.OrderBy.ToString()},
                    {Const.RankingTakeNumber, data.AroundMeRankingListSettings.TakeNumber},
                }}
            };
        }
    }
}
