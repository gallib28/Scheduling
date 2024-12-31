using System;
using System.Collections.Generic;
using System.Linq; // נדרש עבור פעולות חישוב מתקדמות
using Scheduling; // שם המרחב של הפרויקט

namespace Scheduling
{
    class Test
    { // סעיף 1: Context Switch
        static void TestContextSwitch()
        {
            Console.WriteLine("Testing Context Switch...");
            Disk disk = new Disk() ;
            CPU cpu = new CPU(disk); 

            OperatingSystem os = new OperatingSystem(cpu, disk, new FirstComeFirstServedPolicy());
            os.CreateProcess("a.code");
            os.CreateProcess("b.code");

            ProcessTableEntry outgoingProcess = os.PublicContextSwitch(1);

            if (outgoingProcess != null)
            {
                Console.WriteLine("Context Switch successful.");
            }
            else
            {
                Console.WriteLine("Context Switch failed.");
            }
        }

        // סעיף 2: Interrupt
        static void TestInterrupt()
        {
            Disk disk = new Disk() ;
            CPU cpu = new CPU(disk);
            Console.WriteLine("Testing Interrupt...");
            OperatingSystem os = new OperatingSystem(cpu, disk, new FirstComeFirstServedPolicy());
            os.CreateProcess("a.code");
            ReadTokenRequest request = new ReadTokenRequest(1, "testToken");
            os.Interrupt(request);
            Console.WriteLine("Interrupt test completed.");
        }

        // סעיף 3: Idle Process
        static void TestIdleProcess()
        {
            Disk disk = new Disk() ;
            CPU cpu = new CPU(disk);
            Console.WriteLine("Testing Idle Process...");
            OperatingSystem os = new OperatingSystem(cpu, disk, new FirstComeFirstServedPolicy());
            os.ActivateScheduler();
            Console.WriteLine("Idle Process test completed.");
        }

        // סעיף 4: Yield Command
        static void TestYieldCommand()
        {
            Disk disk = new Disk() ;
            CPU cpu = new CPU(disk);            
            Console.WriteLine("Testing Yield Command...");
            OperatingSystem os = new OperatingSystem(cpu, disk, new FirstComeFirstServedPolicy());
            os.CreateProcess("a.code");
            os.CreateProcess("b.code");
            cpu.Execute();
            Console.WriteLine("Yield Command test completed.");
        }

        // סעיף 5: Non-Preemptive Scheduling
        static void TestNonPreemptiveScheduling()
        {
            Disk disk = new Disk() ;
            CPU cpu = new CPU(disk);   
            Console.WriteLine("Testing Non-Preemptive Scheduling...");
            OperatingSystem os = new OperatingSystem(cpu, disk, new FirstComeFirstServedPolicy());
            os.CreateProcess("a.code");
            os.CreateProcess("b.code");
            cpu.Execute();
            Console.WriteLine("Non-Preemptive Scheduling test completed.");
        }

        // סעיף 6: FirstComeFirstServed with Quantum
        static void TestFirstComeFirstServed()
        {
            Disk disk = new Disk() ;
            CPU cpu = new CPU(disk);
            Console.WriteLine("Testing FirstComeFirstServed with Quantum...");
            OperatingSystem os = new OperatingSystem(cpu, disk, new RoundRobin(5));
            os.CreateProcess("a.code");
            os.CreateProcess("b.code");
            cpu.Execute();
            Console.WriteLine("FirstComeFirstServed with Quantum test completed.");
        }

        // סעיף 7: RoundRobin with Priorities
        static void TestRoundRobinWithPriorities()
        {
            Disk disk = new Disk() ;
            CPU cpu = new CPU(disk);
            Console.WriteLine("Testing RoundRobin with Priorities...");
            OperatingSystem os = new OperatingSystem(cpu, disk, new PrioritizedScheduling(5));
            os.CreateProcess("a.code", 1);
            os.CreateProcess("b.code", 2);
            cpu.Execute();
            Console.WriteLine("RoundRobin with Priorities test completed.");
        }

        // סעיף 8: Performance Metrics
        static void TestPerformanceMetrics()
        {
            Disk disk = new Disk() ;
            CPU cpu = new CPU(disk);
            Console.WriteLine("Testing Performance Metrics...");
            OperatingSystem os = new OperatingSystem(cpu, disk, new FirstComeFirstServedPolicy());
            os.CreateProcess("a.code");
            os.CreateProcess("b.code");
            os.ProcessFinished(1);
            os.ProcessFinished(2);

            double avgTurnaround = os.AverageTurnaround();
            int maxStarvation = os.MaximalStarvation();

            Console.WriteLine($"Average Turnaround Time: {avgTurnaround}");
            Console.WriteLine($"Maximal Starvation Time: {maxStarvation}");
            Console.WriteLine("Performance Metrics test completed.");
        }
    }
}



