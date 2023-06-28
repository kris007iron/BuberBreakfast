using BuberBreakfast.ServicesErrors;
using ErrorOr;
using System;
using System.Collections.Generic;

namespace BuberBreakfast.Models
{
    public class Breakfast
    {
        public const int MinNameLength = 3;
        public const int MaxNameLength = 50;

        public const int MinDescriptionLength = 50;
        public const int MaxDescriptionLength = 150;

        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public DateTime StartDateTime { get; }
        public DateTime EndDateTime { get; }
        public DateTime LastModifiedDateTime { get; }
        public List<string> Savory { get; }
        public List<string> Sweet { get; }

        private Breakfast(Guid id, string name, string description, DateTime startDateTime, DateTime endDateTime, DateTime lastModifiedDateTime, List<string> savory, List<string> sweet)
        {
            Id = id;
            Name = name;
            Description = description;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            LastModifiedDateTime = lastModifiedDateTime;
            Savory = savory;
            Sweet = sweet;
        }

        // Factory method to create a Breakfast object from provided parameters
        public static ErrorOr<Breakfast> Create(
            string name,
            string description,
            DateTime startDateTime,
            DateTime endDateTime,
            List<string> savory,
            List<string> sweet,
            Guid? id = null)
        {
            List<Error> errors = new List<Error>();

            // Validate the name length
            if (name.Length < MinNameLength || name.Length > MaxNameLength)
            {
                errors.Add(Errors.Breakfast.InvalidName);
            }

            // Validate the description length
            if (description.Length < MinDescriptionLength || description.Length > MaxDescriptionLength)
            {
                errors.Add(Errors.Breakfast.InvalidDescription);
            }

            // If there are any errors, return the errors
            if (errors.Count > 0)
            {
                return errors;
            }

            // Create a new Breakfast object with the provided parameters
            return new Breakfast(
                id ?? Guid.NewGuid(), // Generate a new ID if not provided
                name,
                description,
                startDateTime,
                endDateTime,
                DateTime.UtcNow, // Set the LastModifiedDateTime to the current UTC time
                savory,
                sweet);
        }
    }
}
