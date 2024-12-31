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
        private OperatingSystem? m_os; 
        public PrioritizedScheduling(int iQuantum)
        {
            m_iQuantum = iQuantum;
        }
        public PrioritizedScheduling(int iQuantum, OperatingSystem os)
        {
            m_iQuantum = iQuantum;
            m_os = os;
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
                        // עדכון quantum של התהליך
                        process.Quantum = m_iQuantum;

                        // עדכון RemainingTime של ה-CPU
                        m_os.CPU.RemainingTime = m_iQuantum;

                        // החזרת התהליך המוכן
                        return processId;
                    }
                }

            }
            return -1;

        }
        public PrioritizedScheduling() 
    {
        // אתחול ברירת מחדל
    }


        public override void AddProcess(int iProcessId)
        {
            int priority = m_os.ProcessTable[iProcessId].Priority; // השגת העדיפות של התהליך

            if (!m_qPriorityQueues.ContainsKey(priority))
                m_qPriorityQueues[priority] = new Queue<int>();

            m_qPriorityQueues[priority].Enqueue(iProcessId);
        }


        public override bool RescheduleAfterInterrupt()
        {
            return true;
            throw new NotImplementedException();
        }
    }
}
