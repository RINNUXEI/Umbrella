using UnityEngine;

namespace Umbrella.Ranking
{
    /// <summary>
    /// Ranking settings ScriptableObject.
    /// </summary>
    [CreateAssetMenu]
    public class RankingSettings : ScriptableObject
    {
        [SerializeField] private string _appURL;
        [SerializeField] private RankingRequestData _defaultRankingRequestData;

        /// <summary>
        /// The web app URL of your Google App Script.
        /// </summary>
        public string AppURL => _appURL;

        /// <summary>
        /// The default ranking request data.
        /// </summary>
        public RankingRequestData DefaultRankingRequestData => _defaultRankingRequestData;
    }
}
