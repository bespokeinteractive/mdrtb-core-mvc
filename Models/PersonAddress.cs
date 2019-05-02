using System;
using EtbSomalia.Extensions;
using EtbSomalia.Services;
using Microsoft.AspNetCore.Http;

namespace EtbSomalia.Models
{
    public class PersonAddress
    {
        public Int64 Id { get; set; }
        public Person Person { get; set; }
        public Boolean Default { get; set; }
        public String Mobile { get; set; }
        public String Telephone { get; set; }
        public String Address { get; set; }
        public String PostalCode { get; set; }
        public String Village { get; set; }
        public String State { get; set; }
        public String County { get; set; }

        public PersonAddress()
        {
            Id = 0;
            Default = false;
            Mobile = "";
            Telephone = "";
            Address = "";
            PostalCode = "";
            Village = "";
            State = "";
            County = "";
        }

        public PersonAddress Save(HttpContext Context){
            return new PatientService(Context).SavePersonAddress(this);
        }

        public PersonAddress Update()
        {
            return this;
        }
    }
}
