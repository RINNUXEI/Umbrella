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

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="score">Score to be sent.</param>
        /// <param name="rankingRequestData">Ranking request data.</param>
        public SendScoreRequestData(float score, RankingRequestData rankingRequestData) : base(rankingRequestData)
        {
            Score = score;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="score">Score</param>
        /// <param name="rankingName">Ranking name</param>
        /// <param name="rankingType">Ranking type</param>
        /// <param name="rankingNumber">Ranking number</param>
        /// <param name="orderBy">Order by</param>
        public SendScoreRequestData(float score, string rankingName, RankingType rankingType, int rankingNumber, OrderBy orderBy)
            : base(rankingName, rankingType, rankingNumber, orderBy)
        {
            Score = score;
        }
    }
}
