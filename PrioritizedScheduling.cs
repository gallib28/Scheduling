using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduling
{
    class PrioritizedScheduling : SchedulingPolicy
    {
        private SortedDictionary<int, Queue<int>> m_qPriorityQueues = new SortedDictionary<int, Queue<int>>(); // תור תהליכים לפי עדיפויות
        private int m_iQuantum; // גודל ה-quantum
        public PrioritizedScheduling(int iQuantum)
        {
            m_iQuantum = iQuantum;
        }

        public override int NextProcess(Dictionary<int, ProcessTableEntry> dProcessTable)
        {
            foreach (var priorityQueue in m_qPriorityQueues.Reverse())
            {
                while (priorityQueue.Value.Count > 0)
                {
                    int processId = priorityQueue.Value.Dequeue();
                    var process = dProcessTable[processId];
                    if (!process.Blocked && !process.Done)
                    {
                        process.Quantum = m_iQuantum; // עדכן את ה-quantum של התהליך
                        return processId;
                    }
                }
            }
            return -1; // אין תהליכים מוכנים
            throw new NotImplementedException();
        }

        public override void AddProcess(int iProcessId)
        {
        int priority = OperatingSystem.GetProcessPriority(iProcessId, m_dProcessTable);
            if (!m_qPriorityQueues.ContainsKey(priority))
                m_qPriorityQueues[priority] = new Queue<int>();

            m_qPriorityQueues[priority].Enqueue(iProcessId);

            throw new NotImplementedException();
        }

        public override bool RescheduleAfterInterrupt()
        {
            return true;
            throw new NotImplementedException();
        }
    }
}
