using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;

namespace zabbixinstall
{
    class Check
    {
        public static bool CheckServices(ref bool CheckServices)
        {
            ServiceController[] Services;
            Services = ServiceController.GetServices();
            foreach (ServiceController i in Services)
            {
                if (i.DisplayName == Data.ServiceName)
                {
                    CheckServices = true;
                    break;
                }
                else
                {
                    CheckServices = false;
                }
            }
            return CheckServices;
        }
    }
}
