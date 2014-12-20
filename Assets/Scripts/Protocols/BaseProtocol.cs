using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ucss
{

    public enum transactionsMode
    {
        flexible,
        successively,
        successivelyWithCheck
    }

    public enum transactionStatus
    {
        added,
        sending,
        timeOut,
        needResend,
        completed
    }

    public struct Transaction
    {
        public string id;
        public object data;
        public object request;
        public transactionStatus status;
        public int timeStart;
        public int timeOut;
    }

    public class BaseProtocol : MonoBehaviour
    {
        public int TimeOut { get; set; }
        public float TimeOutCheck { get; set; }

        private float _lastTimeOutCheck;
        private float _lastGarbageCheck;

        protected EventHandlerServiceInited _onInitCallback;
        protected EventHandlerServiceError _onErrorCallback;

        protected Dictionary<string, Transaction> _transactions;
        protected Queue<string> _transactionsQueue;

        protected transactionsMode _transactionsMode = transactionsMode.flexible;

        public void InitBase(EventHandlerServiceInited initiedCallback, EventHandlerServiceError errorCallback)
        {
            this._onInitCallback = initiedCallback;
            this._onErrorCallback = errorCallback;

            this._transactions = new Dictionary<string, Transaction>();
            this._transactionsQueue = new Queue<string>();
        } // InitBase

        public void SetTransactionsMode(transactionsMode mode)
        {
            this._transactionsMode = mode;
        }


        public void AddTransaction(string id, object data, object request = null, int timeOut = 0)
        {
            if (this._transactions.ContainsKey(id))
            {
                throw new UnityEngine.UnityException("Transaction [" + id + "] already exists in _transactions");
            }

            Transaction transaction = new Transaction();
            transaction.id = id;
            transaction.data = data;
            transaction.request = request;
            transaction.status = transactionStatus.added;
            transaction.timeStart = Ucss.Common.GetSeconds();
            transaction.timeOut = timeOut;

            this._transactions.Add(id, transaction);
            this._transactionsQueue.Enqueue(id);
        }

        public Transaction GetTransaction(string id)
        {
            if (!this._transactions.ContainsKey(id))
            {
                throw new UnityEngine.UnityException("Transaction [" + id + "] is not found in _transactions");
            }
            return this._transactions[id];
        }

        public Transaction GetNextTransaction()
        {
            string transactionId = this._transactionsQueue.Dequeue();
            if (string.IsNullOrEmpty(transactionId))
            {
                return new Transaction();
            }
            if (!this._transactions.ContainsKey(transactionId))
            {
                return new Transaction();
            }
            return this._transactions[transactionId];
        }

        public bool IsNextTransactionExists()
        {
            if (this._transactionsQueue.Count > 0)
            {
                return true;
            }
            return false;
        }

        public void SetTransactionStatus(string id, transactionStatus status)
        {
            if (!this._transactions.ContainsKey(id))
            {
                throw new UnityEngine.UnityException("Transaction [" + id + "] is not found in _transactions");
            }
            Transaction transaction = this._transactions[id];
            transaction.status = status;
            this._transactions[id] = transaction;
        }

        public void RemoveTransaction(string id)
        {
            if (this._transactions.ContainsKey(id))
            {
                this._transactions.Remove(id);
            }
        }

        public bool TransactionValid(string id)
        {
            if (!this._transactions.ContainsKey(id))
            {
                return false;
            }
            if (this._transactions[id].status != transactionStatus.added && this._transactions[id].status != transactionStatus.sending)
            {
                return false;
            }
            return true;
        }

        protected void OnTimeOut(Transaction transaction)
        {
            HTTPRequest request = (HTTPRequest)transaction.request;
            if (!string.IsNullOrEmpty(request.transactionId))
            {
                if (request.onTimeOut != null)
                {
                    request.onTimeOut(transaction.id);
                    return;
                }
                if (request.onError != null)
                {
                    request.onError("timeout", transaction.id);
                    return;
                }
                if (this._onErrorCallback != null)
                {
                    this._onErrorCallback("timeout", transaction.id);
                    return;
                }
                throw new UnityEngine.UnityException("Transaction [" + transaction.id + "] time out, but no callbacks for it");
            }
            else
            {
                throw new UnityEngine.UnityException("Transaction [" + transaction.id + "] time out, but [request] is null");
            }
        }

        // *** UPDATE ***
        void Update()
        {
            // check for timeOut
            if (this.TimeOutCheck > 0.0f && (Time.time - this._lastTimeOutCheck > this.TimeOutCheck))
            {
                this._lastTimeOutCheck = Time.time;
                if (this._transactions.Count > 0)
                {
                    List<string> timeOuts = new List<string>();
                    foreach (KeyValuePair<string, Transaction> entry in this._transactions)
                    {
                        if (entry.Value.status == transactionStatus.sending && entry.Value.timeStart + entry.Value.timeOut < Ucss.Common.GetSeconds())
                        {
                            timeOuts.Add(entry.Key);
                        }
                    } // foreach
                    if (timeOuts.Count > 0)
                    {
                        for (int i = 0; i < timeOuts.Count; i++)
                        {
                            Transaction transaction = this._transactions[timeOuts[i]];
                            transaction.status = transactionStatus.timeOut;
                            this._transactions[timeOuts[i]] = transaction;

                            this.OnTimeOut(transaction);
                        }
                    }
                }
            }

            if (this._transactions.Count > 0 && (Time.time - this._lastGarbageCheck > 300.0f))
            {
                // remove old transactions
                List<string> toRemove = new List<string>();
                foreach (KeyValuePair<string, Transaction> entry in this._transactions)
                {
                    if (entry.Value.status != transactionStatus.added && entry.Value.status != transactionStatus.needResend)
                    {
                        toRemove.Add(entry.Key);
                    }
                } // foreach
                if (toRemove.Count > 0)
                {
                    for (int i = 0; i < toRemove.Count; i++)
                    {
                        this.RemoveTransaction(toRemove[i]);
                        Debug.LogWarning("[BaseProtocol] old transaction [" + toRemove[i] + "] is removed");
                    }
                }
                this._lastGarbageCheck = Time.time;
            }
        } // Update

    } // BaseProtocol
}