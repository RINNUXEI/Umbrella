using System.Collections.Generic;

namespace Umbrella.Ranking
{
    /// <summary>
    /// Ranking response data.
    /// </summary>
    public class RankingResponseData
    {
        /// <summary>
        /// Ranking name.
        /// </summary>
        public string RankingName { get; set; }

        /// <summary>
        /// Around me ranking list.
        /// </summary>
        public IList<RankingData> AroundMeRankingList { get; set; }

        /// <summary>
        /// Top ranking List.
        /// </summary>
        public IList<RankingData> TopRankingList { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rankingName">Ranking name.</param>
        /// <param name="aroundMeRankingList">Around me ranking list.</param>
        /// <param name="topRankingList">Top ranking List.</param>
        public RankingResponseData(string rankingName, IList<RankingData> aroundMeRankingList, IList<RankingData> topRankingList)
        {
            RankingName = rankingName;
            AroundMeRankingList = aroundMeRankingList;
            TopRankingList = topRankingList;
        }
    }
}
