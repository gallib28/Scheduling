using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduling
{
    class FirstComeFirstServedPolicy : SchedulingPolicy
    {
        private Queue<int> m_qReadyQueue = new Queue<int>();


        public override int NextProcess(Dictionary<int, ProcessTableEntry> dProcessTable)
        {
            while (m_qReadyQueue.Count > 0)
            {
                int processId = m_qReadyQueue.Peek();
                var process = dProcessTable[processId];
                if (!process.Blocked && !process.Done)
                    return processId; // תהליך מוכן
                m_qReadyQueue.Dequeue(); // הסר תהליכים חסומים או סיימו
            }
        return -1; // אין תהליכים מוכנים

            throw new NotImplementedException();
        }

        public override void AddProcess(int iProcessId)
        {
            m_qReadyQueue.Enqueue(iProcessId);

            throw new NotImplementedException();
        }

        public override bool RescheduleAfterInterrupt()
        {
            return false;
            throw new NotImplementedException();
        }
    }
}
