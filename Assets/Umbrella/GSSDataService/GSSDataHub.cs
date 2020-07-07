using System;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using UnityEngine.Networking;

namespace Umbrella.GSSDataService
{
    /// <summary>
    /// The core class to send data to Google App Script.
    /// </summary>
    public class GSSDataHub
    {
        private string _appURL;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="appURL">The web app URL of your Google App Script.</param>
        public GSSDataHub(string appURL) => _appURL = appURL;

        /// <summary>
        /// Send data to Google App Script.
        /// You can use yield to wait until the process completes.
        /// </summary>
        /// <param name="context">A MonoBehaviour object to start the data sending coroutine.</param>
        /// <param name="methodName">Method name specified in Google App Script, the server side will call this method to handle the sending data.</param>
        /// <param name="sheetName">Sheet name of the Google sheet to communicate with.</param>
        /// <param name="data">Data to be sent.</param>
        /// <param name="responseHandler">Method will be called to handle response.</param>
        /// <returns></returns>
        public CustomYieldInstruction SendDataAsync(MonoBehaviour context, string methodName, IDictionary<string, object> data, Action<object> responseHandler = null)
        {
            var strData = Json.Serialize(data);

            var formData = new List<IMultipartFormSection>();
            formData.Add(new MultipartFormDataSection(Const.Method, methodName));
            formData.Add(new MultipartFormDataSection(Const.Payload, strData));

            bool complete = false;
            context.StartCoroutine(CT_SendData(formData, status => complete = status, responseHandler));

            return new WaitUntil(() => complete);
        }

        private IEnumerator CT_SendData(List<IMultipartFormSection> formData, Action<bool> updateStatus, Action<object> responseHandler = null)
        {
            updateStatus(false);

            var www = UnityWebRequest.Post(_appURL, formData);

            Debug.Log("<color=blue>[GSSDataService]</color> Start sending data to Google Sheets.");

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError($"<color=blue>[GSSDataService]</color> Sending data to Google Sheets failed. Error: {www.error}");
            }
            else
            {
                Debug.Log("<color=blue>[GSSDataService]</color> Sending data to Google Sheets completed");
                try
                {
                    var response = Json.Deserialize(www.downloadHandler.text);
                    string message = response as string;
                    if (message != null && message.Contains("Error")) Debug.LogError($"<color=blue>[GSSDataService]</color> Getting data from Google Sheets failed. {message}");
                    else responseHandler?.Invoke(response);
                }
                catch (InvalidCastException e)
                {
                    Debug.LogError($"<color=blue>[GSSDataService]</color> Parsing result from Google Sheets failed. Error: {e.Message}");
                }
            }

            updateStatus(true);
        }
    }
}
