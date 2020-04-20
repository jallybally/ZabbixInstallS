using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zabbixinstall
{
    struct Data
    {
        //
        public static int AddressWidth = 0;
        //
        public static string ServiceName = "Zabbix Agent";
        public static string SharedPath64 = @"\\BATMAN\Bases2\test\Zabbix agent\64";
        public static string SharedPath32 = @"\\BATMAN\Bases2\test\Zabbix agent\32";
        public static string ServicePath = System.String.Empty;
        public static string Zabbix_agentd_conf = "zabbix_agentd.conf";
        public static string Zabbix_agentd = "zabbix_agentd.exe";
        public static string Zabbix_get = "zabbix_get.exe";
        public static string Zabbix_sender = "zabbix_sender.exe";
        public static string Zabbixscr = "zabbixscr.exe";
        public static string LogPath = System.String.Empty;
        public static string FersInstall = "\\Zabbix";
        //
        public static bool CheckServices = false;
    }
}
