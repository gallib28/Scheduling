using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduling
{
    class ProcessTableEntry
    {
        public bool Blocked { get; set; }
        public bool Done { get; set; }
        public string Name { get; private set; }
        public AddressSpace AddressSpace { get; set; }
        public int ProcessId { get; private set; }
        public int ProgramCounter { get; set; }
        public ProcessConsole Console { get; set; }
        public int Quantum { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int LastCPUTime { get; set; }
        public int MaxStarvation { get; set; }
        public int Priority { get; set; }

        public ProcessTableEntry tableEntry { get; set; }
        
        public ProcessTableEntry()
        {
            ProcessId = -1; // Temporary value
            AddressSpace = new AddressSpace(ProcessId);
            Console = null;
            Name = string.Empty;
            LastCPUTime = 0;
            StartTime = 0;
            EndTime = -1;
            MaxStarvation = 0;
            Priority = 0;
        }
        
        public ProcessTableEntry(int iProcessId, string sName, Code code)
        {
            tableEntry = new ProcessTableEntry(); // Initialize tableEntry with a default struct
            tableEntry.ProcessId = iProcessId;
            tableEntry.AddressSpace = new AddressSpace(tableEntry.ProcessId);
            tableEntry.AddressSpace.Code = code;
            tableEntry.Console = new ProcessConsole(iProcessId, sName);
            tableEntry.Name = sName;
            tableEntry.LastCPUTime = 0;
            tableEntry.StartTime = 0;
            tableEntry.EndTime = -1;
            tableEntry.MaxStarvation = 0;
            tableEntry.Priority = 0;

            
        }
        // copy constructure ggg
        public ProcessTableEntry(ProcessTableEntry entry)
        {
            this.ProcessId = entry.ProcessId;
            this.AddressSpace = new AddressSpace(entry.AddressSpace);
            this.AddressSpace.Code = entry.AddressSpace.Code;
            this.Console = entry.Console;
            this.Name = entry.Name;
            this.LastCPUTime =entry.LastCPUTime;
            this.ProgramCounter = entry.ProgramCounter;
            this.StartTime = entry.StartTime;
            this.EndTime = entry.EndTime;
            this.MaxStarvation = entry.MaxStarvation;
            this.Priority = entry.Priority;
            this.Quantum = entry.Quantum;
            this.tableEntry= entry.tableEntry; 
        }

        
    }
}
