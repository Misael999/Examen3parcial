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
    public partial class ListaDePagos : ContentPage
    {
        public ListaDePagos()
        {
            InitializeComponent();
            LlenarDatos();
        }

        public async void LlenarDatos()
        {
            var personlist = await App.SQLiteDB.GetPersonasAync();
            if (personlist != null)
            {
                lstdatos.ItemsSource = personlist;
            }
            else
            {

            }
        }

        private async void lstdatos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Models.CPagos item = (Models.CPagos)e.Item;

            var page = new Modificar();
            page.BindingContext = item;
            await Navigation.PushAsync(page);
        }
    }
}