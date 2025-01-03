﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;

namespace Scheduling
{
    class Program
    {
        static void Example1(OperatingSystem os)
        {
            for (int i = 0; i < 3; i++)
            {
                os.CreateProcess("a.code");
                os.CreateProcess("b.code");
            }
        }
        static void Example2(OperatingSystem os)
        {
            for (int i = 0; i < 3; i++)
            {
                os.CreateProcess("ReadFile1.code");
                os.CreateProcess("ReadFile2.code");
            }
        }
        static void Example3(OperatingSystem os)
        {
            for (int i = 0; i < 3; i++)
            {
                os.CreateProcess("c.code");
                os.CreateProcess("d.code");
            }
        }
        static void Example4(OperatingSystem os)
        {
            for (int i = 0; i < 3; i++)
            {
                os.CreateProcess("c.code", i);
                os.CreateProcess("d.code", i + 1);
            }
        }








        






        static void Main(string[] args)
        {
            Console.WriteLine("Program started.");
            //set the working directory to the "Code files" dir
            Directory.SetCurrentDirectory("../../../Code files");

            Disk disk = new Disk();
            CPU cpu = new CPU(disk);
            cpu.Debug = true;
            OperatingSystem os = new OperatingSystem(cpu, disk, new  HighestIndexPolicy());
            //Example1(os);
            //Example2(os);
            Example3(os);
            //Example4(os);
            // OperatingSystem os = new OperatingSystem(cpu, disk, new RoundRobin(5, os));
            // OperatingSystem os = new OperatingSystem(cpu, disk, new FirstComeFirstServedPolicy(5, os));
            // OperatingSystem os = new OperatingSystem(cpu, disk, new PrioritizedScheduling(5, os));
            os.ActivateScheduler();
            cpu.Execute();
            Console.WriteLine("CPU Execute started.");
            Thread.Sleep(1000);
            Console.WriteLine("Average turnaround " + os.AverageTurnaround());
            Console.WriteLine("Maximal starvation " + os.MaximalStarvation());
        }
    }
}
