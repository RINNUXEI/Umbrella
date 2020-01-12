using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Umbrella.Database
{
    public class DatabaseTest : MonoBehaviour
    {
        [SerializeField] private Button _sendDataButton;
        [SerializeField] private Button _getDataButton;

        void Start()
        {
            _sendDataButton.onClick.AddListener(SendData);
            _getDataButton.onClick.AddListener(GetData);
        }

        private void SendData()
        {
            //GSSDataServer.Instance.SendDataAsync("equipment", "umbrella");

            var data = new Dictionary<string, object>();
            data["playerName"] = "totorock";
            data["message"] = "こんにちわ";
            DatabaseManager.Instance.SendDataAsync(data);
        }

        private void GetData()
        {
            // var key = "equipment";
            // GSSDataServer.Instance.GetDataAsync(key, PrintResult);

            // var keys = new List<string> { "playerName", "message" };
            // GSSDataServer.Instance.GetDataAsync(keys, PrintResults);

            var cellRef = "A1:E2";
            DatabaseManager.Instance.GetDataAsync(cellRef, PrintResults);
        }

        private void PrintResult(string result)
        {
            Debug.Log(result);
        }

        private void PrintResults(IList<string> results)
        {
            foreach (var result in results)
            {
                UnityEngine.Debug.Log(result);
            }
        }
    }
}
