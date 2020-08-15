using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PROJ_CHAMADO
{
    [Activity(Theme = "@style/Theme.Splash", NoHistory = true, MainLauncher = true)]
    public class T_Apre : Activity
    {
        TextView Test;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            System.Threading.Thread.Sleep(30);
            StartActivity(typeof(Login));
        }
    }
}