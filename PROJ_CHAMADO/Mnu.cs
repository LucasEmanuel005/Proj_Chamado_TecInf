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
using Android.Graphics.Drawables;

namespace PROJ_CHAMADO
{
    [Activity(Label = "Mnu")]
    public class Mnu : Activity
    {
        ImageView img_ab, img_Cha_abs;
        Button abrir, exibir;

        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Mnu);
            abrir = FindViewById<Button>(Resource.Id.btAbrir);
            exibir = FindViewById<Button>(Resource.Id.bt_exibir);
            img_ab = FindViewById<ImageView>(Resource.Id.img_Ab);
            img_Cha_abs = FindViewById<ImageView>(Resource.Id.img_Ch_abs);

            Drawable cam, cam2;
            cam = Resources.GetDrawable(Resource.Drawable.aber_Cha);
            img_ab.SetImageDrawable(cam);
            cam2 = Resources.GetDrawable(Resource.Drawable.Visualizar);
            img_Cha_abs.SetImageDrawable(cam2);

            img_ab.Click += Img_ab_Click;

            abrir.Click += Abrir_Click;
            exibir.Click += Exibir_Click;
        }

        private void Img_ab_Click(object sender, EventArgs e)
        {
            
        }

        private void Exibir_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(exibir));
        }

        private void Abrir_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(abrir));
        }
    }
}