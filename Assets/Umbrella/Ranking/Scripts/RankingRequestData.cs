using UnityEngine;

namespace Umbrella.Ranking
{
    /// <summary>
    /// Ranking request data.
    /// </summary>
    [System.Serializable]
    public class RankingRequestData
    {
        [SerializeField] private string _rankingName;
        [SerializeField] private RankingType _rankingType;
        [SerializeField] private int _rankingNumber;
        [SerializeField] private OrderBy _orderBy;

        /// <summary>
        /// The name of this ranking, also defines the name of the sheet used to store ranking data.
        /// </summary>
        public string RankingName
        {
            get => _rankingName;
            set => _rankingName = value;
        }

        /// <summary>
        /// The type of this ranking, top, around me or both.
        /// </summary>
        public RankingType RankingType
        {
            get => _rankingType;
            set => _rankingType = value;
        }

        /// <summary>
        /// The  number of ranking data should be retrived for this ranking.
        /// </summary>
        public int RankingNumber
        {
            get => _rankingNumber;
            set => _rankingNumber = value;
        }

        /// <summary>
        /// The sorting order for this ranking.
        /// </summary>
        public OrderBy OrderBy
        {
            get => _orderBy;
            set => _orderBy = value;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rankingName">Ranking name.</param>
        /// <param name="rankingType">Ranking type.</param>
        /// <param name="rankingNumber">Request ranking number.</param>
        /// <param name="orderBy">Sorting order.</param>
        public RankingRequestData(string rankingName, RankingType rankingType, int rankingNumber, OrderBy orderBy)
        {
            _rankingName = rankingName;
            _rankingType = rankingType;
            _rankingNumber = rankingNumber;
            _orderBy = orderBy;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="requestData">Ranking request data.</param>
        public RankingRequestData(RankingRequestData requestData)
        {
            _rankingName = requestData.RankingName;
            _rankingType = requestData.RankingType;
            _rankingNumber = requestData.RankingNumber;
            _orderBy = requestData.OrderBy;
        }
    }

    /// <summary>
    /// Ranking sorting order.
    /// </summary>
    public enum OrderBy
    {
        // Ascending
        ASC,
        // Descending
        DESC
    }

    /// <summary>
    /// Ranking type.
    /// </summary>
    public enum RankingType
    {
        // Get the top n players.
        Top,
        // Get the players around me.
        AroundMe,
        // Get the top players and the players around me.
        TopAndAroundMe
    }
}
