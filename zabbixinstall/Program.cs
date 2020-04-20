using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;
using System.Management;
using System.IO;
using System.Diagnostics;

namespace zabbixinstall
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Check.CheckServices(ref Data.CheckServices) == true)
            {
                Update.UpdateService();
            }
            else if (Check.CheckServices(ref Data.CheckServices) == false)
            { 
                
            }
        }
    }
    class Update : Program
    {
        public static void UpdateService()
        {
            ServiceController Services = new ServiceController(Data.ServiceName);
            if ((Services.Status.Equals(ServiceControllerStatus.Stopped)) || (Services.Status.Equals(ServiceControllerStatus.StopPending)))
            {
                ManagementObjectSearcher i = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
                foreach (ManagementObject y in i.Get())
                {
                    Data.AddressWidth = Convert.ToInt16(y["AddressWidth"]);
                    break;
                }
                i.Dispose();
                if (Data.AddressWidth == 64)
                {
                    GetNewFile64();
                    try
                    {
                        Services.Start();
                    }
                    catch (InvalidOperationException Ex)
                    {
                        Log.ErrorLog(Ex.ToString());
                    }
                }
                else if (Data.AddressWidth == 32)
                {
                    GetNewFile32();
                    try
                    {
                        Services.Start();
                    }
                    catch (InvalidOperationException Ex)
                    {
                        Log.ErrorLog(Ex.ToString());
                    }
                }
            }
            else
            {
                ManagementObjectSearcher i = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Service");
                foreach (ManagementObject y in i.Get())
                {
                    try
                    {
                        if (Convert.ToString(y["DisplayName"]) == Data.ServiceName)
                        {
                            Data.ServicePath = Convert.ToString(y["PathName"]);
                            break;
                        }
                    }
                    catch
                    {
                        Data.ServicePath = "Не определено";
                        break;
                    }
                }
                i.Dispose();
                if (String.IsNullOrEmpty(Data.ServicePath))
                {
                    Data.ServicePath = "Не определено";
                }
                Services.Stop();
                UpdateService();
            }
        }
        static void GetNewFile64()
        {
            DellOldFile();
            File.Copy($"{ Data.SharedPath64}\\{Data.Zabbixscr}", $"{Data.ServicePath}\\{Data.Zabbixscr}", true);
            File.Copy($"{ Data.SharedPath64}\\{Data.Zabbix_agentd}", $"{Data.ServicePath}\\{Data.Zabbix_agentd}", true);
            File.Copy($"{ Data.SharedPath64}\\{Data.Zabbix_agentd_conf}", $"{Data.ServicePath}\\{Data.Zabbix_agentd_conf}", true);
            File.Copy($"{ Data.SharedPath64}\\{Data.Zabbix_get}", $"{Data.ServicePath}\\{Data.Zabbix_get}", true);
            File.Copy($"{ Data.SharedPath64}\\{Data.Zabbix_sender}", $"{Data.ServicePath}\\{Data.Zabbix_sender}", true);
        }
        static void GetNewFile32()
        {
            DellOldFile();
            File.Copy($"{ Data.SharedPath32}\\{Data.Zabbixscr}", $"{Data.ServicePath}\\{Data.Zabbixscr}", true);
            File.Copy($"{ Data.SharedPath32}\\{Data.Zabbix_agentd}", $"{Data.ServicePath}\\{Data.Zabbix_agentd}", true);
            File.Copy($"{ Data.SharedPath32}\\{Data.Zabbix_agentd_conf}", $"{Data.ServicePath}\\{Data.Zabbix_agentd_conf}", true);
            File.Copy($"{ Data.SharedPath32}\\{Data.Zabbix_get}", $"{Data.ServicePath}\\{Data.Zabbix_get}", true);
            File.Copy($"{ Data.SharedPath32}\\{Data.Zabbix_sender}", $"{Data.ServicePath}\\{Data.Zabbix_sender}", true);
        }
        static void DellOldFile()
        {
            string Zabbixscr = $"{Data.ServicePath} \\ {Data.Zabbixscr}";
            string Zabbix_agentd = $"{Data.ServicePath} \\ {Data.Zabbix_agentd}";
            string Zabbix_agentd_conf = $"{Data.ServicePath} \\ {Data.Zabbix_agentd_conf}";
            string Zabbix_get = $"{Data.ServicePath} \\ {Data.Zabbix_get}";
            string Zabbix_sender = $"{Data.ServicePath} \\ {Data.Zabbix_sender}";
            if (File.Exists(Zabbixscr))
            {
                File.Delete(Zabbixscr);
            }
            if (File.Exists(Zabbix_agentd))
            {
                File.Delete(Zabbix_agentd);
            }
            if (File.Exists(Zabbix_agentd_conf))
            {
                File.Delete(Zabbix_agentd_conf);
            }
            if (File.Exists(Zabbix_get))
            {
                File.Delete(Zabbix_get);
            }
            if (File.Exists(Zabbix_sender))
            {
                File.Delete(Zabbix_sender);
            }
        }
    }
    class Install : Program
    {
        public static void InstallZabbix()
        { 
            string RootDir = Environment.GetFolderPath(Environment.SpecialFolder.System) + Data.FersInstall;
            if (!Directory.Exists(RootDir))
            {
                try
                {
                    Directory.CreateDirectory(RootDir);
                    ManagementObjectSearcher i = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
                    foreach (ManagementObject y in i.Get())
                    {
                        Data.AddressWidth = Convert.ToInt16(y["AddressWidth"]);
                        break;
                    }
                    i.Dispose();
                    if (Data.AddressWidth == 64)
                    {
                        try
                        {
                            File.Copy($"{ Data.SharedPath64}\\{Data.Zabbixscr}", $"{RootDir}\\{Data.Zabbixscr}", true);
                            File.Copy($"{ Data.SharedPath64}\\{Data.Zabbix_agentd}", $"{RootDir}\\{Data.Zabbix_agentd}", true);
                            File.Copy($"{ Data.SharedPath64}\\{Data.Zabbix_agentd_conf}", $"{RootDir}\\{Data.Zabbix_agentd_conf}", true);
                            File.Copy($"{ Data.SharedPath64}\\{Data.Zabbix_get}", $"{RootDir}\\{Data.Zabbix_get}", true);
                            File.Copy($"{ Data.SharedPath64}\\{Data.Zabbix_sender}", $"{RootDir}\\{Data.Zabbix_sender}", true);
                            Process Install = new Process();
                            Install.StartInfo.FileName = $"{RootDir}\\{Data.Zabbix_agentd}";
                            Install.StartInfo.Arguments = $"--config \"{RootDir}\\{Data.Zabbix_agentd_conf}\" \"--install\"";
                            Install.Start();
                            Install.WaitForExit();
                            Install.Close();
                            Process Start = new Process();
                            Start.StartInfo.FileName = $"{RootDir}\\{Data.Zabbix_agentd}";
                            Start.StartInfo.Arguments = $"--config \"{RootDir}\\{Data.Zabbix_agentd_conf}\" \"--start\"";
                            Start.Start();
                            Environment.Exit(0);

                        }
                        catch (InvalidOperationException Ex)
                        {
                            Log.ErrorLog(Ex.ToString());
                        }
                    }
                    else if (Data.AddressWidth == 32)
                    {
                        try
                        {
                            File.Copy($"{ Data.SharedPath32}\\{Data.Zabbixscr}", $"{RootDir}\\{Data.Zabbixscr}", true);
                            File.Copy($"{ Data.SharedPath32}\\{Data.Zabbix_agentd}", $"{RootDir}\\{Data.Zabbix_agentd}", true);
                            File.Copy($"{ Data.SharedPath32}\\{Data.Zabbix_agentd_conf}", $"{RootDir}\\{Data.Zabbix_agentd_conf}", true);
                            File.Copy($"{ Data.SharedPath32}\\{Data.Zabbix_get}", $"{RootDir}\\{Data.Zabbix_get}", true);
                            File.Copy($"{ Data.SharedPath32}\\{Data.Zabbix_sender}", $"{RootDir}\\{Data.Zabbix_sender}", true);
                            Process Install = new Process();
                            Install.StartInfo.FileName = $"{RootDir}\\{Data.Zabbix_agentd}";
                            Install.StartInfo.Arguments = $"--config \"{RootDir}\\{Data.Zabbix_agentd_conf}\" \"--install\"";
                            Install.Start();
                            Install.WaitForExit();
                            Install.Close();
                            Process Start = new Process();
                            Start.StartInfo.FileName = $"{RootDir}\\{Data.Zabbix_agentd}";
                            Start.StartInfo.Arguments = $"--config \"{RootDir}\\{Data.Zabbix_agentd_conf}\" \"--start\"";
                            Start.Start();
                            Environment.Exit(0);

                        }
                        catch (InvalidOperationException Ex)
                        {
                            Log.ErrorLog(Ex.ToString());
                        }
                    }

                }
                catch (Exception Ex)
                {
                    Log.ErrorLog(Ex.ToString());
                }
            }
        }
    }
    class Log : Program
    {
        public static void ErrorLog(string Ex)
        {
            Data.LogPath = $"{Data.SharedPath64}\\{Environment.MachineName}.txt";
            if (!File.Exists(Data.LogPath))
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(Data.LogPath, false, System.Text.Encoding.Default))
                    {
                        sw.WriteLine($"{DateTime.Now} {Ex}");
                    }
                }
                catch
                {
                    Environment.Exit(0);
                }
            }
            else
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(Data.LogPath, true, System.Text.Encoding.Default))
                    {
                        sw.WriteLine($"{DateTime.Now} {Ex}");
                    }
                }
                catch
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}
