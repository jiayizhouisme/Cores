namespace Core.Queue.IQueue
{
    public interface IQueuePushInfo
    {
        Task PushMessage(IQueueEntity eq);
        Task PushMessageDelay(IQueueEntity eq, int sec);
        Task PushMessageDelay(IQueueEntity eq, DateTime date);
    }
}