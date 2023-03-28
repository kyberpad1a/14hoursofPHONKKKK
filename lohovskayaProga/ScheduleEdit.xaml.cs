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
using System.Windows.Shapes;

namespace lohovskayaProga
{
    /// <summary>
    /// Логика взаимодействия для ScheduleEdit.xaml
    /// </summary>
    public partial class ScheduleEdit : Window
    {
        Schedule _schedule = new Schedule();
        public ScheduleEdit(Schedule schedule)
        {
            InitializeComponent();
            _schedule = schedule;
            lbFrom.Content = schedule.Route.Airport.IATACode;
            lbTo.Content = schedule.Route.Airport1.IATACode;
            lbAircrft.Content = schedule.Aircraft.Name;
            dpDate.SelectedDate = schedule.Date;
            tbTime.Text = schedule.Time.ToString();
            tbEconomyPrice.Text = schedule.EconomyPrice.ToString();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ManageFlightSchedules manageFlightSchedulesWindow = new ManageFlightSchedules();
            manageFlightSchedulesWindow.Show();
            this.Close();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (dpDate.Text == "" && tbTime.Text == "" && tbEconomyPrice.Text == "")
                MessageBox.Show("All values must be not null");
            else
            {
                _schedule.Date = dpDate.DisplayDate;
                _schedule.Time = TimeSpan.Parse(tbTime.Text);
                _schedule.EconomyPrice = Convert.ToDecimal(tbEconomyPrice.Text);
                try
                {
                    Session2_05Entities.GetContext().SaveChanges();
                    ManageFlightSchedules manageFlightSchedulesWindow = new ManageFlightSchedules();
                    manageFlightSchedulesWindow.Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
