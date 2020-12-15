using System;
using System.Collections.Generic;
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
        //private AppUtilities _appUtilities = new AppUtilities();

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
    }
}
