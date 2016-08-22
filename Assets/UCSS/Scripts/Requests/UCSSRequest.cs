namespace Ucss
{
    public class UCSSRequest
    {
        public string url;
        public string transactionId;

        public EventHandlerServiceError onError;
        public EventHandlerServiceTimeOut onTimeOut;
        public EventHandlerServiceTimeOutRetry onTimeOutRetry;
        public int timeOut;
        public int maxTries;
    }
}