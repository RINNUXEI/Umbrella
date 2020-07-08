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
        /// Get an instance of the default ranking request data.
        /// </summary>
        /// <returns></returns>
        public RankingRequestData GetDefaultRankingRequestDataInstance()
        {
            return new RankingRequestData
            {
                RankingName = _defaultRankingRequestData.RankingName,
                TopRankingListSettings = _defaultRankingRequestData.TopRankingListSettings,
                AroundMeRankingListSettings = _defaultRankingRequestData.AroundMeRankingListSettings
            };
        }
    }
}
