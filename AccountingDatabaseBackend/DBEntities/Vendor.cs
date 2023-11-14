using Bogus;
using Bogus.Extensions.UnitedStates;
using SQLiteUtils.Schema;
using SQLiteUtils.Serialization;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AccountingDatabaseBackend.DBEntities
{
    public class Vendor : ISQLUpdateable
    {
        public int vendor_id;
        public string vendor_name;
        public string phone_number;
        public string ein;
        public int address_id;

        public Vendor() 
        { 
            vendor_name = string.Empty;
            phone_number = string.Empty;
            ein = string.Empty;
        }

        public Vendor(int vendor_id, string vendor_name, string phone_number, string ein, int address_id)
        {
            this.vendor_id = vendor_id;
            this.vendor_name = vendor_name;
            this.phone_number = phone_number;
            this.ein = ein;
            this.address_id = address_id;
        }

        public static List<(Vendor, Address)> GenerateRandom(int count)
        {
            var addresses = Address.CreateRandom(count);

            var vendors = new Faker<Vendor>()
                .StrictMode(true)
                .RuleFor(v => v.vendor_id, f => f.IndexGlobal)
                .RuleFor(v => v.vendor_name, f => f.Company.CompanyName())
                .RuleFor(v => v.phone_number, f => f.Person.Phone)
                .RuleFor(v => v.ein, f => f.Company.Ein())
                .RuleFor(v => v.address_id, f => addresses[f.IndexFaker].id);

            return vendors.Generate(count).Zip(addresses).ToList();
        }

        public static SchemaEntry GetEntry(string name)
        {
            return new SchemaEntry(name, new SchemaFeild[]
            {
                new SchemaFeild.Basic(nameof(vendor_id), "INT", true),
                new SchemaFeild.Basic(nameof(vendor_name), "VARCHAR(20)"),
                new SchemaFeild.Basic(nameof(phone_number), "VARCHAR(10)"),
                new SchemaFeild.Basic(nameof(ein), "STRING"),
                new SchemaFeild.Basic(nameof(address_id), "INT"),

                new SchemaFeild.PrimaryKey(nameof(vendor_id)),
                new SchemaFeild.ForeignKey(nameof(address_id), nameof(Address.id), DBTableNames.ADDRESS_TABLE_NAME)
            });
        }

        public (string, string) GetId()
        {
            return (nameof(vendor_id), vendor_id.ToString());
        }

        public void OnDeserialize(ref SQLiteDataReader reader)
        {
            vendor_id = reader.GetInt32(0);
            vendor_name = reader.GetString(1);
            phone_number = reader.GetString(2);
            ein = reader.GetString(3);
            address_id = reader.GetInt32(4);
        }

        public string[] OnSerialize()
        {
            return new string[]
            {
                vendor_id.ToString(),
                $"'{vendor_name}'",
                $"'{phone_number}'",
                $"'{ein}'",
                address_id.ToString(),
            };
        }
    }
}
