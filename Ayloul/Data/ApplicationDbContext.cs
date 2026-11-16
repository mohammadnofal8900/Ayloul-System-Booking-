using Ayloul.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ayloul.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option) { }


        public DbSet<Background> Backgrounds { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<FrameColor> FrameColors { get; set; }
        public DbSet<Location>  Locations { get; set; }
        public DbSet<RoseColor> RoseColors { get; set; }
        public DbSet<RoseType> RoseTypes { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<BookingService> BookingServices { get; set; }
        
        public DbSet<Customer> Customers { get; set; }
        public DbSet<AuditDBooking> auditBookings { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ChatRoom>()
                .HasOne(c => c.Customer)
                .WithMany()
                .HasForeignKey(c => c.Customer_Id)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<ChatRoom>()
                .HasOne(c => c.Employee)
                .WithMany()
                .HasForeignKey(c => c.Employee_Id)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.Sender_Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Rating>()
                .HasOne(r => r.Booking)
                .WithOne(b => b.Rating)
                .HasForeignKey<Rating>(r => r.Booking_Id)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<Rating>()
                .HasOne(r => r.Customer)
                .WithMany()
                .HasForeignKey(r => r.Customer_Id)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<Rating>()
                .HasOne(r => r.Employee)
                .WithMany()
                .HasForeignKey(r => r.Employee_Id)
                .OnDelete(DeleteBehavior.NoAction);
            

            builder.Entity<Employee>()
        .ToTable("Employees", tb => tb.HasTrigger("TR_Employee_Delete"));
            builder.Entity<ChatRoom>()
       .HasOne(c => c.Customer)
       .WithMany()
       .HasForeignKey(c => c.Customer_Id)
       .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Booking>()
        .ToTable("Bookings", tb => tb.HasTrigger("TR_Booking_Delete"));

            builder.Entity<ChatRoom>()
       .HasOne(c => c.Customer)
       .WithMany()
       .HasForeignKey(c => c.Customer_Id)
       .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ChatRoom>()
                .HasOne(c => c.Employee)
                .WithMany()
                .HasForeignKey(c => c.Employee_Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Message>()
                .HasOne(m => m.ChatRoom)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatRoom_Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.Sender_Id)
                .OnDelete(DeleteBehavior.NoAction);
        }



    }

}
