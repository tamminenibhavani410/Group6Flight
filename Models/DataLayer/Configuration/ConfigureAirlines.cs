using Group6Flight.Models.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Group6Flight.Models.DataLayer.Configuration
{
    public class ConfigureAirlines : IEntityTypeConfiguration<Airline>
    {
        public void Configure(EntityTypeBuilder<Airline> entity)
        {
            // seed initial data
            entity.HasData(
                new Airline { AirlineId = 1, Name = "American Airlines", ImageName = "american_airlines.png" },
                new Airline { AirlineId = 2, Name = "Delta Air Lines", ImageName = "delta_air_lines.png" },
                new Airline { AirlineId = 3, Name = "United Airlines", ImageName = "united_airlines.png" },
                new Airline { AirlineId = 4, Name = "Southwest Airlines", ImageName = "southwest_airlines.png" },
                new Airline { AirlineId = 5, Name = "Alaska Airlines", ImageName = "alaska_airlines.png" },
                new Airline { AirlineId = 6, Name = "JetBlue Airways", ImageName = "jetblue_airways.png" }
            );
        }
    }

}
