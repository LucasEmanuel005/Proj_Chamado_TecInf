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
using System.Threading.Tasks;
using System.Collections;

namespace PROJ_CHAMADO
{
    [Activity(Label = "exibir")]
    public class exibir : Activity
    {

        EditText usuario, dep, andar, sal, desc, prod1, prod2, dat;
        Spinner sp_Pesq, sp_Tipo;
        Button bt_Editar, bt_Cancelar, bt_Deletar;
        ArrayAdapter A_Pesq, A_Tip;
        string clas_Editar, C_number, id_user = "1";
        List<string> arrayDat;

        

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.exibir);
            usuario = FindViewById<EditText>(Resource.Id.c_usuario);
            dep = FindViewById<EditText>(Resource.Id.c_dep);
            andar = FindViewById<EditText>(Resource.Id.c_andar);
            sal = FindViewById<EditText>(Resource.Id.c_sala);
            desc = FindViewById<EditText>(Resource.Id.c_des);
            prod1 = FindViewById<EditText>(Resource.Id.c_prod);
            prod2 = FindViewById<EditText>(Resource.Id.c_prod2);
            bt_Editar = FindViewById<Button>(Resource.Id.bt_Editar);
            bt_Cancelar = FindViewById<Button>(Resource.Id.bt_canc);
            sp_Pesq = FindViewById<Spinner>(Resource.Id.sp_pesqs);
            dat = FindViewById<EditText>(Resource.Id.c_data);
            sp_Tipo = FindViewById<Spinner>(Resource.Id.sp_tip);
            bt_Deletar = FindViewById<Button> (Resource.Id.bt_del);

            A_Tip = ArrayAdapter.CreateFromResource(this, Resource.Array.tip, Android.Resource.Layout.SimpleListItem1);
            sp_Tipo.Adapter = A_Tip;


            bt_Editar.Click += Bt_Editar_Click;
            sp_Tipo.ItemSelected += Sp_Tipo_ItemSelected;
            bt_Deletar.Click += Bt_Deletar_Click;

            bt_Cancelar.Click += Bt_Cancelar_Click;




            string uri = "http://192.168.15.9:80/S_CHAM/Pesq_P_DAT.php";
            HttpClient solicita = new HttpClient();
            Dictionary<string, string> pesq = new Dictionary<string, string>();

            

            var json = JsonConvert.SerializeObject(pesq);
            Console.WriteLine(json);
            var conteudo = new StringContent(json, Encoding.UTF8, "application/json");
            conteudo.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage resp = await solicita.PostAsync(uri, conteudo);         
            var cont = await resp.Content.ReadAsStringAsync();
          

            
            List<PESQ_CHA> nome = JsonConvert.DeserializeObject<List<PESQ_CHA>>(cont);
            List<string> dados = new List<string>();
            foreach (var i in nome)
            {
                dados.Add(i.datas_est.ToString());
            }
            A_Pesq = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, dados);
            sp_Pesq.Adapter = A_Pesq;
            sp_Pesq.ItemSelected += Sp_Pesq_ItemSelected;

           


        }

        private void Bt_Cancelar_Click(object sender, EventArgs e)
        {

            StartActivity(typeof(Mnu));
        }

        private async void Bt_Deletar_Click(object sender, EventArgs e)
        {
            string uri = "http://192.168.15.9:80/S_CHAM/Del.php";
            HttpClient solicita = new HttpClient();
            Dictionary<string, string> pesq = new Dictionary<string, string>();
            pesq.Add("id_cha_json", C_number);

            var json = JsonConvert.SerializeObject(pesq);
            var conteudo = new StringContent(json, Encoding.UTF8, "application/json");
            conteudo.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage resp = await solicita.PostAsync(uri, conteudo);
                        Console.WriteLine("SERVIDOR " + resp.IsSuccessStatusCode);
            var cont = await resp.Content.ReadAsStringAsync();
                        Console.WriteLine(cont);

            Dictionary<string, string> RespUp = JsonConvert.DeserializeObject<Dictionary<string, string>>(cont);
            if (RespUp["resp"] == "sucesso")
            {
                Toast.MakeText(this, "Deletado com Sucesso", ToastLength.Short).Show();
                StartActivity(typeof(Mnu));

            }
            else
            {
                Toast.MakeText(this, "Não foi possível alter", ToastLength.Short).Show();
            }

        }
        private void Sp_Tipo_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            clas_Editar = (sp_Tipo.SelectedItemId + 1).ToString();
        }

        private async void Bt_Editar_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(usuario.Text) || String.IsNullOrEmpty(dep.Text) || String.IsNullOrEmpty(andar.Text) || String.IsNullOrEmpty(sal.Text) || String.IsNullOrEmpty(prod1.Text) || String.IsNullOrEmpty(desc.Text))
            {
                Toast.MakeText(this, "Campos * Obrigatório", ToastLength.Short).Show();
            }
            else
            {

                string url = "http://192.168.15.9:80/S_CHAM/UPD_V2.php";
                HttpClient solicita = new HttpClient();
                Dictionary<string, string> D_UPD = new Dictionary<string, string>();

                D_UPD.Add("b_cham", C_number);
                D_UPD.Add("b_id_users", usuario.Text);
                D_UPD.Add("b_dep", dep.Text);
                D_UPD.Add("b_andar", andar.Text);
                D_UPD.Add("b_sal", sal.Text);
                D_UPD.Add("b_c_data", DateTime.Now.ToString());
                D_UPD.Add("b_prod1", prod1.Text);
                D_UPD.Add("b_prod2", prod2.Text);
                D_UPD.Add("b_descricao", desc.Text);
                D_UPD.Add("b_id_clas", clas_Editar);



                var json = JsonConvert.SerializeObject(D_UPD);
                Console.WriteLine(json);
                var conteudo = new StringContent(json, Encoding.UTF8, "application/json");
                conteudo.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage resp = await solicita.PostAsync(url, conteudo);
          
                var cont = await resp.Content.ReadAsStringAsync();
                Console.WriteLine("Dados Servidor" + cont);


                Dictionary<string, string> RespUp = JsonConvert.DeserializeObject<Dictionary<string, string>>(cont);
                if (RespUp["resp"] == "sucesso")
                {
                    Toast.MakeText(this, "Alterado com Sucesso", ToastLength.Short).Show();
                    StartActivity(typeof(Mnu));
                }
                else
                {
                    Toast.MakeText(this, "Não foi possível alter", ToastLength.Short).Show();
                }
            }
        }

        

        private async void Sp_Pesq_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            string d_P_Pesq = sp_Pesq.SelectedItem.ToString();
            string uri = "http://192.168.15.9:80/S_CHAM/Pesq_C_RESP_TD.php";
            HttpClient solicita = new HttpClient();
            Dictionary<string, string> pesq = new Dictionary<string, string>();
            pesq.Add("pesq_json", d_P_Pesq);
            var json = JsonConvert.SerializeObject(pesq);
            var conteudo = new StringContent(json, Encoding.UTF8, "application/json");
            conteudo.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage resp = await solicita.PostAsync(uri, conteudo);           
            var cont = await resp.Content.ReadAsStringAsync();            
            Dictionary<string, object> j_PHP = JsonConvert.DeserializeObject<Dictionary<string, object>>(cont);
            C_number = j_PHP["id_chamado"].ToString();
            dep.Text = j_PHP["departamento"].ToString();
            andar.Text = j_PHP["andar"].ToString();
            sal.Text = j_PHP["sala"].ToString();
            dat.Text = j_PHP["c_data"].ToString();
            prod1.Text = j_PHP["prod1"].ToString();
            prod2.Text = j_PHP["prod2"].ToString();
            desc.Text = j_PHP["descricao"].ToString();
            usuario.Text = j_PHP["id_users"].ToString();
            int clas_P = Convert.ToInt32(j_PHP["id_clas"]);
            sp_Tipo.SetSelection(clas_P - 1);

        }
        
    }
}
    