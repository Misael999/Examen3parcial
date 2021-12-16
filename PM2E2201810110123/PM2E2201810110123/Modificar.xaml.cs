using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2E2201810110123
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Modificar : ContentPage
    {
        public DateTime ultima;
        public Modificar()
        {
            InitializeComponent();
        }

        private async void BtnLista_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaDePagos());
        }

        private async void Btnmodificar_Clicked(object sender, EventArgs e)
        {
            string imagen = pathFoto.Text;

            //convertir a arreglo de bytes
            byte[] fileByte = System.IO.File.ReadAllBytes(imagen);

            //convertir a base64
            string pathBase64 = Convert.ToBase64String(fileByte);

            var person = new Models.CPagos
            {
                Idpago = Convert.ToInt32(idSitio.Text),
                descripcion = txtdescrip.Text,
                monto = (double)Convert.ToDecimal(txtmonto.Text),
                imagen = pathBase64

            };
            if (await App.SQLiteDB.Grabarpersona(person) != 0)
            {
                await DisplayAlert("Registro", "Datos Modificados de manera correcta", "ok");
                await Navigation.PushAsync(new ListaDePagos());

            }
            else
            {
                await DisplayAlert("Registro", "Ha ocurrido un problema", "ok");
            }
        }

        private async void BtnBorrar_Clicked(object sender, EventArgs e)
        {
            string imagen = pathFoto.Text;

            //convertir a arreglo de bytes
            byte[] fileByte = System.IO.File.ReadAllBytes(imagen);

            //convertir a base64
            string pathBase64 = Convert.ToBase64String(fileByte);

            var person = new Models.CPagos
            {
                Idpago = Convert.ToInt32(idSitio.Text),
                descripcion = txtdescrip.Text,
                monto = (double)Convert.ToDecimal(txtmonto.Text),
                imagen = pathBase64

            };
            if (await App.SQLiteDB.DropPersonaAsync(person) != 0)
            {
                await DisplayAlert("Registro", "Datos Borrados de manera correcta", "ok");
                await Navigation.PushAsync(new ListaDePagos());
            }
            else
            {
                await DisplayAlert("Registro", "Ha ocurrido un problema", "ok");
            }
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

        private void fecha_DateSelected(object sender, DateChangedEventArgs e)
        {
            ultima = e.NewDate;
            var observar = e.NewDate.ToString();
        }
    }
}