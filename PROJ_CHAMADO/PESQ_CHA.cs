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
using Newtonsoft.Json;

namespace PROJ_CHAMADO
{
    class PESQ_CHA
    {
        [JsonProperty(PropertyName = "est_json")]
        public string datas_est { get; set; }

        public PESQ_CHA(string x)
        {this.datas_est = x;}
    }
}