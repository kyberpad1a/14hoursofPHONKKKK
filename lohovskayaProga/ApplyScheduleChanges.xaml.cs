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
    /// Логика взаимодействия для ApplyScheduleChanges.xaml
    /// </summary>
    public partial class ApplyScheduleChanges : Window
    {
        
        public ApplyScheduleChanges()
        {
            InitializeComponent();
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "CSV files (*.csv)|*.csv|XML files (*.xml)|*.xml"
            };
            dialog.Title = "Choose import file";
            if (dialog.ShowDialog() == true)
            {
                string filepath = dialog.FileName;
                List<Schedule> schedules = new List<Schedule>();
                int counterFailed = 0;
                tbImport.Text = filepath;
                //List<string[]> stringList = new List<string[]>();
                using (StreamReader reader = new StreamReader(filepath))
                {
                    while (!reader.EndOfStream)
                    {
                        try
                        {
                           
                            string line = reader.ReadLine();
                            string[] values = line.Split(',');
                            //stringList.Add(values);
                            Schedule schedule = new Schedule();
                            List<Airport> airoportList = Session2_05Entities.GetContext().Airports.ToList();
                            Airport airport = airoportList.Where(x => x.IATACode.Equals(values[4].ToString())).FirstOrDefault();
                            List<Airport> airport1List = Session2_05Entities.GetContext().Airports.ToList();
                            Airport airport1 = airport1List.Where(x => x.IATACode.Equals(values[5].ToString())).FirstOrDefault();
                            List<Aircraft> aircraftList = Session2_05Entities.GetContext().Aircrafts.ToList();
                            Aircraft aircraft = aircraftList.Where(x => x.ID.Equals(Convert.ToInt32(values[6]))).FirstOrDefault();
                            List<Route> routeList = Session2_05Entities.GetContext().Routes.ToList();
                            Route route = routeList.Where(x => x.Airport.Equals(airport) && x.Airport1.Equals(airport1)).FirstOrDefault();


                            schedule.Date = Convert.ToDateTime(values[1]);
                            schedule.Time = TimeSpan.Parse(values[2]);
                            schedule.FlightNumber = values[3];
                            schedule.EconomyPrice = Convert.ToDecimal(values[7].Replace(".", ","));
                            schedule.Confirmed = Convert.ToBoolean(values[8].Replace("OK", "true").Replace("CANCELED", "false"));
                            schedule.Route = route;
                            schedule.Aircraft = aircraft;
                            schedule.AircraftID = aircraft.ID;
                            schedule.RouteID = route.ID;

                            schedules.Add(schedule);
                        }
                        catch
                        {
                            counterFailed += 1;
                        }

                    }

                    foreach(Schedule item in schedules.Distinct().ToList())
                    {
                        try
                        {
                            Session2_05Entities.GetContext().Schedules.Add(item);
                            Session2_05Entities.GetContext().SaveChanges();
                        }
                        catch
                        {
                            counterFailed += 1;
                        }
                    }


                   lbDuplicate.Content = schedules.Count() - schedules.Distinct().ToList().Count();
                    lbSuccessful.Content = schedules.Count() - counterFailed - (schedules.Count() - schedules.Distinct().ToList().Count());
                    lbMissing.Content = counterFailed;
                }
                //var fileData = File.ReadAllLines(dialog.FileName);
                //foreach(var line in fileData)
                //{
                //    var data = line.Split('\t');

                //    //var tempSchedules = new Sche
                //}
            }
        }
    }
}
