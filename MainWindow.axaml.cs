using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Linq;

namespace PlanerDnia;

public partial class MainWindow : Window
{
    public ZadanieButton wybraneZadanie;
    public ObservableCollection<ZadanieButton> ListaZadan { get; set; }
    public MainWindow()
    {
        InitializeComponent();
        ListaZadan = new ObservableCollection<ZadanieButton>();
        Stworz.Click += Stworz_button_Click;
    }

    public class ZadanieButton{
        public string Nazwa{get;set;}
        public string Kategoria{get;set;}
        public bool CzyUkonczone{get;set;}

        public ZadanieButton(string nazwa, string kategoria)
        {
            Nazwa = nazwa;
            Kategoria = kategoria;
            CzyUkonczone = false;
        }
        public override string ToString()
        {
            return $"{Nazwa} ({Kategoria}) - {(CzyUkonczone ? "Ukończone" : "Nieukończone")}";
        }
        
    }
    
    private void Stworz_button_Click(object sender, RoutedEventArgs e)
    {
        var valueZadanie = Zadanie.Text;
        var valueprzedmiot = (ComboBoxZadania.SelectedItem as ComboBoxItem) ?.Content?.ToString() ?? "Nie wybrane";
        
        if (!string.IsNullOrEmpty(valueZadanie) && !string.IsNullOrEmpty(valueprzedmiot))
        {
            ZadanieButton newZadanieButton= new ZadanieButton(valueZadanie, valueprzedmiot);
            ListaZadan.Add(newZadanieButton);
            ListaZadanBox.Items.Add(newZadanieButton);
            Zadanie.Clear();
            ComboBoxZadania.SelectedIndex = -1;
            Odswiez();
        }
    }
    

    
    private void ListaZadanBoxChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ListaZadanBox.SelectedItem != null)
        {
            wybraneZadanie = (ZadanieButton)ListaZadanBox.SelectedItem;
            Ukonczone.IsChecked = wybraneZadanie.CzyUkonczone;
            EdytujZadanie.Text = wybraneZadanie.Nazwa;
            foreach (ComboBoxItem item in EdytujKategorie.Items)
            {
                if (item.Content.ToString() == wybraneZadanie.Kategoria)
                {
                    EdytujKategorie.SelectedItem = item;
                    break;
                }
            }
        }
    }
    
    private void ZapiszZmianyButton(object sender, RoutedEventArgs e)
    {
        if (wybraneZadanie != null)
        {
            wybraneZadanie.Nazwa = EdytujZadanie.Text;
            wybraneZadanie.Kategoria = ((ComboBoxItem)EdytujKategorie.SelectedItem).Content.ToString();
            wybraneZadanie.CzyUkonczone = Ukonczone.IsChecked ?? false;
            ListaZadanBox.Items.Clear();
            foreach (var zadanie in ListaZadan)
            {
                ListaZadanBox.Items.Add(zadanie);
            }

            Odswiez();
        }
    }
        
    private void Odswiez()
    {
        int wszystkieZad = ListaZadan.Count;
        int ukonczoneZad = ListaZadan.Count(zadanie => zadanie.CzyUkonczone);
        Podsumowanie.Text = $"Zadania: {wszystkieZad}, Ukończone: {ukonczoneZad}";
    }
    
    private void UsunZadanieButton(object sender, RoutedEventArgs e)
    {
        if (wybraneZadanie != null)
        {
            ListaZadan.Remove(wybraneZadanie);
            ListaZadanBox.Items.Remove(wybraneZadanie);
            wybraneZadanie = null;
            EdytujZadanie.Clear();
            Ukonczone.IsChecked = false;
            Odswiez();
        }
    }
    
    }