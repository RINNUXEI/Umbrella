using System.Collections;
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
        /// Top ranking List.
        /// </summary>
        public IList<RankingData> TopRankingList { get; set; }

        /// <summary>
        /// Around me ranking list.
        /// </summary>
        public IList<RankingData> AroundMeRankingList { get; set; }
    }

    /// <summary>
    /// Ranking response data deserializer.
    /// </summary>
    public static class RankingResponseDataDeserializer
    {
        /// <summary>
        /// Deserialize MiniJSON object to RankingResponseData.
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns></returns>
        public static RankingResponseData Deserialize(IDictionary data)
        {
            var rankingName = data[Const.RankingName].ToString();
            var topRankingList = DeserializeRankingDataList((IList)data[Const.TopRankingSettings]);
            var aroundMeRankingList = DeserializeRankingDataList((IList)data[Const.AroundMeRankingSettings]);
            return new RankingResponseData
            {
                RankingName = rankingName,
                TopRankingList = topRankingList,
                AroundMeRankingList = aroundMeRankingList
            };

            IList<RankingData> DeserializeRankingDataList(IList list)
            {
                var rankingDataList = new List<RankingData>();
                foreach (IDictionary d in list)
                {
                    var id = d[Const.PlayerId].ToString();
                    var name = d[Const.PlayerName].ToString();
                    var score = float.Parse(d[Const.PlayerScore].ToString());
                    var position = int.Parse(d[Const.PlayerPosition].ToString());
                    rankingDataList.Add(new RankingData(id, name, score, position));
                }
                return rankingDataList;
            }
        }
    }
}
