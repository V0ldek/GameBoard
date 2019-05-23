using System;
using Microsoft.EntityFrameworkCore.Storage;

namespace GameBoard.DataLayer.Transactions
{
    public class Transaction : ITransaction
    {
        private readonly IDbContextTransaction _dbContextTransaction;
        private int _numberOfOpenTransactions;

        public Transaction(IDbContextTransaction dbContextTransaction)
        {
            _dbContextTransaction = dbContextTransaction;
            _numberOfOpenTransactions = 1;
        }

        public Guid TransactionId => _dbContextTransaction.TransactionId;

        public void Commit()
        {
            _numberOfOpenTransactions--;

            if (_numberOfOpenTransactions == 0)
            {
                _dbContextTransaction.Commit();
            }
        }

        public void Dispose() => _dbContextTransaction.Dispose();

        public Transaction BeginTransaction()
        {
            if (_numberOfOpenTransactions > 0)
            {
                _numberOfOpenTransactions++;
            }

            return this;
        }

        public bool TransactionFinished() => _numberOfOpenTransactions == 0;
    }
}