using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DSCMS.Web.Models
{
    //Grid level customization properties
    public class PagedData<T> where T : class
    {
        public IEnumerable<T> Data { get; set; }
        public string PageTitle { get; set; }
        public int NumberOfPages { get; set; }
        public int CurrentPage { get; set; }
        public string[] ColumnNames { get; set; }
        public string EditUrl { get; set; }
        public string DetailUrl { get; set; }
        public string DeleteUrl { get; set; }
        public string AddUrl { get; set; }
        public List<CustomURLoption> CustomActionUrl { get; set; }
        public bool IsEditable { get; set; }
        public int PageSize { get; set; }
        public string[] HeaderNames { get; set; }
        public string AssociatedMethod { get; set; }
        public bool showAction { get; set; }
        public bool showAddButton { get; set; }
        public bool showEditButton { get; set; }
        public bool showDeleteButton { get; set; }
        public bool showDetailButton { get; set; }
        public bool showSearch { get; set; }

        //constructer to initialize values
        public PagedData()
        {
            if (HttpContext.Current.Request.RequestContext.RouteData.DataTokens["area"] != null)
            {
                EditUrl = "/" + HttpContext.Current.Request.RequestContext.RouteData.DataTokens["area"].ToString();
                AddUrl = EditUrl;
                DetailUrl = EditUrl;
                DeleteUrl = EditUrl;
            }
            string Url = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString());
            if (Url != null)
            {
                EditUrl += "/" + Url + "/Edit";
                AddUrl += "/" + Url + "/Create";
                DetailUrl += "/" + Url + "/Detail";
                DeleteUrl += "/" + Url + "/Delete";
            }

            string[] collection = new string[typeof(T).GetProperties().Length];
            int i = 0;
            foreach (PropertyInfo p in typeof(T).GetProperties())
            {
                collection[i] = p.Name;
                i++;
            }
            ColumnNames = collection;
            IsEditable = true;
            CurrentPage = 1;
            AssociatedMethod = "Index";
            showAction = true;
            if (showAction)
            {
                showDetailButton = true;
                showEditButton = true;
                showDeleteButton = true;
            }
            else
            {
                showDetailButton = false;
                showEditButton = false;
                showDeleteButton = false;
            }
            showAddButton = true;
            PageTitle = Url;
            showSearch = true;
        }

        //for more customization use below over loaded versions, function can be called as given below
        //PagedData<vw_tblCity>.ReturnCustomizeData(data, PageSize, new string[] { "Id", "City_Name", "State_Name" }, new string[] { "id", "city", "state" }, null, null, null, null, null, null, null, null);
        //we can create more overloaded version as per requirments.

        public static PagedData<T> ReturnCustomizeData(PagedData<T> data, int PageSize, string[] columnNames, string[] headersName, string editUrl, bool? isEditable, string associatedMethod, bool? showAction, bool? showDetailButton, bool? showEditButton, bool? showDeleteButton, bool? showAddButton, bool? showSearch)
        {
            //This property will create serial no based on paging
            data.PageSize = PageSize;
            //By default all the properties of the class will be appear as column name, you can customize as given below
            data.ColumnNames = columnNames != null ? columnNames : data.ColumnNames;
            //By default database column name will be the header name, you can customize as given below
            data.HeaderNames = headersName != null ? headersName : data.HeaderNames;
            //By default edit url will be same controller and edit name, you can customize as given below
            data.EditUrl = editUrl != null ? editUrl : data.EditUrl;
            //By Default IsEditable is true you can change it to false in case edit not required as given below
            data.IsEditable = isEditable != null ? (bool)isEditable : data.IsEditable;
            //By default Associated method name (For paging) is "ListJson" we can pass if another required as given below
            data.AssociatedMethod = associatedMethod != null ? associatedMethod : data.AssociatedMethod;
            //last column will be action by default where detail, edit, delete link button will be display 
            //in case they are not required make it false as below
            data.showAction = showAction != null ? (bool)showAction : data.showAction;
            //by default details, edit, delete button will be display 
            //in case any of button not required make the property false as below
            data.showDetailButton = showDetailButton != null ? (bool)showDetailButton : data.showDetailButton;
            data.showEditButton = showEditButton != null ? (bool)showEditButton : data.showEditButton;
            data.showDeleteButton = showDeleteButton != null ? (bool)showDeleteButton : data.showDeleteButton;
            data.showAddButton = showAddButton != null ? (bool)showAddButton : data.showAddButton;
            data.showSearch = showSearch != null ? (bool)showSearch : data.showSearch;
            return data;
        }
        public static PagedData<T> ReturnCustomizeData(PagedData<T> data, int PageSize, bool showDetailButton, bool showEditButton, bool showDeleteButton, List<CustomURLoption> CustomActionUrl)
        {
            data.CustomActionUrl = CustomActionUrl != null ? CustomActionUrl : data.CustomActionUrl;
            data.showDetailButton = showDetailButton != null ? (bool)showDetailButton : data.showDetailButton;
            data.showEditButton = showEditButton != null ? (bool)showEditButton : data.showEditButton;
            data.showDeleteButton = showDeleteButton != null ? (bool)showDeleteButton : data.showDeleteButton;

            data.PageSize = PageSize;
            return data;
        }
        public static PagedData<T> ReturnCustomizeData(PagedData<T> data, int PageSize)
        {
            data.PageSize = PageSize;
            return data;
        }
        public static PagedData<T> ReturnCustomizeData(PagedData<T> data, int PageSize,bool? showAction)
        {
            data.showAction = showAction != null ? (bool)showAction : data.showAction;
            data.PageSize = PageSize;
            return data;
        }
        public static PagedData<T> ReturnCustomizeData(PagedData<T> data, int PageSize, string PageTitle)
        {
            data.PageSize = PageSize;
            data.PageTitle = PageTitle != null ? PageTitle : data.PageTitle;
            return data;
        }
        public static PagedData<T> ReturnCustomizeData(PagedData<T> data, int PageSize, string[] columnNames, string[] headersName)
        {
            data.PageSize = PageSize;
            data.ColumnNames = columnNames;
            data.HeaderNames = headersName;
            return data;
        }
        public static PagedData<T> ReturnCustomizeData(PagedData<T> data, int PageSize, string[] columnNames, string[] headersName, bool? showAction)
        {
            data.PageSize = PageSize;
            data.ColumnNames = columnNames;
            data.HeaderNames = headersName;
            data.showAction = showAction != null ? (bool)showAction : data.showAction;
            return data;
        }
        public static PagedData<T> ReturnCustomizeData(PagedData<T> data, int PageSize, string[] columnNames, string[] headersName, string associatedMethod)
        {
            data.PageSize = PageSize;
            data.ColumnNames = columnNames;
            data.HeaderNames = headersName;
            data.AssociatedMethod = associatedMethod != null ? associatedMethod : data.AssociatedMethod;
            return data;
        }
    }

    public class CustomURLoption
    {
        public string LinkName { get; set; }
        public string UrlName { get; set; }
        public string ClassOnButton { get; set; }
        public CustomURLoption()
        {
            ClassOnButton = "";
        }
    }
}