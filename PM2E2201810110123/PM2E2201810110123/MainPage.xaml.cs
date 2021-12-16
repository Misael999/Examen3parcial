using Plugin.Media;
using PM2E2201810110123.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PM2E2201810110123
{
    public partial class MainPage : ContentPage
    {
        public DateTime ultima;
        public string imagenglobal = null;
        public MainPage()
        {
            InitializeComponent();
        }

        private async void btnBuscarFoto_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Alerta", "No se puede elegir una foto", "OK");
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync();
            if (file == null)
                return;
            pathFoto.Text = file.Path;
            fotografia.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
        }

        private async void btnTomarFoto_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Alerta", "Cámara no disponible", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                SaveToAlbum = true
            });

            if (file == null)
                return;

            pathFoto.Text = file.AlbumPath;

            //convertir a arreglo de bytes
            byte[] fileByte = System.IO.File.ReadAllBytes(file.AlbumPath);

            //convertir a base64
            string pathBase64 = Convert.ToBase64String(fileByte);


            fotografia.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

        }

        private async void Btnguardar_Clicked(object sender, EventArgs e)
        {

            string imagen = pathFoto.Text;

            //convertir a arreglo de bytes
            byte[] fileByte = System.IO.File.ReadAllBytes(imagen);

            //convertir a base64
            string pathBase64 = Convert.ToBase64String(fileByte);

            if (ValidarDatos())
            {
                CPagos person = new CPagos
                {


                    descripcion = txtdescrip.Text,
                    monto = (double)Convert.ToDecimal(txtmonto.Text),   
                    imagen = pathBase64,

                };
                await App.SQLiteDB.SavePersona(person);

                await DisplayAlert("Registro", "Datos agregados de manera correcta", "ok");
            }
            else
            {
                await DisplayAlert("Advertencia", "Ingresar todos los datos", "ok");
            }
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            ultima = e.NewDate;
            var observar = e.NewDate.ToString();
        }

        public bool ValidarDatos()
        {
            bool respuesta;

            if (String.IsNullOrEmpty(txtdescrip.Text))
            {
                respuesta = false;
            }

            else if (String.IsNullOrEmpty(txtmonto.Text))
            {
                respuesta = false;
            }

            else
            {
                respuesta = true;
            }
            return respuesta;

        }

        private async void BtnLista_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaDePagos());
        }
    }
}
