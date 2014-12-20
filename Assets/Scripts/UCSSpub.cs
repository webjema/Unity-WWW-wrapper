namespace Ucss
{
    public enum UCSSprotocols
    {
        ws,
        amf,
        socketio,
        sockjs,
        rest
    }

    public delegate void EventHandlerServiceInited();
    public delegate void EventHandlerServiceError(string error, string transactionId);
    public delegate void EventHandlerServiceTimeOut(string transactionId);
    public delegate void EventHandlerResponse(object data, string transactionId);
}