using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduling
{
    class RoundRobin : SchedulingPolicy
    {
        private Queue<int> m_qReadyQueue = new Queue<int>(); // תור תהליכים מוכנים
        private int m_iQuantum; // גודל ה-quantum

        public RoundRobin(int iQuantum)
        {
            m_iQuantum = iQuantum;

        }

        public override int NextProcess(Dictionary<int, ProcessTableEntry> dProcessTable)
        {
            while (m_qReadyQueue.Count > 0)
            {
                int processId = m_qReadyQueue.Dequeue();
                var process = dProcessTable[processId];
                if (!process.Blocked && !process.Done)
                {
                    process.Quantum = m_iQuantum; // עדכן את ה-quantum של התהליך
                    return processId;
                }
            }
            throw new NotImplementedException();
        }

        public override void AddProcess(int iProcessId)
        {
            
            m_qReadyQueue.Enqueue(iProcessId);
            throw new NotImplementedException();
        }

        public override bool RescheduleAfterInterrupt()
        {
            return true; 
            throw new NotImplementedException();
        }
    }
}
