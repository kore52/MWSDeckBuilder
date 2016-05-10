using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace MWSDeckBuilder
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<MagicCardBase> database;
        private ObservableCollection<MagicDeckCard> mainboard = new ObservableCollection<MagicDeckCard>();
        private ObservableCollection<MagicDeckCard> sideboard = new ObservableCollection<MagicDeckCard>();

        public MainWindow()
        {
            InitializeComponent();

            LoadDatabase();
        }

        private void LoadDatabase()
        {
            var csv = new CsvReader(new StreamReader("CardSet.csv"));
            csv.Configuration.RegisterClassMap<MagicCardClassMap>();
            database = csv.GetRecords<MagicCardBase>().OrderBy(x => x.Edition).ThenBy(x => x.No).ToList();

            GridDatabase.ItemsSource = new ObservableCollection<MagicCardBase>(database);
        }

        private void OpenDeckFile(string deckFilePath)
        {
            try
            {
                var reader = new MagicDeckReader(database);
                var deck = reader.OpenDeckFile(deckFilePath);
                GridMainboard.ItemsSource = deck.Item1;
                GridSideboard.ItemsSource = deck.Item2;
            }
            catch(Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }
        }

        private void SaveDeckFile(string filename)
        {
            try
            {
                var writer = new MagicDeckWriter(database);
                var deck = new Tuple<ObservableCollection<MagicDeckCard>, ObservableCollection<MagicDeckCard>>(mainboard, sideboard);
                writer.SaveDeckFile(filename, deck);
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }
        }

        private void AddToDeck(ObservableCollection<MagicDeckCard> deck, IList items, int amount)
        {
            foreach (var item in items)
            {
                var card = item as MagicCardBase;
                var newcard = new MagicDeckCard(card);
                var result = deck.Where(x => x.Name == card.Name && x.Edition == card.Edition).ToList();
                if (result.Count > 0)
                {
                    for (int i = 0; i < deck.Count; i++)
                    {
                        if (deck[i].Name == card.Name && deck[i].Edition == card.Edition)
                        {
                            deck[i].Amount += amount;
                        }
                    }
                }
                else
                {
                    newcard.Amount += amount;
                    deck.Add(newcard);
                }
            }
        }

        private void RemoveFromDeck(ObservableCollection<MagicDeckCard> deck, IList items, int amount)
        {
            foreach (var item in items)
            {
                var card = item as MagicDeckCard;
                var result = deck.Where(x => x.Name == card.Name && x.Edition == card.Edition).ToList();
                if (result.Count > 0)
                {
                    for (int i = 0; i < deck.Count; i++)
                    {
                        if (deck[i].Name == card.Name && deck[i].Edition == card.Edition)
                        {
                            deck[i].Amount -= amount;
                            if (deck[i].Amount <= 0)
                                deck.RemoveAt(i);
                        }
                    }
                }
                else
                {
                    deck.Remove(card);
                }
            }
        }

        private void AddToMainboard(IList items, int amount)
        {
            AddToDeck(mainboard, items, amount);
            GridMainboard.ItemsSource = new ObservableCollection<MagicCardBase>(mainboard.OrderBy(x => x.Name));
        }

        private void AddToSideboard(IList items, int amount)
        {
            AddToDeck(sideboard, items, amount);
            GridSideboard.ItemsSource = new ObservableCollection<MagicCardBase>(sideboard.OrderBy(x => x.Name));
        }

        private void RemoveFromMainboard(IList items, int amount)
        {
            RemoveFromDeck(mainboard, items, amount);
            GridMainboard.ItemsSource = new ObservableCollection<MagicCardBase>(mainboard.OrderBy(x => x.Name));
        }

        private void RemoveFromSideboard(IList items, int amount)
        {
            RemoveFromDeck(sideboard, items, amount);
            GridSideboard.ItemsSource = new ObservableCollection<MagicCardBase>(sideboard.OrderBy(x => x.Name));
        }

        private void OnClick_ButtonMoveBaseToMain(object sender, RoutedEventArgs e)
        {
            AddToMainboard(GridDatabase.SelectedItems, 1);
        }

        private void OnClick_ButtonMoveBaseToMain4(object sender, RoutedEventArgs e)
        {
            AddToMainboard(GridDatabase.SelectedItems, 4);
        }

        private void OnClick_ButtonDeleteMain(object sender, RoutedEventArgs e)
        {
            RemoveFromMainboard(GridMainboard.SelectedItems, 1);
        }

        private void OnClick_ButtonDeleteMain4(object sender, RoutedEventArgs e)
        {
            RemoveFromMainboard(GridMainboard.SelectedItems, 4);
        }

        private void OnClick_ButtonMoveBaseToSide(object sender, RoutedEventArgs e)
        {
            AddToSideboard(GridDatabase.SelectedItems, 1);
        }

        private void OnClick_ButtonDeleteSide(object sender, RoutedEventArgs e)
        {
            RemoveFromSideboard(GridSideboard.SelectedItems, 1);
        }

        private void OnClick_ButtonMoveMainToSide(object sender, RoutedEventArgs e)
        {
            AddToSideboard(GridMainboard.SelectedItems, 1);
            RemoveFromMainboard(GridMainboard.SelectedItems, 1);
        }

        private void OnClick_ButtonMoveSideToMain(object sender, RoutedEventArgs e)
        {
            AddToMainboard(GridSideboard.SelectedItems, 1);
            RemoveFromSideboard(GridSideboard.SelectedItems, 1);
        }

        private void OnClick_ButtonSaveDeck(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.FilterIndex = 1;
            dlg.Filter = "MWS Format|*.mwDeck|MO Format|*.txt";
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                var newFileName = dlg.SafeFileName;
                SaveDeckFile(newFileName);
            }
        }

        private void OnClick_ButtonOpenDeck(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "Supported Deck Format|*.txt;*.mwDeck";
            dlg.CheckFileExists = true;
            dlg.CheckPathExists = true;

            bool? result = dlg.ShowDialog();
            string deckFilePath;
            if (result == true)
                deckFilePath = dlg.FileName;
            else
                return;

            OpenDeckFile(deckFilePath);
        }
    }
}
