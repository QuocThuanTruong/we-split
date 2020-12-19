using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections;
using System.ComponentModel;
using System.Windows.Controls;
using System.Diagnostics;

namespace WeSplit.Utilities 
{
    class DatabaseUtilities : Page
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

                    result[i].Total_Member = _databaseWeSplit
                        .Database
                        .SqlQuery<int>($"Select Count(*) as Total_Member from JourneyAttendance where ID_Journey = {result[i].ID_Journey}")
                        .Single();
                    result[i].Total_Member_For_Binding = $"{result[i].Total_Member} thành viên";

                    switch (result[i].Status)
                    {
                        case -1:
                            result[i].Icon_Status_Source = FindResource("BadgeDone").ToString();
                            break;
                        case 0:
                            result[i].Icon_Status_Source = FindResource("Badgecurrent").ToString();
                            break;
                        case 1:
                            result[i].Icon_Status_Source = FindResource("BadgePlan").ToString();
                            break;
                    }
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

                    result[i].Total_Member = _databaseWeSplit
                        .Database
                        .SqlQuery<int>($"Select Count(*) as Total_Member from JourneyAttendance where ID_Journey = {result[i].ID_Journey}")
                        .Single();

                    result[i].Total_Member_For_Binding = $"{result[i].Total_Member} thành viên";

                    switch (result[i].Status)
                    {
                        case -1:
                            result[i].Icon_Status_Source = FindResource("BadgeDone").ToString();
                            break;
                        case 0:
                            result[i].Icon_Status_Source = FindResource("Badgecurrent").ToString();
                            break;
                        case 1:
                            result[i].Icon_Status_Source = FindResource("BadgePlan").ToString();
                            break;
                    }

                }
            }

            return result;
        }

        public Journey GetJourneyForBindingInListView(Journey journey)
        {
            Journey result = journey;
            Site site = _databaseWeSplit
                        .Database
                        .SqlQuery<Site>($"Select * from Site where ID_Site = {result.ID_Site}")
                        .FirstOrDefault();

            result.Site_Name = site.Site_Name;
            result.Site_Avatar = site.Site_Link_Avt;

            result.Total_Day = (int)(result.EndDate - result.StartDate).Value.TotalDays;

            result.Total_Day_For_Binding = $"{result.Total_Day} ngày";
            result.Total_Distance_For_Binding = $"{result.Distance} km lộ trình";

            result.Total_Member = _databaseWeSplit
                .Database
                .SqlQuery<int>($"Select Count(*) as Total_Member from JourneyAttendance where ID_Journey = {result.ID_Journey}")
                .Single();

            result.Total_Member_For_Binding = $"{result.Total_Member} thành viên";

            switch (result.Status)
            {
                case -1:
                    result.Icon_Status_Source = FindResource("BadgeDone").ToString();
                    break;
                case 0:
                    result.Icon_Status_Source = FindResource("Badgecurrent").ToString();
                    break;
                case 1:
                    result.Icon_Status_Source = FindResource("BadgePlan").ToString();
                    break;
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
                    .SqlQuery<Route>($"Select * from Route where ID_Journey = {result.ID_Journey} and Is_Active = 1")
                    .ToList();

                for (int i = 0; i < routes.Count; ++i)
                {
                    routes[i].Route_Index = i;
                }

                result.Route_For_Binding = routes;

                //Member
                List<JourneyAttendance> members = _databaseWeSplit
                    .Database 
                    .SqlQuery<JourneyAttendance>($"Select * from JourneyAttendance where ID_Journey = {result.ID_Journey} and Is_Active = 1")
                    .ToList();

                for (int i = 0; i < members.Count; ++i)
                {
                    members[i].Money_For_Binding = _appUtilities.GetMoneyForBinding(decimal.ToInt32(members[i].Receivables_Money ?? 0));
                    members[i].Member_Index = i + 1;
                }

                result.JourneyAttendances = members;

                //Images
                List<JourneyImage> images = _databaseWeSplit
                    .Database
                    .SqlQuery<JourneyImage>($"Select * from JourneyImage where ID_Journey = {result.ID_Journey} and Is_Active = 1")
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
                    .SqlQuery<Expens>($"Select * from Expenses where ID_Journey = {ID_Journey} and Is_Active = 1")
                    .ToList();

                result.Expens_For_Binding = result.Expenses.ToList();

                for (int i = 0; i < result.Expens_For_Binding.Count; ++i)
                {
                    result.Expens_For_Binding[i].Expense_Index = i + 1;
                }

                for (int i = 0; i < result.Expens_For_Binding.Count; ++i)
                {
                    result.Expens_For_Binding[i].Expenses_For_Binding = _appUtilities.GetMoneyForBinding(decimal.ToInt32(result.Expens_For_Binding[i].Expenses_Money ?? 0));
                }

                //Advance
                result.Advances = _databaseWeSplit
                    .Database
                    .SqlQuery<Advance>($"Select * from Advance where ID_Journey = {ID_Journey} and Is_Active = 1")
                    .ToList();


                result.Advances_For_Binding = result.Advances.ToList();
                for (int i = 0; i < result.Advances_For_Binding.Count; ++i)
                {
                    result.Advances_For_Binding[i].Advance_Index = i;

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

        public string GetMemberNameByID(int ID_Member)
        {
            string result = _databaseWeSplit
                .Database
                .SqlQuery<string>($"Select Member_Name from JourneyAttendance where ID_Member = {ID_Member}")
                .Single();

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
            decimal result = 0;

            try
            {
                result = _databaseWeSplit
                .Database
                .SqlQuery<decimal>($"select [dbo].[CalcSumExpensesByIDJourney]({ID_Journey})")
                .Single();
            } catch (Exception e)
            {
                //throw e;
            }
            
            return result;
        }

        public (List<Journey>, int) GetJourneySearchResult(string search_text, string condition)
        {
            (List<Journey> journeysSearchResultList, int totalJourneySearch) result;
            List<Journey> journeysSearchResultList = new List<Journey>();
            int totalJourneySearch = 0;

            try
            {
                string[] OPERATOR = { "and", "or", "and not" };

                //Chuẩn hóa hết mấy cái khoảng trắng thừa.
                //đưa hết mấy cái operator về and, or, and not. không để AND....
                search_text = GetStandardString(search_text);

                //Lấy hết mấy cái "abcd" vô cái stack để hồi pop ra.
                Stack<string> keywords = GetListKeyWords(search_text);

                //:V lấy hết and, or, and not đẩy vô queue.
                Queue<int> operators = GetListOperator(search_text);

                //Nếu số ngoặc kép " là lẻ thì để khỏi crash. thay " thành # :). Best sửa.
                //Tại sao lại là keywords.Count. Vì lúc lấy cái keywords ra thì chỉ có kết quả khi số " là chẵn. còn nếu " lẻ thì keywords sẽ k có phần tử nào.
                if (keywords.Count == 0 || (keywords.Count > 0 && operators.Count == 0))
                {
                    search_text = search_text.Replace("\"", "#");

                    string query = "";

                    if (condition.Length > 0)
                    {
                        query = $"SELECT count(distinct(ID_Journey)) FROM SearchJourney(N'{search_text}') WHERE {condition}";
                    }
                    else
                    {
                        query = $"SELECT count(distinct(ID_Journey)) FROM SearchJourney(N'{search_text}')";
                    }

                    totalJourneySearch = _databaseWeSplit
                        .Database
                        .SqlQuery<int>(query)
                        .Single();

                    if (totalJourneySearch > 0)
                    {
                        query = query.Replace("count(distinct(ID_Journey))", "distinct(ID_Journey)");
                        //query += $" ORDER BY [{sortedBy.column}] {sortedBy.type} OFFSET {currentPage - 1}*{totalJourneyPerPage} ROWS FETCH NEXT {totalJourneyPerPage} ROWS ONLY";

                        var ID_journeysSearchResultList = _databaseWeSplit
                            .Database
                            .SqlQuery<int>(query)
                            .ToList();

                        foreach (var ID_Journey in ID_journeysSearchResultList)
                        {
                            Journey journey = _databaseWeSplit
                                .Database
                                .SqlQuery<Journey>($"Select * from Journey where ID_Journey = {ID_Journey}")
                                .Single();

                            journeysSearchResultList.Add(journey);
                        }

                        for (int i = 0; i < journeysSearchResultList.Count; ++i)
                        {
                            journeysSearchResultList[i] = GetJourneyForBindingInListView(journeysSearchResultList[i]);
                        }
                    }

                    //Thay xong rồi thì tìm bình thường thôi
                    //var JourneysSearchResult = SearchJourney(search_text).OrderByDescending(r => r.RANK);

                    //foreach (var JourneySearchResult in JourneysSearchResult)
                    //{
                    //    var Journey = from r in Journeys
                    //                 where r.ID_Journey == JourneySearchResult.ID_Journey
                    //                 select r;
                    //    JourneysSearchResultList.Add(Journey.FirstOrDefault());
                    //}

                }
                //Điều kiện này tại có nhiều khi nguòi ta chỉ nhập "ab" mà không có toán tử and, or, and not á. thì cũng tìm bình thường á.
                else if (operators.Count > 0)
                {
                    /*
                        Những cái mà dùng HashSet là để khỏi loại những kết quả trùng nhau thui. như kiểu để select distinct
                    */

                    //Cái này để hồi lấy kết quả sau khi thực hiện hết các phép toán tìm kiếm
                    HashSet<int> tempIDsResult = new HashSet<int>();

                    //Cái deathID này là lúc dùng and not á.
                    //Ví dụ "a" and not "b" là coi như thằng nào có "a b" là lấy id bỏ vô cái deadthID này
                    //Đến hồi xét mà có thằng nào nằm trong này là loại luôn
                    HashSet<int> deathID = new HashSet<int>();

                    //Bắt đầu với toán tử đầu tiên
                    int count = 1;

                    //Thực hiện đến khi nào hết toán tử
                    while (operators.Count > 0)
                    {
                        //Toán tử tìm kiếm đẩy vô queue từ trái sang phải. kiểu thực hiện từ trái sáng phải á.
                        var operatorStr = OPERATOR[operators.Dequeue() - 1];

                        //params1 là list các param1 :V 
                        //Lợi hại khi bắt đầu kết hợp nếu có từ 2 toán tử tìm kiếm trong search_text
                        List<string> params1 = new List<string>();

                        //Cái chỗ này là á. 
                        //Khi thực hiện "abc" opr "def" nó sẽ ra 1 list kết quả hoặc thậm chí là k có kết quả nào.
                        //Cần đếm số kết quả đó khi push vô stack để hồi pop ra cho đủ nên mới tồn tại cái count này.
                        //pop ra đủ thì mới thực hiện tiếp mấy cái toán tử sau cho nó chuẩn được
                        while (count > 0)
                        {
                            params1.Add(keywords.Pop());
                            --count;
                        }

                        //param2 thì chỉ có 1 thâu.
                        string param2 = keywords.Pop();

                        //Cái này để tránh bị trùng á
                        HashSet<string> tempKeyWords = new HashSet<string>();

                        //Bắt đầu quá trình thực hiện phép toán tìm kiếm
                        foreach (var param1 in params1)
                        {
                            string query = "";

                            //Thực hiện tìm lần lượt nào
                            string tempSearchText = param1 + " " + operatorStr + " " + param2;

                            query = $"SELECT Distinct * FROM SearchJourney(N'{tempSearchText}')";

                            var tempJourneysSearchResult = _databaseWeSplit
                                                        .Database
                                                        .SqlQuery<Journey>(query)
                                                        .ToList();

                            count += tempJourneysSearchResult.Count();

                            foreach (var tempJourneySearchResult in tempJourneysSearchResult)
                            {
                                if (operators.Count == 0)
                                {
                                    tempIDsResult.Add(tempJourneySearchResult.ID_Journey);
                                }
                            }

                            while (tempKeyWords.Count > 0)
                            {
                                keywords.Push(tempKeyWords.First());
                                tempKeyWords.Remove(tempKeyWords.First());
                            }
                        }
                    }

                    //Lấy kểt quả cuối
                    bool hasConditionBefore = (condition.Length > 0 ? true : false);
                    string resultQuery = "";

                    if (tempIDsResult.Count > 0)
                    {
                        if (condition.Length > 0)
                        {
                            condition += " AND (";
                        }
                        else
                        {
                            //Do Nothing
                        }

                        foreach (var tempID in tempIDsResult)
                        {
                            condition += $" ID_Journey = {tempID} OR";
                        }

                        if (condition.Length > 0)
                        {
                            //Select * from [dbo].[Journey] where FAVORITE_FLAG = 1 AND (FOOD_GROUP = N'Ăn sáng' OR FOOD_GROUP = N'Món chính')
                            //Select * from [dbo].[Journey] where FOOD_GROUP = N'Ăn sáng'
                            condition = condition.Substring(0, condition.Length - 3);

                            if (hasConditionBefore)
                            {
                                condition += ")";
                            }
                            else
                            {
                                //Do Nothing
                            }

                            if (condition.Length > 0)
                            {
                                resultQuery = $"SELECT count(distinct(ID_Journey)) FROM Journey WHERE {condition}";
                            }
                        }
                        else
                        {
                            //Do Nothing
                        }

                        totalJourneySearch = _databaseWeSplit
                            .Database
                            .SqlQuery<int>(resultQuery)
                            .Single();

                        if (totalJourneySearch > 0)
                        {
                            resultQuery = resultQuery.Replace("count(distinct(ID_Journey))", "distinct(ID_Journey)");
                            //resultQuery += $" ORDER BY [{sortedBy.column}] {sortedBy.type} OFFSET {currentPage - 1}*{totalJourneyPerPage} ROWS FETCH NEXT {totalJourneyPerPage} ROWS ONLY";

                            var ID_journeysSearchResultList = _databaseWeSplit
                                .Database
                                .SqlQuery<int>(resultQuery)
                                .ToList();

                            foreach (var ID_Journey in ID_journeysSearchResultList)
                            {
                                Journey journey = _databaseWeSplit
                                    .Database
                                    .SqlQuery<Journey>($"Select * from Journey where ID_Journey = {ID_Journey}")
                                    .Single();

                                journeysSearchResultList.Add(journey);
                            }

                            for (int i = 0; i < journeysSearchResultList.Count; ++i)
                            {
                                journeysSearchResultList[i] = GetJourneyForBindingInListView(journeysSearchResultList[i]);
                            }
                        }
                    }
                    else
                    {
                        //Do Nothing
                    }
                }
                else
                {
                    string query = "";

                    if (condition.Length > 0)
                    {
                        query = $"SELECT count(distinct(ID_Journey)) FROM SearchJourney(N'{search_text}') WHERE {condition}";
                    }
                    else
                    {
                        query = $"SELECT count(distinct(ID_Journey)) FROM SearchJourney(N'{search_text}')";
                    }

                    totalJourneySearch = _databaseWeSplit
                        .Database
                        .SqlQuery<int>(query)
                        .Single();

                    if (totalJourneySearch > 0)
                    {
                        query = query.Replace("count(distinct(ID_Journey))", "distinct(ID_Journey)");
                        //query += $" ORDER BY [{sortedBy.column}] {sortedBy.type} OFFSET {currentPage - 1}*{totalJourneyPerPage} ROWS FETCH NEXT {totalJourneyPerPage} ROWS ONLY";

                        var ID_journeysSearchResultList = _databaseWeSplit
                            .Database
                            .SqlQuery<int>(query)
                            .ToList();

                        foreach (var ID_Journey in ID_journeysSearchResultList)
                        {
                            Journey journey = _databaseWeSplit
                                .Database
                                .SqlQuery<Journey>($"Select * from Journey where ID_Journey = {ID_Journey}")
                                .Single();

                            journeysSearchResultList.Add(journey);
                        }

                        for (int i = 0; i < journeysSearchResultList.Count; ++i)
                        {
                            journeysSearchResultList[i] = GetJourneyForBindingInListView(journeysSearchResultList[i]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
            }

            result.journeysSearchResultList = journeysSearchResultList;
            result.totalJourneySearch = totalJourneySearch;

            return result;
        }

        public string GetStandardString(string srcString)
        {
            string result = srcString;

            result = result.Trim();

            while (result.IndexOf("  ") != -1)
            {
                result = result.Replace("  ", " ");
            }

            result = result.ToLower();

            return result;
        }

        public Stack<string> GetListKeyWords(string search_text)
        {
            Stack<string> result = new Stack<string>();
            Stack<string> temp = new Stack<string>();

            List<int> indexQuotes = new List<int>();

            for (int i = 0; i < search_text.Length; ++i)
            {
                if (search_text[i] == '"')
                {
                    indexQuotes.Add(i);
                }
                else
                {
                    //Do Nothing
                }
            }

            if (indexQuotes.Count % 2 == 0)
            {
                for (int i = 0; i < indexQuotes.Count - 1; i += 2)
                {
                    string tempString = "";

                    for (int j = indexQuotes[i]; j <= indexQuotes[i + 1]; ++j)
                    {
                        tempString += search_text[j];
                    }

                    //"abc" and "123"
                    if (tempString.Length > 2)
                    {
                        temp.Push(tempString);
                    }
                }
            }

            while (temp.Count > 0)
            {
                result.Push(temp.Pop());
            }

            return result;
        }

        //Lấy cái list toán tử nè 
        public Queue<int> GetListOperator(string search_text)
        {
            Queue<int> result = new Queue<int>();

            var tokens = search_text.Split(' ');

            for (int i = 0; i < tokens.Length; ++i)
            {
                if (tokens[i] == "and")
                {
                    if (i + 1 < tokens.Length && tokens[i + 1] == "not")
                    {
                        result.Enqueue(3);
                    }
                    else
                    {
                        result.Enqueue(1);
                    }
                }
                else if (tokens[i] == "or")
                {
                    result.Enqueue(2);
                }
            }
            return result;
        }
        public (List<Journey>, int) ExecQureyToGetJourneys(string condition)
        {
            (List<Journey> JourneysResult, int totalJourney) result;
            List<Journey> JourneysResult = new List<Journey>();
            int totalJourney = 0;

            string query = "";

            if (condition.Length > 0)
            {
                query = $"SELECT COUNT(ID_Journey) FROM [dbo].[Journey] WHERE {condition}";
            }
            else
            {
                query = $"SELECT COUNT(ID_Journey) FROM [dbo].[Journey]";
            }

            totalJourney = _databaseWeSplit
                .Database
                .SqlQuery<int>(query)
                .Single();

            if (totalJourney > 0)
            {
                query = query.Replace("COUNT(ID_Journey)", "*");
                //query += $" ORDER BY [{sortedBy.column}] {sortedBy.type} OFFSET {currentPage - 1}*{totalJourneyPerPage} ROWS FETCH NEXT {totalJourneyPerPage} ROWS ONLY";

                JourneysResult = _databaseWeSplit
                .Database
                .SqlQuery<Journey>(query)
                .ToList();

                for (int i = 0; i < JourneysResult.Count; ++i)
                {
                    JourneysResult[i] = GetJourneyForBindingInListView(JourneysResult[i]);
                }
            }

            result.JourneysResult = JourneysResult;
            result.totalJourney = totalJourney;

            return result;
        }

        public void UpdateJourneyStatus(int ID_Journey, int status)
        {
            _databaseWeSplit
                .Database
                .ExecuteSqlCommand($"Update Journey Set Status = {status} Where ID_Journey = {ID_Journey}");
        }

        public void UpdateJourney(Nullable<int> idJourney, string journeyName, Nullable<int> idSite, string startPlace, string startProvince, Nullable<int> status, Nullable<System.DateTime> startDate, Nullable<System.DateTime> endDate, Nullable<double> distance)
        {
            _databaseWeSplit
                .Database
                .ExecuteSqlCommand($"Update Journey Set Journey_Name = N'{journeyName}', ID_Site = {idSite}, Start_Place = N'{startPlace}', Start_Province = N'{startProvince}', Status = {status}, StartDate = N'{startDate}', EndDate = N'{endDate}', Distance = {distance} Where ID_Journey = {idJourney}");
        }

        public void Expense(Nullable<int> idExpenses, Nullable<int> idJourney, Nullable<decimal> expense, string des, int Is_Active)
        {
            _databaseWeSplit
                .Database
                .ExecuteSqlCommand($"Update Expenses Set Expenses_Money = {expense}, Expenses_Description = N'{des}', Is_Active = {Is_Active} Where ID_Expenses = {idExpenses} And ID_Journey = {idJourney}");
        }

        public void FinishCurrentJourney()
        {
            int ID_Current_Journey = _databaseWeSplit
                .Database
                .SqlQuery<int>("Select ID_Journey from Journey Where Status = 0")
                .Single();

            UpdateJourneyStatus(ID_Current_Journey, -1);
        }
    }
}
