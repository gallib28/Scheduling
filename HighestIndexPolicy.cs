using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduling
{
    class HighestIndexPolicy : SchedulingPolicy
    {
        public override int NextProcess(Dictionary<int, ProcessTableEntry> dProcessTable)
        {
            var readyProcesses = dProcessTable.Values
                .Where(entry => !entry.Done && !entry.Blocked)
                .OrderByDescending(entry => entry.Priority);
            return readyProcesses.Any() ? readyProcesses.First().ProcessId : -1;

            throw new NotImplementedException();
        }

        public override void AddProcess(int iProcessId)
        {
            throw new NotImplementedException();
        }

        public override bool RescheduleAfterInterrupt()
        {
            throw new NotImplementedException();
        }
    }
}
