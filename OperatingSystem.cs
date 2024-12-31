using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Scheduling
{
    class OperatingSystem
    {   
        public Dictionary<int, ProcessTableEntry> ProcessTable {
            get { return m_dProcessTable; }
        } 
        
        private ProcessTableEntry m_idleProcess; // משתנה לתהליך Idle

        public Disk Disk { get; private set; }
        public CPU CPU { get; private set; }
        private Dictionary<int, ProcessTableEntry> m_dProcessTable;
        private List<ReadTokenRequest> m_lReadRequests;
        private int m_cProcesses;
        private SchedulingPolicy m_spPolicy;
        private static int IDLE_PROCESS_ID = 0;

        public IdleCode idlepro{get;}
        public OperatingSystem(CPU cpu, Disk disk, SchedulingPolicy sp)
        {
            CPU = cpu;
            Disk = disk;
            m_dProcessTable = new Dictionary<int, ProcessTableEntry>();
            m_lReadRequests = new List<ReadTokenRequest>();
            cpu.OperatingSystem = this;
            disk.OperatingSystem = this;
            m_spPolicy = sp;

            
            // creating the file for idle
            m_idleProcess = new ProcessTableEntry(IDLE_PROCESS_ID, "Idle", new IdleCode());
            m_idleProcess.Done = false;
            m_idleProcess.Blocked = false;


            m_dProcessTable.Add(IDLE_PROCESS_ID, m_idleProcess);


        }


        private void ScheduleNextProcess()
        {
            int nextProcessId = m_spPolicy.NextProcess(m_dProcessTable);

            if (nextProcessId == -1)
            {
                nextProcessId = IDLE_PROCESS_ID;
            }

            // עדכון ה-CPU עם התהליך הבא
            CPU.SetProcess(m_dProcessTable[nextProcessId]);
        }


        private int GenerateUniqueId()
        {
            return m_cProcesses + 1;
        }

        public void CreateProcess(string sCodeFileName,Code code)
                {
                    int processId = GenerateUniqueId();
                    m_dProcessTable[m_cProcesses] = new ProcessTableEntry(processId, sCodeFileName, code);
                    m_dProcessTable[m_cProcesses].StartTime = CPU.TickCount;
                    m_spPolicy.AddProcess(m_cProcesses);
                    m_cProcesses++;
                }
                

        public void CreateProcess(string sCodeFileName)
        {
            Code code = new Code(sCodeFileName);
            m_dProcessTable[m_cProcesses] = new ProcessTableEntry(m_cProcesses, sCodeFileName, code);
            m_dProcessTable[m_cProcesses].StartTime = CPU.TickCount;
            m_spPolicy.AddProcess(m_cProcesses);
            m_cProcesses++;
        }
        public void CreateProcess(string sCodeFileName, int iPriority)
        {
            Code code = new Code(sCodeFileName);
            m_dProcessTable[m_cProcesses] = new ProcessTableEntry(m_cProcesses, sCodeFileName, code);
            m_dProcessTable[m_cProcesses].Priority = iPriority;
            m_dProcessTable[m_cProcesses].StartTime = CPU.TickCount;
            m_spPolicy.AddProcess(m_cProcesses);
            m_cProcesses++;
        }

        public void ProcessTerminated(Exception e)
        {
            if (e != null)
                Console.WriteLine("Process " + CPU.ActiveProcess + " terminated unexpectedly. " + e);
            m_dProcessTable[CPU.ActiveProcess].Done = true;
            m_dProcessTable[CPU.ActiveProcess].Console.Close();
            m_dProcessTable[CPU.ActiveProcess].EndTime = CPU.TickCount;
            ActivateScheduler();
        }

        public void TimeoutReached()
        {
            ActivateScheduler();
        }
        public ProcessTableEntry PublicContextSwitch(int iEnteringProcessId)
        {
            return ContextSwitch(iEnteringProcessId);
        }
        public void ReadToken(string sFileName, int iTokenNumber, int iProcessId, string sParameterName)
        {
            ReadTokenRequest request = new ReadTokenRequest(iProcessId,null);
            request.TokenNumber = iTokenNumber;
            request.TargetVariable = sParameterName;
            request.Token = null;
            request.FileName = sFileName;
            m_dProcessTable[iProcessId].Blocked = true;
            if (Disk.ActiveRequest == null)
                Disk.ActiveRequest = request;
            else
                m_lReadRequests.Add(request);
            CPU.ProgramCounter = CPU.ProgramCounter + 1;
            ActivateScheduler();
        }

        public static int GetProcessPriority(int processId, Dictionary<int, ProcessTableEntry> processTable)
        {
            if (processTable.ContainsKey(processId))
            {
                return processTable[processId].Priority;
            }
            else
            {
                throw new ArgumentException($"Process with ID {processId} does not exist.");
            }
        }
        public void HandleInterrupt()
        {
            Console.WriteLine("Interrupt accepted");

            // קריאה לתהליך הבא
            ScheduleNextProcess();
        }


        public void Interrupt(ReadTokenRequest rFinishedRequest)
        {
            //your code here
            /*
            there is 4 steps:
            1. test if the return token is null and translate the value to double (check if finished reading the file)
            2. writing the value to the specific process addressSpace
            3. Activate the next request in queue on the disk
            4. Activate scheduler if policy requires
            */
            // Step 1
            double tokenValue;
            if (rFinishedRequest.Token == null)
            {
                tokenValue = double.NaN; // End of file
            }
            else
            {
                tokenValue = double.Parse(rFinishedRequest.Token); // Convert token to double
            }
            // Step 2
            m_dProcessTable[rFinishedRequest.ProcessId].AddressSpace[rFinishedRequest.TargetVariable] = tokenValue;
            // Step 3
            if (m_lReadRequests.Count > 0)
            {
                Disk.ActiveRequest = m_lReadRequests[0]; // Set the next request as active
                m_lReadRequests.RemoveAt(0); // Remove it from the queue
            }
            // Step 4
            if (m_spPolicy.RescheduleAfterInterrupt())
            {
                ActivateScheduler();
            }
            //implement an "end read request" interrupt handler.
            //translate the returned token into a value (double). DONE
            //when the token is null, EOF has been reached. DONE
            //write the value to the appropriate address space of the calling process.DONE
            //activate the next request in queue on the disk. DONE
            //activate scheduler if policy requires. DONE
            throw new NotImplementedException();            
        }

        private ProcessTableEntry ContextSwitch(int iEnteringProcessId)
        {
            //your code here
            if (CPU.ActiveProcess != -1) // checking if there is an active process
            {
                // Save the current active process ID
                int iExitingProcessId = CPU.ActiveProcess;
                // getting the tuple of the process from the dictionary
                ProcessTableEntry exitingProcess = m_dProcessTable[iExitingProcessId];
                // Update the exiting process details in the process table , taking from the cpu the info of the running process
                exitingProcess.ProgramCounter = CPU.ProgramCounter;
                exitingProcess.AddressSpace = CPU.ActiveAddressSpace;
                exitingProcess.Console = CPU.ActiveConsole;
                exitingProcess.LastCPUTime = CPU.TickCount;
                //finished updating the table 

                // making the switch of the processes
                CPU.ActiveProcess = iEnteringProcessId;
                ProcessTableEntry enteringProcess = m_dProcessTable[iEnteringProcessId];

                //telling the cpu who the new process he nedd to work on
                CPU.ProgramCounter = enteringProcess.ProgramCounter;
                CPU.ActiveAddressSpace = enteringProcess.AddressSpace;
                CPU.ActiveConsole = enteringProcess.Console;
                    
                return exitingProcess; 
            }
            else // no active process
            {
                //simply making the cpu to run the new process
                CPU.ActiveProcess = iEnteringProcessId ;
                ProcessTableEntry enteringProcess = m_dProcessTable[iEnteringProcessId];
                //updating the new process info to the cpu
                CPU.ProgramCounter = enteringProcess.ProgramCounter;
                CPU.ActiveAddressSpace = enteringProcess.AddressSpace;
                CPU.ActiveConsole = enteringProcess.Console;
                return null;
            }
            
            
            //implement a context switch, switching between the currently active process on the CPU to the process with pid iEnteringProcessId
            //You need to switch the following: ActiveProcess, ActiveAddressSpace, ActiveConsole, ProgramCounter.
            //All values are stored in the process table (m_dProcessTable)
            //Our CPU does not have registers, so we do not store or switch register values.
            //returns the process table information of the outgoing process
            //After this method terminates, the execution continues with the new process
            throw new NotImplementedException();
        }


        // help function for ActivateScheduler
        private bool OnlyIdleRemains()
        {
            // Check if all processes are either done or blocked, except the idle process
            return m_dProcessTable.Values.All(entry => entry.Done || entry.Blocked || entry.ProcessId == 0);
        }

        public void ActivateScheduler()
        {
            int iNextProcessId = m_spPolicy.NextProcess(m_dProcessTable);
            if (iNextProcessId == -1 || OnlyIdleRemains())
            {
                Console.WriteLine("Only idle remains.");
                CPU.Done = true;
            }
            else
            {
                ContextSwitch(iNextProcessId);
                Console.WriteLine($"Switching to process: {iNextProcessId}");
            }
            ScheduleNextProcess();
        }

        public double AverageTurnaround()
        {
            double totalTurnaroundTime = 0;
            int completedProcesses = 0;

            foreach (var process in m_dProcessTable.Values)
            {
                if (process.Done)
                {
                    totalTurnaroundTime += process.EndTime - process.StartTime; // זמן סיום פחות זמן התחלה
                    completedProcesses++;
                }
            }

            if (completedProcesses == 0)
            {
                return 0; // אין תהליכים שהסתיימו
            }

            return totalTurnaroundTime / completedProcesses; // זמן ממוצע
        }
        public int MaximalStarvation()
        {
            int maxStarvation = 0;

            foreach (var process in m_dProcessTable.Values)
            {
                if (!process.Done && !process.Blocked)
                {
                    int starvationTime = CPU.TickCount - process.LastCPUTime; // חישוב הזמן מאז הפעם האחרונה שהתהליך רץ
                    process.MaxStarvation = Math.Max(process.MaxStarvation, starvationTime); // עדכון ה-MaxStarvation של התהליך
                    maxStarvation = Math.Max(maxStarvation, process.MaxStarvation); // עדכון ה-MaxStarvation המקסימלי
                }
            }

            return maxStarvation;
        }
    private void RunExample2()
        {
            // Example setup (pseudo-code as per instructions)
            for (int i = 0; i < 3; i++)
            {
                CreateProcess("ReadFile1.code");
                CreateProcess("ReadFile2.code");
            }

            ActivateScheduler();
            CPU.Execute();

            Console.WriteLine("All processes have completed their execution.");
        }

    public void ProcessFinished(int processId)
    {
        Console.WriteLine($"Process {processId} done.");
        var p = m_dProcessTable[processId];
        p.EndTime = CPU.TickCount; // הגדרת זמן הסיום
        p.Done = true; 
        // קריאה לתהליך הבא
        ScheduleNextProcess();
    }

    
    
    }
}
