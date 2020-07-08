using System;
using UnityEngine;

namespace Umbrella.Ranking
{
    /// <summary>
    /// Ranking request list settings.
    /// </summary>
    [Serializable]
    public struct RankingRequestListSettings
    {
        [SerializeField] private int _takeNumber;
        [SerializeField] private OrderBy _orderBy;

        /// <summary>
        /// How many ranking data should we take.
        /// </summary>
        public int TakeNumber
        {
            get => _takeNumber;
            set => _takeNumber = value;
        }

        /// <summary>
        /// Ranking order by.
        /// </summary>
        public OrderBy OrderBy
        {
            get => _orderBy;
            set => _orderBy = value;
        }
    }

    /// <summary>
    /// Ranking order by.
    /// </summary>
    public enum OrderBy
    {
        // Ascending
        ASC,
        // Descending
        DESC
    }
}
