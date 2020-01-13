using System;
using System.Collections;
using System.Collections.Generic;
using Umbrella.GSSDataService;
using UnityEngine;

namespace Umbrella.Database
{
    /// <summary>
    /// A singleton class manages sending and getting data.
    /// </summary>
    public class DatabaseManager : MonoBehaviour
    {
        [SerializeField] private string _appURL;
        [SerializeField] private string _defaultSheet = "Sheet1";

        private GSSDataHub _dataHub;

        /// <summary>
        /// Singleton class instance.
        /// </summary>
        public static DatabaseManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Send a single data to Google Sheets.
        /// You can use yield to wait until the process completes.
        /// </summary>
        /// <param name="key">Key (name) of the data.</param>
        /// <param name="value">Value (content) of the data.</param>
        /// <param name="handleResponse">Method that will be called to handle response.</param>
        /// <param name="sheetName">Sheet name of the Google sheet to communicate with.</param>
        /// <returns></returns>
        public CustomYieldInstruction SendDataAsync(string key, object value, Action<string> handleResponse = null, string sheetName = "")
        {
            return SendDataAsync(new Dictionary<string, object> { { key, value } }, handleResponse, sheetName);
        }

        /// <summary>
        /// Send mutiple data to Google Sheets.
        /// You can use yield to wait until the process completes.
        /// </summary>
        /// <param name="keyValuePairs">Key value pairs of the data.</param>
        /// <param name="handleResponse">Method that will be called to handle response.</param>
        /// <param name="sheetName">Sheet name of the Google sheet to communicate with.</param>
        /// <returns></returns>
        public CustomYieldInstruction SendDataAsync(IDictionary<string, object> keyValuePairs, Action<string> handleResponse = null, string sheetName = "")
        {
            if (_dataHub == null) _dataHub = new GSSDataHub(_appURL);

            var sendData = new Dictionary<string, object>();
            sendData[DatabaseConsts.UserId] = Helpers.GetUserID();
            sendData[DatabaseConsts.KeyValuePairs] = keyValuePairs;

            if (string.IsNullOrEmpty(sheetName)) sheetName = _defaultSheet;
            return _dataHub.SendDataAsync(this, DatabaseConsts.SaveDataMethod, sheetName, sendData, response =>
            {
                if (handleResponse == null) return;

                var result = response.ToString();
                handleResponse.Invoke(result);
            });
        }

        /// <summary>
        /// Get a single data from Google sheets.
        /// You can use yield to wait until the process completes.
        /// </summary>
        /// <param name="key">Key (name) of the data.</param>
        /// <param name="handleResponse">Method that will be called to handle response.</param>
        /// <param name="sheetName">Sheet name of the Google sheet to communicate with.</param>
        /// <returns></returns>
        public CustomYieldInstruction GetDataAsync(string key, Action<string> handleResponse, string sheetName = "")
        {
            if (_dataHub == null) _dataHub = new GSSDataHub(_appURL);

            var sendData = new Dictionary<string, object>();
            sendData[DatabaseConsts.UserId] = Helpers.GetUserID();
            sendData[DatabaseConsts.Keys] = new string[] { key };

            if (string.IsNullOrEmpty(sheetName)) sheetName = _defaultSheet;
            return _dataHub.SendDataAsync(this, DatabaseConsts.GetDataMethod, sheetName, sendData, response =>
            {
                if (handleResponse == null) return;

                var results = (IList)response;
                if (results == null || results.Count == 0) return;

                var result = results[0].ToString();
                handleResponse.Invoke(result);
            });
        }

        /// <summary>
        /// Get mutiple data from Google sheets.
        /// You can use yield to wait until the process completes.
        /// </summary>
        /// <param name="keys">A list of data keys.</param>
        /// <param name="handleResponse">Method that will be called to handle response.</param>
        /// <param name="sheetName">Sheet name of the Google sheet to communicate with.</param>
        /// <returns></returns>
        public CustomYieldInstruction GetDataAsync(IList<string> keys, Action<IList<string>> handleResponse, string sheetName = "")
        {
            if (_dataHub == null) _dataHub = new GSSDataHub(_appURL);

            var sendData = new Dictionary<string, object>();
            sendData[DatabaseConsts.UserId] = Helpers.GetUserID();
            sendData[DatabaseConsts.Keys] = keys;

            if (string.IsNullOrEmpty(sheetName)) sheetName = _defaultSheet;
            return _dataHub.SendDataAsync(this, DatabaseConsts.GetDataMethod, sheetName, sendData, response =>
            {
                if (handleResponse == null) return;

                var results = (IList)response;
                if (results == null || results.Count == 0) return;

                var values = new List<string>();
                foreach (var result in results) values.Add(result.ToString());
                handleResponse.Invoke(values);
            });
        }

        /// <summary>
        /// Get data from Google sheets within the specified cell range.
        /// You can use yield to wait until the process completes.
        /// </summary>
        /// <param name="cellReference">The cell range to retrieve data from.</param>
        /// <param name="handleResponse">Method that will be called to handle response.</param>
        /// <param name="sheetName">Sheet name of the Google sheet to communicate with.</param>
        /// <returns></returns>
        public CustomYieldInstruction GetDataAsync(string cellReference, Action<IList<string>> handleResponse, string sheetName = "")
        {
            if (_dataHub == null) _dataHub = new GSSDataHub(_appURL);

            var sendData = new Dictionary<string, object>();
            sendData[DatabaseConsts.CellReference] = cellReference;

            if (string.IsNullOrEmpty(sheetName)) sheetName = _defaultSheet;
            return _dataHub.SendDataAsync(this, DatabaseConsts.GetDataMethod, sheetName, sendData, response =>
            {
                if (handleResponse == null) return;

                var results = (IList)response;
                if (results == null || results.Count == 0) return;

                var values = new List<string>();
                foreach (var result in results) values.Add(result.ToString());
                handleResponse.Invoke(values);
            });
        }
    }
}
