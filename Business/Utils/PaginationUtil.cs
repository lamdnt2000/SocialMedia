using System;
using System.Collections.Generic;
using System.Reflection;

namespace API.Utils
{
    public class PaginationUtil<T>
    {
        public List<string> FilterList(object classObj)
        {
            Type type = ((T)classObj).GetType();
            List<string> classNameList = new List<string>();
            foreach (PropertyInfo p in type.GetProperties())
            {
                if (!p.PropertyType.Name.Contains("ICollection"))
                {
                    classNameList.Add(p.Name);
                }
            }
            return classNameList;
        }

        /*
        public PaginationRequestDTO<T> ValidatePaginationParam(PaginationRequestDTO<T> pageRequest, string className)
        {
            T searchModel = pageRequest.SearchModel;
            int pageSize = pageRequest.PageSize;
            int pageIndex = pageRequest.PageIndex;
            string sortType = pageRequest.SortType;
            string sortBy = pageRequest.SortBy;
            List<string> sortByListAvail = new PaginationUtil<T>().FilterList(className);

            if (pageSize < 1)
            {
                pageSize = 10;
            }
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            if (sortBy == null || !sortByListAvail.Contains(sortBy))
            {
                //ko set thi mac dinh sort theo ID
                sortBy = sortByListAvail[0];
            }
            if (sortType == null || !sortType.Equals("dsc"))
            {
                sortType = "asc";
            }
            return new PaginationRequestDTO<T>(searchModel, pageIndex, pageSize, sortBy, sortType);
        }*/
    }
}
