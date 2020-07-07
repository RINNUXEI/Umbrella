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
    public class DatabaseManager : GSSDataSender<DatabaseManager>
    {
        [SerializeField] private DatabaseSettings _settings;

        protected override string AppURL => _settings.AppURL;

        /// <summary>
        /// Send data to Google Sheets.
        /// You can use yield to wait until the process completes.
        /// </summary>
        /// <param name="data">Key value pairs of the data.</param>
        /// <param name="sheetName">Sheet name of the Google sheet to communicate with, if not provided the default sheet name will be used.</param>
        /// <returns></returns>
        public CustomYieldInstruction SendDataAsync(IDictionary<string, object> data, string sheetName = null)
        {
            var sendData = new Dictionary<string, object>();
            sendData[Const.UserId] = LocalSaveDataHelper.GetUserID();
            sendData[Const.SheetName] = sheetName ?? _settings.DefaultSheetName;
            sendData[Const.Data] = data;
            return SendRequestAsync(Const.SaveDataMethod, sendData, null);
        }

        /// <summary>
        /// Get  data from Google sheets.
        /// You can use yield to wait until the process completes.
        /// </summary>
        /// <param name="keys">List of data keys.</param>
        /// <param name="responseHandler">Method that will be called to handle response.</param>
        /// <param name="sheetName">Sheet name of the Google sheet to communicate with, if not provided the default sheet name will be used.</param>
        /// <returns></returns>
        public CustomYieldInstruction GetDataAsync(IList<string> keys, Action<IList<string>> responseHandler, string sheetName = null)
        {
            var sendData = new Dictionary<string, object>();
            sendData[Const.UserId] = LocalSaveDataHelper.GetUserID();
            sendData[Const.SheetName] = sheetName ?? _settings.DefaultSheetName;
            sendData[Const.Data] = keys;
            return SendRequestAsync(Const.GetDataMethod, sendData, response => responseHandler?.Invoke(ParseResponse(response)));
        }

        /// <summary>
        /// Get data from Google sheets within the specified cell range.
        /// You can use yield to wait until the process completes.
        /// </summary>
        /// <param name="cellReference">The cell range to retrieve data from.</param>
        /// <param name="responseHandler">Method that will be called to handle response.</param>
        /// <param name="sheetName">Sheet name of the Google sheet to communicate with, if not provided the default sheet name will be used.</param>
        /// <returns></returns>
        public CustomYieldInstruction GetDataAsync(string cellReference, Action<IList<string>> responseHandler, string sheetName = null)
        {
            var sendData = new Dictionary<string, object>();
            sendData[Const.SheetName] = sheetName ?? _settings.DefaultSheetName;
            sendData[Const.CellReference] = cellReference;
            return SendRequestAsync(Const.GetDataMethod, sendData, response => responseHandler?.Invoke(ParseResponse(response)));
        }

        private IList<string> ParseResponse(object response)
        {
            var resultList = new List<string>();
            foreach (var result in (IList)response) resultList.Add(result.ToString());
            return resultList;
        }
    }
}
