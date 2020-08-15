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
using System.Net.Http;
using System.Net.Http.Headers;

namespace PROJ_CHAMADO
{
    [Activity(Label = "abrir")]
    public class abrir : Activity
    {
        EditText  usuario, dep, andar, sal, desc, prod1, prod2;
        Spinner Tipo;
        Button Cont, Cancelar;
        ArrayAdapter A_Tip, A_Prod;
        string clas;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.abrir);           
            usuario = FindViewById<EditText>(Resource.Id.c_usuario);
            dep = FindViewById<EditText>(Resource.Id.c_dep);
            andar = FindViewById<EditText>(Resource.Id.c_andar); 
            sal = FindViewById<EditText>(Resource.Id.c_sala);
            desc = FindViewById<EditText>(Resource.Id.c_des);
            prod1 = FindViewById<EditText>(Resource.Id.c_prod);
            prod2 = FindViewById<EditText>(Resource.Id.c_prod2);
            Cont = FindViewById<Button>(Resource.Id.bt_cont);
            Cancelar = FindViewById<Button>(Resource.Id.c_canc);
            Tipo = FindViewById<Spinner>(Resource.Id.sp_tipo);
            A_Tip = ArrayAdapter.CreateFromResource(this, Resource.Array.tip, Android.Resource.Layout.SimpleListItem1);
            Tipo.Adapter = A_Tip;

            Cont.Click += Cont_Click;
            Cancelar.Click += Cancelar_Click;
            Tipo.ItemSelected += Tipo_ItemSelected;
            

 


        }

        private void Tipo_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            clas = (Tipo.SelectedItemId + 1).ToString();
        }

        private void Cancelar_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(Mnu));
        }

        private async void Cont_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(usuario.Text) || String.IsNullOrEmpty(dep.Text) || String.IsNullOrEmpty(andar.Text) || String.IsNullOrEmpty(sal.Text) || String.IsNullOrEmpty(prod1.Text) || String.IsNullOrEmpty(desc.Text)) 
            {
                Toast.MakeText(this, "Campos * Obrigatório", ToastLength.Short).Show();
            }
           
            else { 
                string url = "http://192.168.15.9:80/S_CHAM/abrir_C.php";
                Dictionary<string, string> Ab_Cham = new Dictionary<string, string>();


                Ab_Cham.Add("b_id_users", usuario.Text);
                Ab_Cham.Add("b_dep", dep.Text);
                Ab_Cham.Add("b_andar", andar.Text);
                Ab_Cham.Add("b_sal", sal.Text);
                Ab_Cham.Add("b_c_data", DateTime.Now.ToString());
                Ab_Cham.Add("b_prod1", prod1.Text);
                Ab_Cham.Add("b_prod2", prod2.Text);
                Ab_Cham.Add("b_descricao", desc.Text);
                Ab_Cham.Add("b_id_clas", clas);

                HttpClient solicita = new HttpClient();
                var json = JsonConvert.SerializeObject(Ab_Cham);
                var ctd = new StringContent(json, Encoding.UTF8, "application/json");
                ctd.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage resultado = await solicita.PostAsync(url, ctd);
                var leitura = await resultado.Content.ReadAsStringAsync();
                string resp = JsonConvert.DeserializeObject<string>(leitura);
                if (resp.ToLower() == "certo")
                {
                    Toast.MakeText(this, "CADASTRADO COM SUCESSO", ToastLength.Short).Show();

                    usuario.Text = null;
                    dep.Text = null;
                    andar.Text = null;
                    sal.Text = null;
                    prod1.Text = null;
                    prod2.Text = null;
                    desc.Text = null;
                    Tipo.SetSelection(0);
                    StartActivity(typeof(Mnu));

                }
                else
                {
                    Toast.MakeText(this, "ERRO NO CADASTRO", ToastLength.Short).Show();
                }
            }
        }
    }
}
