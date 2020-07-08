using System.Collections.Generic;

namespace Umbrella.Ranking
{
    /// <summary>
    /// Send score request data.
    /// </summary>
    public class SendScoreRequestData : RankingRequestData
    {
        /// <summary>
        /// Score to be sent.
        /// </summary>
        public float Score { get; set; }
    }

    /// <summary>
    /// Send score request data serializer.
    /// </summary>
    public static class SendScoreRequestDataSerializer
    {
        /// <summary>
        /// Serialize SendScoreRequestData to make it compatible with MiniJSON.
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns></returns>
        public static IDictionary<string, object> Serialize(SendScoreRequestData data)
        {
            var serialized = RankingRequestDataSerializer.Serialize(data);
            serialized[Const.PlayerScore] = data.Score;
            return serialized;
        }
    }
}
