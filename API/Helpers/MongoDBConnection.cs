using API.Constants;
using APP.Entity;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace APP.Helpers
{
    public static class MongoDBConnection
    {
        public static dynamic GetConnection()
        {
            List<Domain> allDocument = new List<Domain>();
            var records = Convert.ToInt32(Environment.GetEnvironmentVariable(InfraConstants.EnviromentVarName.RECORDS));

            if (Environment.GetEnvironmentVariable(InfraConstants.EnviromentVarName.FIND_BY_FILE).ToUpper() == "YES")
            {
                //// jsonDomain.json
                var currentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Helpers\\File\\jsonDomain.json";
                string json = System.IO.File.ReadAllText(currentDirectory);
                allDocument = BsonSerializer.Deserialize<List<Domain>>(json);
            }
            else
            {
                var mongoclient = new MongoClient(Environment.GetEnvironmentVariable(InfraConstants.EnviromentVarName.DB_MONGO_CONNSTRING));
                var database = mongoclient.GetDatabase(Environment.GetEnvironmentVariable(InfraConstants.EnviromentVarName.DATABASE_NAME));
                var collection = database.GetCollection<Domain>(Environment.GetEnvironmentVariable(InfraConstants.EnviromentVarName.DATABASE_COLLECTION));
                allDocument = collection.AsQueryable().ToList();
            }

            var filters = allDocument.Distinct(new ItemFilterComparer()).ToList();
            List<Domain> clonedList = AddCloneList(filters, 0);
            while (clonedList.Count < records)
            {
                clonedList.AddRange(AddCloneList(filters, clonedList.Count));
            }

            var cloneListResponse = new List<Domain>(clonedList.GetRange(0, records));
            var res = new List<Domain>();
            List<Domain> sorted = GetSorted(cloneListResponse);

            foreach (var u1 in sorted)
            {
                List<Domain> filterbyCompany = sorted.Where(x => x.Company.Trim() == u1.Company.Trim()).ToList();
                if (filterbyCompany != null && filterbyCompany.Count > 1)
                    GetDuplitacteValue(filterbyCompany);
                res.Add(u1);
            }

            dynamic objResponse = new List<System.Dynamic.ExpandoObject>();
            foreach (var item in res)
            {
                objResponse.Add(ConvertToResponse(item));
            }

            return objResponse;
        }

        private static List<Domain> AddCloneList(List<Domain> output, int index)
        {
            List<Domain> clonedList = new List<Domain>();
            foreach (var a in output)
            {
                index = index + 1;
                clonedList.Add(new Domain()
                {
                    Index = index,
                    Company = a.Company,
                    CategoryID = a.CategoryID,
                    Image_List = a.Image_List,
                    AccountID = a.AccountID,
                    AcctId = a.AcctId,
                    Active = a.Active,
                    Address1 = a.Address1,
                    CategoryName = a.CategoryName,
                    City = a.City,
                    Company_SortBy = a.Company_SortBy,
                    ContainDeals = a.ContainDeals,
                    DateCreated = a.DateCreated,
                    Description = a.Description,
                    Email = a.Email,
                    Id = a.Id,
                    LastUpdate = a.LastUpdate,
                    ListingID = a.ListingID,
                    Phone = a.Phone,
                    SortKey = a.SortKey,
                    State = a.State,
                    SubCategoryID = a.SubCategoryID,
                    SubCategoryName = a.SubCategoryName,
                    Website = a.Website,
                    Zip = a.Zip
                });
            }

            return clonedList;
        }

        private static List<Domain> GetSorted(List<Domain> cloneListResponse)
        {
            var sorted = cloneListResponse.OrderBy(x => x.ListingID)
                                   .ThenBy(x => x.Company)
                                   .ThenBy(x => x.CategoryID)
                                   .ThenBy(x => x.Image_List)
                                   .ToList();

            sorted.Sort((x, y) =>
            {
                int ret = string.Compare(x.Company, y.Company);
                return ret != 0 ? ret : x.Company.CompareTo(y.Company);
            });

            return sorted;
        }

        private static void GetDuplitacteValue(List<Domain> filterbyCompany)
        {
            for (int i = 0; i < filterbyCompany.Count; i++)
            {
                var cSortBy = filterbyCompany[i].Company_SortBy;
                filterbyCompany[i].Company = cSortBy + " " + i;
            }
        }

        private static System.Dynamic.ExpandoObject ConvertToResponse(Domain item)
        {
            dynamic dynamicResponse = new System.Dynamic.ExpandoObject();
            dynamicResponse.ListingID = item.ListingID;
            dynamicResponse.ImageList = !string.IsNullOrWhiteSpace(item.Image_List) ? item.Image_List.Split('|').FirstOrDefault() : null;
            dynamicResponse.Company = item.Company;
            dynamicResponse.CategoryID = item.CategoryID;
            ////dynamicResponse.Index = item.Index;
            return dynamicResponse;
        }
    }
}
