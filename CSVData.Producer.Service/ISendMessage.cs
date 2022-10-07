namespace CSVData.Producer.Service
{
    using System.Threading.Tasks;

    public interface ISendMessage
    {
        Task<bool> SendMessageRequest(string key, string message);
    }
}
