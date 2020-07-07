using System;
using System.Collections.Generic;
using UnityEngine;

namespace Umbrella.GSSDataService
{
    /// <summary>
    /// GSS data sender, derive this class to implement your specific data sender.
    /// </summary>
    public class GSSDataSender<T> : MonoBehaviour where T : MonoBehaviour
    {
        private GSSDataHub _dataHandler;

        /// <summary>
        /// The GSS web URL.
        /// </summary>
        protected virtual string AppURL { get; }

        /// <summary>
        /// Singleton class instance.
        /// </summary>
        public static T Instance { get; private set; }

        /// <summary>
        /// User unique identity.
        /// </summary>
        public string UserId => LocalSaveDataHelper.GetUserID();

        /// <summary>
        /// User name saved in local.
        /// </summary>
        public string UserName
        {
            get => LocalSaveDataHelper.GetUserName();
            set => LocalSaveDataHelper.SetUserName(value);
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        protected virtual CustomYieldInstruction SendRequestAsync(string methodName, IDictionary<string, object> data, Action<object> responseHandler)
        {
            if (_dataHandler == null) _dataHandler = new GSSDataHub(AppURL);
            return _dataHandler.SendDataAsync(this, methodName, data, response => responseHandler?.Invoke(response));
        }
    }
}
