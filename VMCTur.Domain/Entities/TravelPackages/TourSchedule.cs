using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMCTur.Domain.Entities.TravelPackages
{
    public class TourSchedule
    {
        #region Properties

        public DateTime DateHourTour { get; set; }
        public string CustomerName { get; set; }
        public int QuantityParticipants { get; set; }
        public string TourNamePasseio { get; set; }
        public string TourGuidename { get; set; }
        public string VehicleModel { get; set; }

        //Smael: return the color of the day for organize the schedule.
        public string ColorOfDay
        {
            get
            {
                return GetColorOfDay();
            }
        }

        #endregion

        #region Ctor

        public TourSchedule() { }

        public TourSchedule(DateTime dateHourTour, string customerName, int quantityParticipants, string tourNamePasseio, string tourGuidename, string vehicleModel)
        {
            DateHourTour = dateHourTour;
            CustomerName = customerName;
            QuantityParticipants = quantityParticipants;
            TourNamePasseio = tourNamePasseio;
            TourGuidename = tourGuidename;
            VehicleModel = vehicleModel;
        }

        #endregion

        #region Methods     

        private string GetColorOfDay()
        {
            string color = string.Empty;

            switch (DateHourTour.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    color = "#46A510";
                    break;
                case DayOfWeek.Tuesday:
                    color = "#FFD700";
                    break;
                case DayOfWeek.Wednesday:
                    color = "#FE4902";
                    break;
                case DayOfWeek.Thursday:
                    color = "#58A8E0";
                    break;
                case DayOfWeek.Friday:
                    color = "#FA8072";
                    break;
                case DayOfWeek.Saturday:
                    color = "#C7C7C7";
                    break;
                case DayOfWeek.Sunday:
                    color = "#FFA85A";
                    break;
                default:
                    color = "#46A510";
                    break;
            }

            return color;
        }

        #endregion
    }
}
