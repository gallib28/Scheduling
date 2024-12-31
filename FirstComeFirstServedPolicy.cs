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
                int processId = m_qReadyQueue.Peek(); // מקבל את התהליך הראשון בתור
                var process = dProcessTable[processId];

                // אם התהליך אינו חסום ולא סיים, החזר את ה-ID שלו
                if (!process.Blocked && !process.Done)
                {
                    return processId;
                }

                // הסר תהליכים חסומים או שסיימו
                m_qReadyQueue.Dequeue();
            }
            return -1;
        }

        public override void AddProcess(int iProcessId)
        {
            m_qReadyQueue.Enqueue(iProcessId);

           // throw new NotImplementedException();
        }

        public override bool RescheduleAfterInterrupt()
        {
            return false;
           // throw new NotImplementedException();
        }
    }
}
