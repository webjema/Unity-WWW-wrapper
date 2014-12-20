using UnityEngine;
using System.Collections.Generic;

namespace Ucss
{

    public class UCSS : MonoBehaviour
    {
        private static UCSS _instance;
        private static string _controllersHolder = "Controllers"; // you can edit it
        //============================
        // CSS singleton
        public static UCSS Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject();
                    _instance = go.AddComponent<UCSS>();
                    go.name = "UCSS";
                    if (!string.IsNullOrEmpty(_controllersHolder))
                    {
                        GameObject parent = GameObject.Find(_controllersHolder) as GameObject;
                        if (parent != null)
                        {
                            go.transform.parent = parent.transform;
                        }
                    }
                }
                return _instance;
            }
        }
        //============================


        private static HTTPProtocol _http;
        //============================
        // CSS singleton
        public static HTTPProtocol HTTP
        {
            get
            {
                if (_http == null)
                {
                    _http = UCSS.Instance.gameObject.AddComponent<HTTPProtocol>();
                    _http.Init();
                }
                return _http;
            }
        }
        //============================

        private Dictionary<string, BaseProtocol> _initedProtocols = new Dictionary<string, BaseProtocol>();
        //private Dictionary<string, Component> _addedManagers = new Dictionary<string, Component>();

        void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
        }

        public static void InitService(UCSSprotocols protocol, UCSSservices serviceName, string host, EventHandlerServiceInited initedCallback = null, EventHandlerServiceError errorCallback = null)
        {
            UCSS.InitService(protocol, serviceName.ToString(), host, initedCallback, errorCallback);
        }

        public static void InitService(UCSSprotocols protocol, string serviceName, string host, EventHandlerServiceInited initedCallback = null, EventHandlerServiceError errorCallback = null)
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                throw new System.ArgumentNullException("serviceName");
            }
            if (string.IsNullOrEmpty(host))
            {
                throw new System.ArgumentNullException("host");
            }

            if (UCSS.Instance._initedProtocols.Count > 0 && UCSS.Instance._initedProtocols.ContainsKey(serviceName))
            {
                throw new System.InvalidOperationException("serviceName [" + serviceName + "] already exists in CSS");
            }

            BaseProtocol selectedProtocol = null;
            switch (protocol)
            {
                case UCSSprotocols.amf:
                    break;
                default:
                    throw new System.InvalidOperationException("protocol [" + protocol + "] is NOT found in InitService");
            }
            if (selectedProtocol != null)
            {
                UCSS.Instance._initedProtocols.Add(serviceName, selectedProtocol);
            }
            //Debug.Log("CSS.Instance._initedProtocols = " + UCSS.Instance._initedProtocols.Count);
        } // InitService

        /*
         * 
         * DoRequest 
         *
         */


        // no requests here for WWW warapper


        public static string GenerateTransactionId(string name)
        {
            System.Random RNG = new System.Random();
            return name + Ucss.Common.GetSeconds().ToString() + Ucss.Common.Md5Sum(Ucss.Common.GetSeconds().ToString() + RNG.Next(999999).ToString() + name) + RNG.Next(999999).ToString();
        } // GenerateTransactionId

        public static void RemoveTransaction(UCSSservices serviceName, string id)
        {
            UCSS.RemoveTransaction(serviceName.ToString(), id);
        } // RemoveTransaction

        public static void RemoveTransaction(string serviceName, string id)
        {
            BaseProtocol protocol = UCSS.GetInitedProtocol(serviceName);
            protocol.RemoveTransaction(id);
        } // RemoveTransaction


        public static BaseProtocol GetInitedProtocol(string serviceName)
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                throw new System.ArgumentNullException("serviceName");
            }
            if (UCSS.Instance._initedProtocols == null || !UCSS.Instance._initedProtocols.ContainsKey(serviceName))
            {
                throw new System.InvalidOperationException("serviceName [" + serviceName + "] is NOT found in inited protocols");
            }
            return UCSS.Instance._initedProtocols[serviceName];
        } // GetInitedProtocol

    } // UCSS
}