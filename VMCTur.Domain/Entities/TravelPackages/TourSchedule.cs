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
        public string TourComments { get; set; }
        public bool Shared { get; set; }

        public int QuantityAdult { get; set; }
        public int QuantityChild { get; set; }
        public int QuantityElderly { get; set; }

        public string QuantityParticipantsDetails
        {
            get
            {
                string aux = string.Empty;

                if (QuantityAdult > 0)
                    aux += " Adultos: " + QuantityAdult;

                if (QuantityChild > 0)
                    aux += " Crianças: " + QuantityChild;

                if (QuantityElderly > 0)
                    aux += " Idosos: " + QuantityElderly;

                return aux;
            }
        }

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

        public TourSchedule(DateTime dateHourTour, string customerName, int quantityParticipants, string tourNamePasseio, 
                            string tourGuidename, string vehicleModel, string tourComments, bool shared,
                            int quantityAdult, int quantityChild, int quantityElderly)
        {
            DateHourTour = dateHourTour;
            CustomerName = customerName;
            QuantityParticipants = quantityParticipants;
            TourNamePasseio = tourNamePasseio;
            TourGuidename = tourGuidename;
            VehicleModel = vehicleModel;
            TourComments = tourComments;
            Shared = shared;
            QuantityAdult = quantityAdult;
            QuantityChild = quantityChild;
            QuantityElderly = quantityElderly;
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
