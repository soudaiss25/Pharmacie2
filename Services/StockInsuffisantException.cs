namespace Pharmacie2.Services
{
    /// <summary>Levée par StockService.Retirer quand le stock ne suffit pas (message en français).</summary>
    public class StockInsuffisantException : InvalidOperationException
    {
        public StockInsuffisantException(string message) : base(message) { }
    }
}
