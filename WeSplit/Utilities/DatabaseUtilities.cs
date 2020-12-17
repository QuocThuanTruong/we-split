using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeSplit.Utilities
{
    class DatabaseUtilities
    {
        private DatabaseUtilities() { }

        private static DatabaseUtilities _databaseInstance;
        private static WeSplitEntities _databaseWeSplit;
        private AppUtilities _appUtilities = AppUtilities.GetAppInstance();

        public static DatabaseUtilities GetDBInstance()
        {
            if (_databaseInstance == null)
            {
                _databaseInstance = new DatabaseUtilities();
                _databaseWeSplit = new WeSplitEntities();
            }
            else
            {
                //Do Nothing
            }

            return _databaseInstance;
        }

        public int GetMaxIDJourney()
        {
            int result = _databaseWeSplit
                .Database
                .SqlQuery<int>("Select Max(ID_Journey) from Journey")
                .Single();

            return result;
        }

        public int GetMaxIDSite()
        {
            int result = _databaseWeSplit
                .Database
                .SqlQuery<int>("Select Max(ID_Site) from Site")
                .Single();

            return result;
        }

        public int GetMaxIDMember()
        {
            int result = _databaseWeSplit
                .Database
                .SqlQuery<int>("Select Max(ID_Member) from JourneyAttendance")
                .Single();

            return result;
        }

        public int GetMaxIDExpenses()
        {
            int result = _databaseWeSplit
                .Database
                .SqlQuery<int>("Select Max(ID_Expenses) from Expenses")
                .Single();

            return result;
        }

        public Province GetProvinceByName(string Province_Name)
        {
            Province result = _databaseWeSplit
               .Database
               .SqlQuery<Province>($"Select* from Province where Province_Name = N'{Province_Name}'")
               .Single();

            return result;
        }

        public List<Site> GetSiteForBindingInHomePageView(int status)
        {
            List<Site> result = new List<Site>();

            var journeysByStatus = _databaseWeSplit
                .Database
                .SqlQuery<Journey>($"Select * from Journey where Status = {status} ORDER BY ID_Journey")
                .ToList();

            foreach (var journey in journeysByStatus)
            {
                var site = _databaseWeSplit
                    .Database
                    .SqlQuery<Site>($"Select * from Site where ID_Site = {journey.ID_Site}")
                    .Single();

                site.Distance = journey.Distance ?? 0.0;
                site.ID_Journey = journey.ID_Journey;

                result.Add(site);
            }

            return result;
        } 

        public Journey GetcurrentJourney()
        {
            Journey result = _databaseWeSplit
                .Database
                .SqlQuery<Journey>("Select * from Journey where Status = 0")
                .FirstOrDefault();

            if (result != null)
            {
                var site = this.GetSiteForBindingInHomePageView(0);

                if (site.Count == 1)
                {
                    result.Site_Name = site[0].Site_Name;
                    result.Site_Avatar = site[0].Site_Link_Avt;

                    var journeyProgess = _databaseWeSplit
                        .Database
                        .SqlQuery<double>($"select [dbo].[CalcJourneyProgress]({result.ID_Journey})")
                        .Single();

                    result.Journey_Progress = journeyProgess;
                }
            }

            return result;
        }

        public List<Journey> GetListJourney()
        {
            List<Journey> result = _databaseWeSplit
                .Database
                .SqlQuery<Journey>("Select * from Journey")
                .ToList();

            if (result.Count > 0)
            {
                for (int i = 0; i < result.Count; ++i)
                {
                    Site site = _databaseWeSplit
                        .Database
                        .SqlQuery<Site>($"Select * from Site where ID_Site = {result[i].ID_Site}")
                        .FirstOrDefault();

                    result[i].Site_Name = site.Site_Name;
                    result[i].Site_Avatar = site.Site_Link_Avt;

                    result[i].Total_Day = (int)(result[i].EndDate - result[i].StartDate).Value.TotalDays;

                    result[i].Total_Day_For_Binding = $"{result[i].Total_Day} ngày";
                    result[i].Total_Distance_For_Binding = $"{result[i].Distance} km lộ trình";
                    result[i].Total_Member_For_Binding = $"{result[i].Total_Member} thành viên";

                }
            }

            return result;
        }

        public List<Journey> GetListJourneyByStatus(int status)
        {
            List<Journey> result = _databaseWeSplit
                .Database
                .SqlQuery<Journey>($"Select * from Journey where Status = {status}")
                .ToList();

            if (result.Count > 0)
            {
                for (int i = 0; i < result.Count; ++i)
                {
                    Site site = _databaseWeSplit
                        .Database
                        .SqlQuery<Site>($"Select * from Site where ID_Site = {result[i].ID_Site}")
                        .FirstOrDefault();

                    result[i].Site_Name = site.Site_Name;
                    result[i].Site_Avatar = site.Site_Link_Avt;

                    result[i].Total_Day = (int)(result[i].EndDate - result[i].StartDate).Value.TotalDays;

                    result[i].Total_Day_For_Binding = $"{result[i].Total_Day} ngày";
                    result[i].Total_Distance_For_Binding = $"{result[i].Distance} km lộ trình";
                    result[i].Total_Member_For_Binding = $"{result[i].Total_Member} thành viên";

                }
            }

            return result;
        }

        public Journey GetJourneyByID(int ID_Journey)
        {
            Journey result = _databaseWeSplit
                .Database
                .SqlQuery<Journey>($"Select * from Journey where ID_Journey = {ID_Journey}")
                .FirstOrDefault();

            if (result != null)
            {
                //Site
                Site site = _databaseWeSplit
                    .Database
                    .SqlQuery<Site>($"Select * from Site where ID_Site = {result.ID_Site}")
                    .SingleOrDefault();

                result.Site_Name = site.Site_Name;
                result.Site_Avatar = site.Site_Link_Avt;
                
                //progress slider
                var journeyProgess = _databaseWeSplit
                        .Database
                        .SqlQuery<double>($"select [dbo].[CalcJourneyProgress]({result.ID_Journey})")
                        .Single();

                result.Journey_Progress = journeyProgess;

                //Date
                result.Start_Date_For_Binding = result.StartDate.Value.ToShortDateString();
                result.End_Date_For_Binding = result.EndDate.Value.ToShortDateString();

                //Route
                List<Route> routes = _databaseWeSplit
                    .Database
                    .SqlQuery<Route>($"Select * from Route where ID_Journey = {result.ID_Journey}")
                    .ToList();

                result.Route_For_Binding = routes;

                //Member
                List<JourneyAttendance> members = _databaseWeSplit
                    .Database
                    .SqlQuery<JourneyAttendance>($"Select * from JourneyAttendance where ID_Journey = {result.ID_Journey}")
                    .ToList();

                for (int i = 0; i < members.Count; ++i)
                {
                    members[i].Money_For_Binding = _appUtilities.GetMoneyForBinding(decimal.ToInt32(members[i].Receivables_Money ?? 0));
                }

                result.JourneyAttendances = members;

                //Images
                List<JourneyImage> images = _databaseWeSplit
                    .Database
                    .SqlQuery<JourneyImage>($"Select * from JourneyImage where ID_Journey = {result.ID_Journey}")
                    .ToList();

                result.Images_For_Binding = images;

                //Devide Money
                List<DevideMoney_Result> devideMoney = _databaseWeSplit.DevideMoney(result.ID_Journey).ToList();

                for (int i = 0; i < devideMoney.Count; ++i)
                {
                    devideMoney[i].Remain_For_Binding = _appUtilities.GetMoneyForBinding(decimal.ToInt32(devideMoney[i].Remain ?? 0));

                    if (devideMoney[i].ID_Lender != null)
                    {
                        devideMoney[i].Advance_Money_Lender = $"{_appUtilities.GetMoneyForBinding(decimal.ToInt32(devideMoney[i].Advance_Money ?? 0))} cho T.Viên ({devideMoney[i].ID_Lender})";
                    }
                }

                result.Devide_Money_For_Binding = devideMoney;

                //Statistical
                result.Total_Receivables = _databaseInstance.GetTotalReceivable(ID_Journey);
                result.Total_Receivables_For_Binding = "Tổng thu: " + _appUtilities.GetMoneyForBinding(decimal.ToInt32(result.Total_Receivables));

                result.Total_Expenses = _databaseInstance.GetTotalExpenses(ID_Journey);
                result.Total_Expenses_For_Binding = "Tổng chi: " + _appUtilities.GetMoneyForBinding(decimal.ToInt32(result.Total_Expenses));

                result.Remain = result.Total_Receivables - result.Total_Expenses;
                result.Remain_For_Binding = "Số dư: " + _appUtilities.GetMoneyForBinding(decimal.ToInt32(result.Remain)); 

                //Expenses
                result.Expenses = _databaseWeSplit
                    .Database
                    .SqlQuery<Expens>($"Select * from Expenses where ID_Journey = {ID_Journey}")
                    .ToList();

                result.Expens_For_Binding = result.Expenses.ToList();

                for (int i = 0; i < result.Expens_For_Binding.Count; ++i)
                {
                    result.Expens_For_Binding[i].Expenses_For_Binding = _appUtilities.GetMoneyForBinding(decimal.ToInt32(result.Expens_For_Binding[i].Expenses_Money ?? 0));
                }

                //Advance
                result.Advances = _databaseWeSplit
                    .Database
                    .SqlQuery<Advance>($"Select * from Advance where ID_Journey = {ID_Journey}")
                    .ToList();

                result.Advances_For_Binding = result.Advances.ToList();
                for (int i = 0; i < result.Advances_For_Binding.Count; ++i)
                {
                    result.Advances_For_Binding[i].Borrower_Name = _databaseWeSplit
                        .Database
                        .SqlQuery<string>($"Select Member_Name from JourneyAttendance where ID_Member = {result.Advances_For_Binding[i].ID_Borrower}")
                        .Single();

                    result.Advances_For_Binding[i].Lender_Name = _databaseWeSplit
                       .Database
                       .SqlQuery<string>($"Select Member_Name from JourneyAttendance where ID_Member = {result.Advances_For_Binding[i].ID_Lender}")
                       .Single();

                    result.Advances_For_Binding[i].Money_For_Binding = _appUtilities.GetMoneyForBinding(decimal.ToInt32(result.Advances_For_Binding[i].Advance_Money ?? 0));
                }

            }

            return result;
        }

        public List<Site> GetListSite()
        {
            List<Site> result = _databaseWeSplit
                .Database
                .SqlQuery<Site>("Select * from Site")
                .ToList();

            for (int i = 0; i < result.Count; ++i)
            {
                Province province = _databaseWeSplit
                    .Database
                    .SqlQuery<Province>($"Select * from Province where ID_Province = {result[i].ID_Province}")
                    .Single();

                result[i].Province_Name = province.Province_Name;
            }

            return result;
        }

        public List<Province> GetListProvince()
        {
            List<Province> result = _databaseWeSplit
                .Database
                .SqlQuery<Province>("Select * from Province")
                .ToList();

            return result;
        }

        public List<Site> GetListSiteByProvince(int ID_Province)
        {
            List<Site> result = _databaseWeSplit
                .Database
                .SqlQuery<Site>($"Select * from Site where ID_Province = {ID_Province}")
                .ToList();

            return result;
        }

        public List<Route> GetListRouteOfJourney(int ID_Journey)
        {
            List<Route> result = _databaseWeSplit
               .Database
               .SqlQuery<Route>($"Select * from Route where ID_Journey = {ID_Journey}")
               .ToList();

            return result;
        }

        public Site GetSiteByID(int ID_Site)
        {
            Site result = _databaseWeSplit
                .Database
                .SqlQuery<Site>($"Select * from Site where ID_Site = {ID_Site}")
                .Single();

            return result;
        }

        public Province GetProvinceByID(int ID_Province)
        {
            Province result = _databaseWeSplit
                .Database
                .SqlQuery<Province>($"Select * from Province where ID_Province = {ID_Province}")
                .Single();

            return result;
        }

        public int AddNewSite(int idSite, int idProvince, string siteName, string siteDescription, string siteLinkAvt, string siteAddress)
        {
            return _databaseWeSplit.AddSite(idSite, idProvince, siteName, siteDescription, siteLinkAvt, siteAddress);
        }

        public int AddNewJourney(Nullable<int> idJourney, string journeyName, Nullable<int> idSite, string startPlace, string startProvince, Nullable<int> status, Nullable<System.DateTime> startDate, Nullable<System.DateTime> endDate, Nullable<double> distance) {
            return _databaseWeSplit.AddJourney(idJourney, journeyName, idSite, startPlace, startProvince, status, startDate, endDate, distance);
        }

        public int AddExpense(Nullable<int> idExpenses, Nullable<int> idJourney, Nullable<decimal> expense, string des)
        {
            return _databaseWeSplit.AddExpense(idExpenses, idJourney, expense, des);
        }

        public int AddRoute(Nullable<int> idJourney, Nullable<int> ordinalNumber, string place, string province, string routeDescription, Nullable<int> routeStatus)
        {
            return _databaseWeSplit.AddRoute(idJourney, ordinalNumber, place, province, routeDescription, routeStatus);
        }

        public int AddJourneyAttendance(Nullable<int> idMember, Nullable<int> idJourney, string memberName, string phoneNumber, Nullable<decimal> receivable, string role)
        {
            return _databaseWeSplit.AddJourneyAttendance(idMember, idJourney, memberName, phoneNumber, receivable, role);
        }

        public decimal GetTotalReceivable(int ID_Journey)
        {
            decimal result = _databaseWeSplit
                 .Database
                 .SqlQuery<decimal>($"select [dbo].[CalcSumReceivablesByIDJourney]({ID_Journey})")
                 .Single();

            return result;
        }

        public decimal GetTotalExpenses(int ID_Journey)
        {
            decimal result = _databaseWeSplit
                 .Database
                 .SqlQuery<decimal>($"select [dbo].[CalcSumExpensesByIDJourney]({ID_Journey})")
                 .Single();

            return result;
        }
    }
}
