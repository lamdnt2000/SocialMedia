namespace WebAPI.Config
{ 
    public class HangfireQueueSetting
    {
        public string QueueName { get; set; }

        public int WorkerCount { get; set; }
    }
}
