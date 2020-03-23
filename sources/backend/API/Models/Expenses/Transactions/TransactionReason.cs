namespace AcademyCloud.API.Models.Expenses.Transactions
{
    public class TransactionReason
    {
        public TransactionType Type { get; set; }
        public string Info { get; set; }
        public static TransactionReason FromGrpc(AcademyCloud.Expenses.Protos.Transactions.TransactionReason reason)
        {
            return new TransactionReason
            {
                Type = (TransactionType)reason.Type,
                Info = reason.Info
            };

        }
    }
}