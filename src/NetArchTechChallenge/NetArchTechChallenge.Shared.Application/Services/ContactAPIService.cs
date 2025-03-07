using NetArchTechChallenge.Shared.Application.DTOs;
using ApplicationException = NetArchTechChallenge.Shared.Application.Exceptions.ApplicationException;
using System.Text.RegularExpressions;
using NetArchTechChallenge.Shared.Domain.Events;
using NetArchTechChallenge.Shared.Application.Messaging;

namespace NetArchTechChallenge.Shared.Application.Services
{
    public class ContactAPIService
    {
        private IMessageService _messageBus;

        public ContactAPIService(IMessageService messageBus)
        {
            _messageBus = messageBus;
        }

        public void Create(ContactDto dto)
        {
            EnsureValidation(dto);

            Console.WriteLine("Sending to Create_Queue");

            var @event = new ContactCreatedEvent(dto.Name, dto.PhoneDDD, dto.PhoneNumber, dto.EmailAddress);
            _messageBus.Publish(@event);
            //Send to Create Queue
        }

        public async void Update(ContactDto dto)
        {
            EnsureValidation(dto);

            Console.WriteLine("Sending to Update_Queue");
            var @event = new ContactUpdatedEvent(dto.Id, dto.Name, dto.PhoneDDD, dto.PhoneNumber, dto.EmailAddress);
            _messageBus.Publish(@event);
        }

        public void Delete(Guid id)
        {
            Console.WriteLine("Sending to Delete_Queue");
            var @event = new ContactDeletedEvent(id);
            _messageBus.Publish(@event);
            //Send to Delete Queue
        }

        private void EnsureValidation(ContactDto dto)
        {
            var errorList = new List<string>();

            var nameIsEmpty = string.IsNullOrWhiteSpace(dto.Name);
            if (nameIsEmpty)
                errorList.Add("Name shouldn't be empty!");

            var phoneDDDIsInvalid = !ValidatePhoneDDD(dto.PhoneDDD);
            if (phoneDDDIsInvalid)
                errorList.Add("Phone DDD is invalid!");

            var phoneNumberIsInvalid = !dto.PhoneNumber.All(char.IsNumber);
            if (phoneNumberIsInvalid)
                errorList.Add("Phone Number should have only numbers!");

            phoneNumberIsInvalid = !phoneNumberIsInvalid && !ValidatePhoneNumber(dto.PhoneNumber);
            if (phoneNumberIsInvalid)
                errorList.Add("Phone Number is invalid!");

            var emailIsInvalid = !ValidateEmailAddress(dto.EmailAddress);
            if (emailIsInvalid)
                errorList.Add("Email Address is invalid!");

            //var nameAlreadyInUse = !nameIsEmpty && Repository.ContactNameAlreadyExists(vo.Name, vo.Id);
            //if (nameAlreadyInUse)
            //    errorList.Add("Name already in use!");

            //var phoneAlreadyInUse = !phoneDDDIsInvalid && !phoneNumberIsInvalid && Repository.ContactPhoneAlreadyExists(vo.PhoneDDD, vo.PhoneNumber, vo.Id);
            //if (phoneAlreadyInUse)
            //    errorList.Add("Phone already in use!");

            //var emailAlreadyInUse = !emailIsInvalid && Repository.ContactEmailAlreadyExists(vo.EmailAddress, vo.Id);
            //if (emailAlreadyInUse)
            //    errorList.Add("Email already in use!");

            if (errorList.Count > 0)
                throw new ApplicationException(string.Join("\n", errorList));
        }

        private static bool ValidatePhoneDDD(string phoneDDD)
        {
            var validDDDs = new List<string>()
            {
                "11", "12", "13", "14", "15", "16", "17", "18", "19",
                "21", "22", "24", "27", "28",
                "31", "32", "33", "34", "35", "37", "38",
                "41", "42", "43", "44", "45", "46", "47", "48", "49",
                "51", "53", "54", "55",
                "61", "62", "64", "63",
                "65", "66", "67",
                "68", "69",
                "71", "73", "74", "75", "77", "79",
                "81", "82", "83", "84", "85", "86", "87", "88", "89",
                "91", "92", "93", "94", "95", "96", "97", "98", "99"
            };

            return validDDDs.Contains(phoneDDD);
        }

        private static bool ValidatePhoneNumber(string phoneNumber)
        {
            string mobilePhonePattern = @"^9\d{8}$";
            string fixedPhonePattern = @"^[2-5]\d{7}$";

            var isMobileValid = Regex.IsMatch(phoneNumber, mobilePhonePattern, RegexOptions.None, TimeSpan.FromMilliseconds(500));
            var isFixedValid = Regex.IsMatch(phoneNumber, fixedPhonePattern, RegexOptions.None, TimeSpan.FromMilliseconds(500));

            return isMobileValid || isFixedValid;
        }

        private static bool ValidateEmailAddress(string emailAddress)
        {
            string emailPattern = @"^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,}$";

            var isEmailValid = Regex.IsMatch(emailAddress, emailPattern, RegexOptions.None, TimeSpan.FromMilliseconds(500));
            return isEmailValid;
        }
    }
}
