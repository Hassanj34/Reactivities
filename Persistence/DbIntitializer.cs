using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class DbInitializer
    {
        public static async Task SeedData(AppDbContext context, UserManager<User> userManager)
        {
            // Generate GUIDs once and reuse them
            var userBobId = Guid.NewGuid().ToString();
            var userTomId = Guid.NewGuid().ToString();
            var userJaneId = Guid.NewGuid().ToString();
            var userAliId = Guid.NewGuid().ToString();
            var userMariaId = Guid.NewGuid().ToString();
            var userKenjiId = Guid.NewGuid().ToString();

            if (!userManager.Users.Any())
            {
                var users = new List<User>
                {
                    new() { Id = userBobId, DisplayName = "Bob Martin", UserName = "bob.martin@test.com", Email = "bob.martin@test.com" },
                    new() { Id = userTomId, DisplayName = "Tom Walker", UserName = "tom.walker@test.com", Email = "tom.walker@test.com" },
                    new() { Id = userJaneId, DisplayName = "Jane Wilson", UserName = "jane.wilson@test.com", Email = "jane.wilson@test.com" },
                    new() { Id = userAliId, DisplayName = "Ali Khan", UserName = "ali.khan@test.com", Email = "ali.khan@test.com" },
                    new() { Id = userMariaId, DisplayName = "Maria Rossi", UserName = "maria.rossi@test.com", Email = "maria.rossi@test.com" },
                    new() { Id = userKenjiId, DisplayName = "Kenji Tanaka", UserName = "kenji.tanaka@test.com", Email = "kenji.tanaka@test.com" }
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }
            }

            if (context.Activities.Any()) return;

            var activities = new List<Activity>
            {
                new()
                {
                    Title = "London Tech Meetup",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Monthly meetup for software developers",
                    Category = "tech",
                    City = "London",
                    Venue = "Google Campus London",
                    Latitude = 51.522462,
                    Longitude = -0.088654,
                    Attendees =
                    [
                        new() { UserId = userBobId, IsHost = true },
                        new() { UserId = userTomId },
                        new() { UserId = userJaneId }
                    ]
                },
                new()
                {
                    Title = "Paris Art Exhibition",
                    Date = DateTime.Now.AddMonths(-1),
                    Description = "Modern art exhibition at the Louvre",
                    Category = "culture",
                    City = "Paris",
                    Venue = "Louvre Museum",
                    Latitude = 48.8611473,
                    Longitude = 2.3380276,
                    Attendees =
                    [
                        new() { UserId = userMariaId, IsHost = true },
                        new() { UserId = userJaneId }
                    ]
                },
                new()
                {
                    Title = "New York Startup Pitch Night",
                    Date = DateTime.Now.AddMonths(1),
                    Description = "Pitch night for early-stage startups",
                    Category = "business",
                    City = "New York",
                    Venue = "WeWork Manhattan",
                    Latitude = 40.712776,
                    Longitude = -74.005974,
                    Attendees =
                    [
                        new() { UserId = userTomId, IsHost = true },
                        new() { UserId = userBobId },
                        new() { UserId = userAliId }
                    ]
                },
                new()
                {
                    Title = "Tokyo React Conference",
                    Date = DateTime.Now.AddMonths(2),
                    Description = "Conference focused on React and frontend tech",
                    Category = "tech",
                    City = "Tokyo",
                    Venue = "Tokyo International Forum",
                    Latitude = 35.6764,
                    Longitude = 139.6993,
                    Attendees =
                    [
                        new() { UserId = userKenjiId, IsHost = true },
                        new() { UserId = userBobId }
                    ]
                },
                new()
                {
                    Title = "Rome Historical Walking Tour",
                    Date = DateTime.Now.AddMonths(2),
                    Description = "Guided tour through ancient Rome",
                    Category = "travel",
                    City = "Rome",
                    Venue = "Colosseum",
                    Latitude = 41.8902,
                    Longitude = 12.4922,
                    Attendees =
                    [
                        new() { UserId = userMariaId, IsHost = true },
                        new() { UserId = userJaneId }
                    ]
                },
                new()
                {
                    Title = "Dubai Networking Dinner",
                    Date = DateTime.Now.AddMonths(3),
                    Description = "Business networking dinner",
                    Category = "business",
                    City = "Dubai",
                    Venue = "Burj Khalifa Lounge",
                    Latitude = 25.1972,
                    Longitude = 55.2744,
                    Attendees =
                    [
                        new() { UserId = userAliId, IsHost = true },
                        new() { UserId = userTomId }
                    ]
                },
                new()
                {
                    Title = "Berlin Music Festival",
                    Date = DateTime.Now.AddMonths(3),
                    Description = "Outdoor electronic music festival",
                    Category = "music",
                    City = "Berlin",
                    Venue = "Tempelhofer Feld",
                    Latitude = 52.4730,
                    Longitude = 13.4039,
                    Attendees =
                    [
                        new() { UserId = userBobId, IsHost = true },
                        new() { UserId = userMariaId }
                    ]
                },
                new()
                {
                    Title = "Sydney Beach Yoga",
                    Date = DateTime.Now.AddMonths(4),
                    Description = "Morning yoga session by the beach",
                    Category = "fitness",
                    City = "Sydney",
                    Venue = "Bondi Beach",
                    Latitude = -33.8908,
                    Longitude = 151.2743,
                    Attendees =
                    [
                        new() { UserId = userJaneId, IsHost = true },
                        new() { UserId = userKenjiId }
                    ]
                },
                new()
                {
                    Title = "San Francisco Cloud Workshop",
                    Date = DateTime.Now.AddMonths(4),
                    Description = "Hands-on cloud computing workshop",
                    Category = "tech",
                    City = "San Francisco",
                    Venue = "Salesforce Tower",
                    Latitude = 37.7897,
                    Longitude = -122.3960,
                    Attendees =
                    [
                        new() { UserId = userTomId, IsHost = true },
                        new() { UserId = userBobId }
                    ]
                },
                new()
                {
                    Title = "Barcelona Food Tour",
                    Date = DateTime.Now.AddMonths(5),
                    Description = "Local food tasting experience",
                    Category = "food",
                    City = "Barcelona",
                    Venue = "La Boqueria Market",
                    Latitude = 41.3826,
                    Longitude = 2.1719,
                    Attendees =
                    [
                        new() { UserId = userMariaId, IsHost = true },
                        new() { UserId = userAliId }
                    ]
                },
                new()
                {
                    Title = "Toronto AI Meetup",
                    Date = DateTime.Now.AddMonths(6),
                    Description = "AI and machine learning meetup",
                    Category = "tech",
                    City = "Toronto",
                    Venue = "MaRS Discovery District",
                    Latitude = 43.6596,
                    Longitude = -79.3871,
                    Attendees =
                    [
                        new() { UserId = userKenjiId, IsHost = true },
                        new() { UserId = userTomId }
                    ]
                },
                new()
                {
                    Title = "Istanbul Photography Walk",
                    Date = DateTime.Now.AddMonths(6),
                    Description = "Street photography walk",
                    Category = "art",
                    City = "Istanbul",
                    Venue = "Galata Tower",
                    Latitude = 41.0256,
                    Longitude = 28.9744,
                    Attendees =
                    [
                        new() { UserId = userAliId, IsHost = true },
                        new() { UserId = userJaneId }
                    ]
                }
            };

            context.Activities.AddRange(activities);
            await context.SaveChangesAsync();
        }
    }
}