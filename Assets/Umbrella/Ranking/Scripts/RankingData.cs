using System.Collections;
using Umbrella.GSSDataService;

namespace Umbrella.Ranking
{
    /// <summary>
    /// Ranking data object holds information to be displayed via UI.
    /// </summary>
    public class RankingData
    {
        /// <summary>
        /// Player Id.
        /// </summary>
        public string PlayerId { get; set; }

        /// <summary>
        /// Player name.
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// Player score.
        /// </summary>
        public float PlayerScore { get; set; }

        /// <summary>
        /// Player ranking position.
        /// </summary>
        /// <value></value>
        public int PlayerPosition { get; set; }

        /// <summary>
        /// Is this ranking data belong to the local player?
        /// </summary>
        public bool IsMyRanking => PlayerId == LocalSaveDataHelper.GetUserID();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="playerId">Player Id.</param>
        /// <param name="playerName">Player name.</param>
        /// <param name="playerScore">Player score.</param>
        /// <param name="playerPosition">Player order.</param>
        public RankingData(string playerId, string playerName, float playerScore, int playerPosition)
        {
            PlayerId = playerId;
            PlayerName = playerName;
            PlayerScore = playerScore;
            PlayerPosition = playerPosition;
        }
    }
}
