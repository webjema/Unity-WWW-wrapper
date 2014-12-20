using UnityEngine;
using System.Collections;

namespace Ucss
{
    public enum UCSSservices
    {
        APIServer,
        InitServer,
        DataServer,
        MainServer
    }

    public static class UCSSconfig
    {

        public static int requestDefaultTimeOut = 10; // seconds
        public static float requestDefaultTimeOutCheck = 0.5f; // seconds
    }

}