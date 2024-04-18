using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ЯмалетдиновЛангуаге
{
    /// <summary>
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        int CountRecords = 0;
        int CountPage = 0;
        int CurrentPage = 0;
        int Client_LW_Size = 0;
        int setSizeForLW = 10;
        int actualSizeLW = 0;
        List<Client> CurrentPageList = new List<Client>();
        List<Client> TableList = new List<Client>();
        public ClientPage()
        {
            InitializeComponent();
            Gender_CB.SelectedIndex = 0;
            ManySort_CB.SelectedIndex = 0;
            var _current_Client = Yamaletdinov_LanguageSchoolEntities.GetContext().Client.ToList();
            Client_LW.ItemsSource = _current_Client;
            Client_LW_Size = _current_Client.Count;
            Page_ComboBox.SelectedIndex = 0;
            UpdateClients();
            
        }
        private void ChangePage(int direction, int? selectedPage)
        {
            if (Page_ComboBox.SelectedIndex == 0)
                setSizeForLW = 10;
            if (Page_ComboBox.SelectedIndex == 1)
                setSizeForLW = 50;
            if (Page_ComboBox.SelectedIndex == 2)
                setSizeForLW = 200;
            if (Page_ComboBox.SelectedIndex == 3)
                setSizeForLW = Client_LW_Size;
            CurrentPageList.Clear();
            CountRecords = TableList.Count;

            if (CountRecords % setSizeForLW > 0)
            {
                CountPage = CountRecords / setSizeForLW + 1;
            }
            else
            {
                CountPage = CountRecords / setSizeForLW;
            }

            Boolean Ifupdate = true;
            int min;
            if (selectedPage.HasValue)
            {
                if (selectedPage >= 0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    min = CurrentPage * setSizeForLW + setSizeForLW < CountRecords ? CurrentPage * setSizeForLW + setSizeForLW : CountRecords;
                    for (int i = CurrentPage * setSizeForLW; i < min; i++)
                    {
                        CurrentPageList.Add(TableList[i]);
                    }
                }
            }
            else
            {
                switch (direction)
                {
                    case 1:
                        if (CurrentPage > 0)
                        {
                            CurrentPage--;
                            min = CurrentPage * setSizeForLW + setSizeForLW < CountRecords ? CurrentPage * setSizeForLW + setSizeForLW : CountRecords;
                            for (int i = CurrentPage * setSizeForLW; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;

                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min = CurrentPage * setSizeForLW + setSizeForLW < CountRecords ? CurrentPage * setSizeForLW + setSizeForLW : CountRecords;
                            for (int i = CurrentPage * setSizeForLW; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                }
            }
            if (Ifupdate)
            {
                PageListBox.Items.Clear();
                for (int i = 1; i <= CountPage; i++)
                {
                    PageListBox.Items.Add(i);
                }
                PageListBox.SelectedIndex = CurrentPage;

                min = CurrentPage * setSizeForLW + setSizeForLW < CountRecords ? CurrentPage * setSizeForLW + setSizeForLW : CountRecords;
                TBCount.Text = actualSizeLW.ToString();
                TBAllRecords.Text = "из" + Client_LW_Size.ToString();

                Client_LW.ItemsSource = CurrentPageList;
                Client_LW.Items.Refresh();
            }
        }
        public void UpdateClients()
        {
            var current_Client = Yamaletdinov_LanguageSchoolEntities.GetContext().Client.ToList();
            if(Gender_CB.SelectedIndex == 1)
            {
                current_Client = current_Client.Where(p => p.GenderCode.Contains('м')).ToList();
            }
            if (Gender_CB.SelectedIndex == 2)
            {
                current_Client = current_Client.Where(p => p.GenderCode.Contains('ж')).ToList();
            }
            current_Client = current_Client.Where(p => p.FirstName.ToLower().Contains(Search.Text.ToLower()) ||
            p.LastName.ToLower().Contains(Search.Text.ToLower()) ||
            p.Patronymic.ToLower().Contains(Search.Text.ToLower()) ||
            p.phone.Contains(Search.Text) || p.Email.Contains(Search.Text.ToLower())).ToList();
            if(ManySort_CB.SelectedIndex == 1)
            {
                current_Client = current_Client.OrderBy(p => p.FirstName).ToList();
            }
            if (ManySort_CB.SelectedIndex == 2)
            {
                current_Client = current_Client.OrderByDescending(p => p.RegistrationDate).ToList();
            }
            if (ManySort_CB.SelectedIndex == 3)
            {
                current_Client = current_Client.OrderByDescending(p => p.countStartDates).ToList();
            }
            Client_LW.ItemsSource = current_Client;
            TableList = current_Client;
            actualSizeLW = TableList.Count;
            ChangePage(0, 0);
        }
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateClients();
        }

        private void Gender_CB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateClients();
        }

        private void Famil_A_Checked(object sender, RoutedEventArgs e)
        {
            UpdateClients();
        }

        private void Younger_Checked(object sender, RoutedEventArgs e)
        {
            UpdateClients();
        }

        private void Famil_Z_Checked(object sender, RoutedEventArgs e)
        {
            UpdateClients();
        }

        private void Older_Checked(object sender, RoutedEventArgs e)
        {
            UpdateClients();
        }

        private void Many_Checked(object sender, RoutedEventArgs e)
        {
            UpdateClients();
        }

        private void AntiMany_Checked(object sender, RoutedEventArgs e)
        {
            UpdateClients();
        }

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);
        }

        private void Page_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateClients();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var currentClient = Client_LW.SelectedItem as Client;
            var currentClientService = Yamaletdinov_LanguageSchoolEntities.GetContext().ClientService.ToList();
            currentClientService = currentClientService.Where(p => p.ClientID == currentClient.ID).ToList();
            if (currentClientService.Count != 0)
                MessageBox.Show("Невозможно выполнить удаление, т.к у клиента есть информация о посещениях");
            else
            {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Yamaletdinov_LanguageSchoolEntities.GetContext().Client.Remove(currentClient);
                        Yamaletdinov_LanguageSchoolEntities.GetContext().SaveChanges();
                        UpdateClients();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }

        private void ManySort_CB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateClients();
        }
        private void EditDutton_Click(object sender, RoutedEventArgs e)
        {
            var currentClient = Client_LW.SelectedItem as Client;
            Manager.MainFrame.Navigate(new AddEditClients(currentClient));
            UpdateClients();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditClients(null));
            UpdateClients();
        }
    }
}
