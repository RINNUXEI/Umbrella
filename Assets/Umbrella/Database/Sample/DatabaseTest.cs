using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Umbrella.Database
{
    public class DatabaseTest : MonoBehaviour
    {
        [SerializeField] private InputField _sendDataInputField;
        [SerializeField] private Button _sendDataButton;
        [SerializeField] private InputField _getDataInputField;
        [SerializeField] private Button _getDataButton;
        [SerializeField] private Text _getDataResults;

        void Start()
        {
            _sendDataButton.onClick.AddListener(SendData);
            _getDataButton.onClick.AddListener(GetData);
        }

        private void SendData()
        {
            var content = _sendDataInputField.text;
            var keyValuePairs = content.Split(',');
            var data = new Dictionary<string, object>();
            foreach (var kvp in keyValuePairs)
            {
                var split = kvp.Split(':');
                var key = split[0];
                var value = split[1];
                data[key] = value;
            }
            DatabaseManager.Instance.SendDataAsync(data);
        }

        private void GetData()
        {
            var content = _getDataInputField.text;
            var keys = content.Split(',');
            DatabaseManager.Instance.GetDataAsync(keys, PrintResults);

            // You can also use cell reference to get data.
            //DatabaseManager.Instance.GetDataAsync("A1:E2", PrintResults);
        }

        private void PrintResults(IList<string> results)
        {
            _getDataResults.text = string.Join(",", results);
        }
    }
}
