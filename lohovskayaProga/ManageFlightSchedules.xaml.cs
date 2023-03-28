using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace lohovskayaProga
{
    /// <summary>
    /// Логика взаимодействия для ManageFlightSchedules.xaml
    /// </summary>
    /// 

    /*public Nullable<double> BuisnessPrice
    {
        get
        {
            return Convert.ToDouble(EconomyPrice) * 1.35;
        }
        set
        {
            BuisnessPrice = Convert.ToDouble(EconomyPrice) * 1.35;
        }
    }
    public Nullable<double> FirstClassPrice
    {
        get
        {
            return BuisnessPrice * 1.3;
        }
        set
        {

        }
    }*/
    public partial class ManageFlightSchedules : Window
    {
        private Schedule _currentSchedule = new Schedule();
        private Route _currentRoute = new Route();
        private Airport _currentAircraft = new Airport();

        public ManageFlightSchedules()
        {
            InitializeComponent();
            UpdateWindow();
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyScheduleChanges applyScheduleChanges = new ApplyScheduleChanges();
            applyScheduleChanges.Show();
            this.Close();
        }


        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            List<Schedule> schedules = Session2_05Entities.GetContext().Schedules.ToList();
            if (cbFrom.Text == "" && cbTo.Text == "" && cbSortBy.Text == "" && dpOubound.Text == "" && tbFlightNumber.Text == "")
                DGridSchedules.ItemsSource = schedules;
            else if(cbFrom.Text ==  cbTo.Text )
            {
                DGridSchedules.ItemsSource = null;
            }
            else
            {
                if (cbFrom.Text == "")
                    cbFrom.SelectedItem = null;
                if (cbTo.Text == "")
                    cbTo.SelectedItem = null;
                DGridSchedules.ItemsSource = schedules.Where(
                    schedule => schedule.Route.Airport.Equals(cbFrom.SelectedItem) ||
                    schedule.Route.Airport1.Equals(cbTo.SelectedItem) ||
                    schedule.Date.Equals(dpOubound.DisplayDate) ||
                    schedule.FlightNumber.Equals(tbFlightNumber.Text)
                    ).ToList();
            }
        }

        private void UpdateWindow()
        {
            List<Airport> air = Session2_05Entities.GetContext().Airports.ToList();
            air.Add(new Airport());
            cbFrom.ItemsSource = cbTo.ItemsSource = air;
            cbSortBy.ItemsSource = Session2_05Entities.GetContext().Schedules.ToList();
            List<Schedule> schedules = Session2_05Entities.GetContext().Schedules.ToList();
            DGridSchedules.ItemsSource = schedules;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (DGridSchedules.SelectedIndex != -1)
            {
                Schedule _currentSchedule = (Schedule)DGridSchedules.SelectedItem;
                _currentSchedule.Confirmed = !_currentSchedule.Confirmed;

                try
                {
                    Session2_05Entities.GetContext().SaveChanges();
                    UpdateWindow();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (DGridSchedules.SelectedIndex != -1)
            {
                new ScheduleEdit((Schedule)DGridSchedules.SelectedItem).Show();
                this.Close();
            }
        }
    }
}
