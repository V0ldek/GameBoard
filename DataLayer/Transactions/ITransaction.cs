using System;

namespace GameBoard.DataLayer.Transactions
{
    public interface ITransaction : IDisposable
    {
        Guid TransactionId { get; }

        void Commit();

        Transaction BeginTransaction();

        bool TransactionFinished();
    }
}