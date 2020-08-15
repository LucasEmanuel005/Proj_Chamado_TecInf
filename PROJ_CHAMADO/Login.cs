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
using Android.Graphics.Drawables;


namespace PROJ_CHAMADO
{
    [Activity(Label = "Login")]
    public class Login : Activity
    {
        ImageView img_Login;
        EditText email, senha;
        Button enviar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);
            email = FindViewById<EditText>(Resource.Id.cpEmail);
            senha = FindViewById<EditText>(Resource.Id.cpSenha);
            enviar = FindViewById<Button>(Resource.Id.btEntrar);
            img_Login = FindViewById<ImageView>(Resource.Id.img_Login);
            enviar.Click += Enviar_Click;

            Drawable cam;
            cam = Resources.GetDrawable(Resource.Drawable.suporte);
            img_Login.SetImageDrawable(cam);

        }

        private async void Enviar_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(email.Text) || String.IsNullOrEmpty(senha.Text))
            { 
                Toast.MakeText(this, "Campos Obrigatório!!", ToastLength.Short).Show();

            }
            else
            {
                string url = "http://192.168.15.9:80/S_CHAM/login.php";
                HttpClient sol = new HttpClient();
                Dictionary<string, string> dados = new Dictionary<string, string>();
                dados.Add("email_j", email.Text);
                dados.Add("senha_j", senha.Text);
                var c_json = JsonConvert.SerializeObject(dados);

                


                var ctd = new StringContent(c_json, Encoding.UTF8, "application/json");
                ctd.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage res = await sol.PostAsync(url, ctd);

                if (res.IsSuccessStatusCode == false)
                {
                    Toast.MakeText(this, "Conecte a Internet ou ao servidor ", ToastLength.Short).Show();
                }
                else 
                {

                    var cont = await res.Content.ReadAsStringAsync();


                    Dictionary<string, string> j_PHP = JsonConvert.DeserializeObject<Dictionary<string, string>>(cont);
                    if (j_PHP["resp"] == "yes")
                    {

                        StartActivity(typeof(Mnu));
                        email.Text = null;
                        senha.Text = null;
                    }
                    else
                    {
                        Toast.MakeText(this, "Usuário não cadastrado", ToastLength.Short).Show();
                    }
                }




            }

        }
    }
}