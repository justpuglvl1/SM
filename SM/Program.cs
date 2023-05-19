using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenHardwareMonitor.Hardware;

namespace SM
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PerformanceCounter cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            dynamic firstValue;

            Computer comp = new Computer() { CPUEnabled = true, GPUEnabled = true, RAMEnabled = true, HDDEnabled = true };
            comp.Open();

            string temp = "";

            while (true)
            {
                System.Console.Clear();
                firstValue = cpuCounter.NextValue();

                Console.WriteLine($"Процессор: {firstValue}%");
                foreach (var hardwareItem in comp.Hardware)
                {
                    if (hardwareItem.HardwareType == HardwareType.CPU
                     || hardwareItem.HardwareType == HardwareType.GpuNvidia
                     || hardwareItem.HardwareType == HardwareType.GpuAti
                     || hardwareItem.HardwareType == HardwareType.RAM
                     || hardwareItem.HardwareType == HardwareType.HDD)
                    {
                        hardwareItem.Update();
                        foreach (IHardware subHardware in hardwareItem.SubHardware)
                            subHardware.Update();
                        
                        foreach (var sensor in hardwareItem.Sensors)
                        {
                            temp = String.Format("{0} = {1}\r\n", sensor.Name, sensor.Value.HasValue ? sensor.Value.Value.ToString() : "no value");
                            Console.Write(temp);
                        }
                    }
                }
                Thread.Sleep(5000);
            }
        }
    }
}
