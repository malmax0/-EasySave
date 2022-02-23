using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfApp.ViewModel;

namespace WpfApp
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
            public App()
            {

                //Register Syncfusion license
                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTc4Nzg2QDMxMzkyZTM0MmUzMGpzZTlwcWduSEthcEFsM1Z3amZKSXlyY3Y5bVZ2SVlNRTBsVkcwWWFzK1E9");
            }
    }
}
