﻿using System.Data.Entity;
using VMCTur.Domain.Entities.Customers;
using VMCTur.Domain.Entities.TourGuides;
using VMCTur.Domain.Entities.TravelPackages;
using VMCTur.Domain.Entities.Tours;
using VMCTur.Domain.Entities.Users;
using VMCTur.Domain.Entities.Vehicles;
using VMCTur.Infra.Data.Map;
using VMCTur.Domain.Entities.Financial.BillsReceive;
using VMCTur.Domain.Entities.Financial.BillsPay;
using VMCTur.Domain.Entities.Reservations;

namespace VMCTur.Infra.Data
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class AppDataContext : DbContext
    {
        public AppDataContext() : base("AppConnectionString")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;        
                        
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<TourGuide> TourGuides { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<TravelPackage> TravelPackages { get; set; }
        public DbSet<BillReceive> BillReceives { get; set; }
        public DbSet<BillPay> BillPays { get; set; }
        public DbSet<TravelPackageTour> TravelPackageTours { get; set; }
        public DbSet<TravelPackageParticipant> TravelPackageParticipants { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<UserLog> UserLog { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new CustomerMap());
            modelBuilder.Configurations.Add(new VehicleMap());
            modelBuilder.Configurations.Add(new TourGuideMap());            
            modelBuilder.Configurations.Add(new TourMap());
            modelBuilder.Configurations.Add(new TravelPackageMap());
            modelBuilder.Configurations.Add(new TravelPackageTourMap());
            modelBuilder.Configurations.Add(new TravelPackageParticipantMap());
            modelBuilder.Configurations.Add(new BillReceiveMap());
            modelBuilder.Configurations.Add(new BillPayMap());
            modelBuilder.Configurations.Add(new ReservationMap());
            modelBuilder.Configurations.Add(new UserLogMap());
        }
    }
}
