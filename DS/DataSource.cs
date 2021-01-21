using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using Excel = Microsoft.Office.Interop.Excel;


namespace DS
{
    public static class DataSource
    {
        public static List<BusLine> BusLineList;
        public static List<BusLineStation> BusLineStationList;
        public static List<BusStation> BusStationList;
        public static List<User> UserList;
        public static List<ConsecutiveStations> ConsecutiveStationsList;
        public static Random r = new Random();


        static DataSource()
        {
            BusLineList = new List<BusLine>();
            BusLineStationList = new List<BusLineStation>();
            BusStationList = new List<BusStation>();
            ConsecutiveStationsList = new List<ConsecutiveStations>();
            UserList = new List<User>();
            BusLineList = new List<BusLine>();
            InitAllLists();
        }
        static void InitAllLists()
        {
            BusStationList = new List<BusStation>
            {
                new BusStation
                {
                    BusStationKey = 38880,
                    Coordinates = new GeoCoordinate(31.866772 , 34.864555),
                    StationName = "התאנה/האלון",
                    StationAddress = "רחוב:התאנה  עיר: יציץ",
                    IsActive=true,
                    HasARoof=true
                },
                new BusStation
                {
                    BusStationKey = 38881,
                    Coordinates = new GeoCoordinate(31.809325 , 34.784347),
                    StationName = "דרך הפרחים/יסמין",
                    StationAddress = "רחוב:דרך הפרחים 46 עיר: גדרה",
                    IsActive=true
                },
                new BusStation
                {
                    BusStationKey = 38883,
                    Coordinates = new GeoCoordinate(31.80037 , 34.778239),
                    StationName = "יצחק רבין/פנחס ספיר",
                    StationAddress = "רחוב:דרך יצחק רבין  עיר: גדרה",
                    IsActive=true
                },
                new BusStation
                {
                    BusStationKey = 38884,
                    Coordinates = new GeoCoordinate(31.799224 , 34.782985),
                    StationName = "מנחם בגין/יצחק רבין",
                    StationAddress = " רחוב:שדרות מנחם בגין 4 עיר: גדרה",
                    IsActive=true
                },
                new BusStation
                {
                    BusStationKey = 38885,
                    Coordinates = new GeoCoordinate(31.800334 , 34.785069),
                    StationName = "חיים הרצוג/דולב",
                    StationAddress = " רחוב:חיים הרצוג 12 עיר: גדרה",
                    IsActive=true
                },
                new BusStation
                {
                    BusStationKey = 38886,
                    Coordinates = new GeoCoordinate(31.802319 , 34.786735),
                    StationName = "בית ספר גוונים/ארז",
                    StationAddress = " רחוב:ארז 2 עיר: גדרה",
                    IsActive=true,
                    HasARoof=true
                },
                new BusStation
                {
                    BusStationKey = 38887,
                    Coordinates = new GeoCoordinate(31.804595 , 34.786623),
                    StationName = "דרך האילנות/אלון",
                    StationAddress = " רחוב:דרך האילנות 13 עיר: גדרה",
                    IsActive=true,
                    HasARoof=true
                },
                 new BusStation
                {
                    BusStationKey = 38888,
                    Coordinates = new GeoCoordinate(31.805041 , 34.785098),
                    StationName = "דרך האילנות/מנחם בגין",
                    StationAddress = " רחוב:דרך האילנות 3 עיר: גדרה",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 38889,
                    Coordinates = new GeoCoordinate(31.816751 , 34.782252),
                    StationName = "העצמאות/וייצמן",
                    StationAddress = " רחוב:העצמאות 1 עיר: גדרה",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 38895,
                    Coordinates = new GeoCoordinate(31.806959 , 34.773504),
                    StationName = "בן גוריון/פוקס",
                    StationAddress = " רחוב:שדרות בן גוריון 39 עיר: גדרה",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 38898,
                    Coordinates = new GeoCoordinate(31.884187 , 34.805494),
                    StationName = "לוי אשכול/הרב דוד ישראל",
                    StationAddress = " רחוב:לוי אשכול  עיר: רחובות",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 38899,
                    Coordinates = new GeoCoordinate(31.910118 , 34.805809),
                    StationName = "שושן/אופנהיימר",
                    StationAddress = " רחוב:ד''ר אריה שושן  עיר: רחובות",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 38901,
                    Coordinates = new GeoCoordinate(31.882474 , 34.80506),
                    StationName = "הרב דוד ישראל/אריה דולצין",
                    StationAddress = " רחוב:הרב דוד ישראל 63 עיר: רחובות",
                    IsActive=true,
                    HasARoof=true
                },
                 new BusStation
                {
                    BusStationKey = 38903,
                    Coordinates = new GeoCoordinate(31.878667 , 34.81138),
                    StationName = "קרוננברג/ארגמן",
                    StationAddress = " רחוב:יוסף קרוננברג  עיר: רחובות",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 38904,
                    Coordinates = new GeoCoordinate(31.975479 , 34.813355),
                    StationName = "יעקב פריימן/בנימין שמוטקין",
                    StationAddress = " רחוב:יעקב פריימן 9 עיר: ראשון לציון",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 38905,
                    Coordinates = new GeoCoordinate(31.982177 , 34.789445),
                    StationName = "אנילביץ'/שלום אש",
                    StationAddress = " רחוב:אנילביץ' 13 עיר: ראשון לציון",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 38906,
                    Coordinates = new GeoCoordinate(31.948552 , 34.822422),
                    StationName = "נחמיה/בית העלמין",
                    StationAddress = " רחוב:נחמיה 71 עיר: ראשון לציון",
                    IsActive=true,
                    HasARoof=true
                },
                 new BusStation
                {
                    BusStationKey = 38907,
                    Coordinates = new GeoCoordinate(31.967732 , 34.816339),
                    StationName = "יהודה הלוי/יוחנן הסנדלר",
                    StationAddress = " רחוב:יהודה הלוי 89 עיר: ראשון לציון",
                    IsActive=true,
                    HasARoof=true
                },
                 new BusStation
                {
                    BusStationKey = 38908,
                    Coordinates = new GeoCoordinate(31.893823 , 34.824617),
                    StationName = "ההגנה/חי''ש",
                    StationAddress = " רחוב:ההגנה 37 עיר: רחובות",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 38909,
                    Coordinates = new GeoCoordinate(31.854169 , 34.824714),
                    StationName = "קרית עקרון/כביש 411",
                    StationAddress = " רחוב:411  עיר: גזר",
                    IsActive=true,
                    HasARoof=true
                },
                 new BusStation
                {
                    BusStationKey = 38910,
                    Coordinates = new GeoCoordinate(31.811907 , 34.900793),
                    StationName = "צומת חולדה/כביש 411",
                    StationAddress = " רחוב:411  עיר: גזר",
                    IsActive=true,
                    HasARoof=true
                },
                 new BusStation
                {
                    BusStationKey = 38911,
                    Coordinates = new GeoCoordinate(31.956842 , 34.814839),
                    StationName = "גרינשפן/יגאל אלון",
                    StationAddress = " רחוב:גרינשפן  עיר: ראשון לציון",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 38913,
                    Coordinates = new GeoCoordinate(31.992306 , 34.75691),
                    StationName = "משה שרת/יעקב קנר",
                    StationAddress = " רחוב:משה שרת 38 עיר: ראשון לציון",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 38914,
                    Coordinates = new GeoCoordinate(31.98497 , 34.78262),
                    StationName = "הדייגים/הנחשול",
                    StationAddress = " רחוב:הדייגים 45 עיר: ראשון לציון",
                    IsActive=true,
                    HasARoof=true
                },
                 new BusStation
                {
                    BusStationKey = 38916,
                    Coordinates = new GeoCoordinate(31.968049 , 34.818099),
                    StationName = "יוסף בורג/משואות יצחק",
                    StationAddress = " רחוב:דר יוסף בורג 9 עיר: ראשון לציון",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 38917,
                    Coordinates = new GeoCoordinate(31.968936 , 34.820043),
                    StationName = "יוסף בורג/כתריאל רפפורט",
                    StationAddress = " רחוב:דר יוסף בורג עיר: ראשון לציון",
                    IsActive=true,
                    HasARoof=true
                },
                 new BusStation
                {
                    BusStationKey = 38919,
                    Coordinates = new GeoCoordinate(31.923041 , 34.798033),
                    StationName = "וייצמן/דרך יצחק רבין",
                    StationAddress = " רחוב:וייצמן  עיר: נס ציונה",
                    IsActive=true,
                    HasARoof=true
                },
                 new BusStation
                {
                    BusStationKey = 38920,
                    Coordinates = new GeoCoordinate(31.98568 , 34.764014),
                    StationName = "שדרות משה דיין/יוסף לישנסקי",
                    StationAddress = " רחוב:שדרות משה דיין  עיר: ראשון לציון",
                    IsActive=true,
                    HasARoof=true
                },
                 new BusStation
                {
                    BusStationKey = 38921,
                    Coordinates = new GeoCoordinate(31.992583 , 34.751999),
                    StationName = "השר חיים שפירא/יוסף ספיר",
                    StationAddress = " רחוב:השר חיים משה שפירא 4 עיר: ראשון לציון",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 38922,
                    Coordinates = new GeoCoordinate(31.990757 , 34.755683),
                    StationName = "השר חיים שפירא/הרב שלום ג'רופי",
                    StationAddress = " רחוב:השר חיים משה שפירא 16 עיר: ראשון לציון",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 39001,
                    Coordinates = new GeoCoordinate(31.950254 , 34.819244),
                    StationName = "שדרות יעקב/יוסף הנשיא",
                    StationAddress = " רחוב:שדרות יעקב 65 עיר: ראשון לציון",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 39002,
                    Coordinates = new GeoCoordinate(31.95111 , 34.819766),
                    StationName = "שדרות יעקב/עזרא",
                    StationAddress = " רחוב:שדרות יעקב 59 עיר: ראשון לציון",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 39004,
                    Coordinates = new GeoCoordinate(31.905052 , 34.818909),
                    StationName = "לייב יוספזון/יעקב ברמן",
                    StationAddress = " רחוב:יהודה לייב יוספזון  עיר: רחובות",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 39005,
                    Coordinates = new GeoCoordinate(31.901879 , 34.819443),
                    StationName = "הרב יעקב ברמן/הרב יהודה צבי מלצר",
                    StationAddress = " רחוב:הרב יעקב ברמן 4 עיר: רחובות",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 39006,
                    Coordinates = new GeoCoordinate(31.90281 , 34.818922),
                    StationName = "ברמן/מלצר",
                    StationAddress = " רחוב:הרב יעקב ברמן  עיר: רחובות",
                    IsActive=true,
                    HasARoof=true
                },
                 new BusStation
                {
                    BusStationKey = 39007,
                    Coordinates = new GeoCoordinate(31.904567 , 34.815296),
                    StationName = "הנשיא הראשון/מכון ויצמן",
                    StationAddress = " רחוב:הנשיא הראשון 55 עיר: רחובות",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 39008,
                    Coordinates = new GeoCoordinate(31.904755 , 34.816661),
                    StationName = "הנשיא הראשון/קיפניס",
                    StationAddress = " רחוב:הנשיא הראשון 56 עיר: רחובות",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 39012,
                    Coordinates = new GeoCoordinate(31.937387 , 34.838609),
                    StationName = "הירדן/הערבה",
                    StationAddress = " רחוב:הירדן 23 עיר: באר יעקב",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 39013,
                    Coordinates = new GeoCoordinate(31.936925 , 34.838341),
                    StationName = "הירדן/חרוד",
                    StationAddress = " רחוב:הירדן 22 עיר: באר יעקב",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 39014,
                    Coordinates = new GeoCoordinate(31.939037 , 34.831964),
                    StationName = "האלונים/הדקל",
                    StationAddress = " רחוב:שדרות האלונים  עיר: באר יעקב",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 39017,
                    Coordinates = new GeoCoordinate(31.939656 , 34.832104),
                    StationName = "האלונים א/הדקל",
                    StationAddress = " רחוב:שדרות האלונים  עיר: באר יעקב",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 39018,
                    Coordinates = new GeoCoordinate(31.914324 , 35.023589),
                    StationName = "פארק תעשיות שילת",
                    StationAddress = " רחוב:דרך הזית  עיר: שילת",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 39019,
                    Coordinates = new GeoCoordinate(31.914816 , 35.023028),
                    StationName = "פארק תעשיות שילת",
                    StationAddress = " רחוב:דרך הזית  עיר: שילת",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 39024,
                    Coordinates = new GeoCoordinate(31.908499 , 35.007955),
                    StationName = "עיריית מודיעין מכבים רעות",
                    StationAddress = " רחוב:  עיר: מודיעין מכבים רעות",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 39028,
                    Coordinates = new GeoCoordinate(31.907828 , 35.000614),
                    StationName = "חיים ברלב/מרדכי מקלף",
                    StationAddress = " רחוב:חיים ברלב 30 עיר: מודיעין מכבים רעות",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 39029,
                    Coordinates = new GeoCoordinate(31.907603 , 35.000816),
                    StationName = "חיים ברלב/מרדכי מקלף",
                    StationAddress =  "רחוב:חיים ברלב  עיר: מודיעין מכבים רעות",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 39035,
                    Coordinates = new GeoCoordinate(31.941611 , 34.843114),
                    StationName = "אלמוג סנטר/אפרים קישון",
                    StationAddress =  " רחוב:אפרים קישון 20 עיר: באר יעקב",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 39039,
                    Coordinates = new GeoCoordinate(31.931068 , 34.884936),
                    StationName = "גן חק''ל/רבאט",
                    StationAddress =  " רחוב:רבאט  עיר: רמלה",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 39041,
                    Coordinates = new GeoCoordinate(31.933379 , 34.887207),
                    StationName = "קניון צ. רמלה לוד/דוכיפת",
                    StationAddress =  " רחוב:דוכיפת  עיר: רמלה",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 39042,
                    Coordinates = new GeoCoordinate(31.929318 , 34.880069),
                    StationName = "היצירה/התקווה",
                    StationAddress =  " רחוב:היצירה 2 עיר: רמלה",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 39044,
                    Coordinates = new GeoCoordinate(31.932402 , 34.881442),
                    StationName = "עמל/היצירה",
                    StationAddress =  " רחוב:עמל  עיר: רמלה",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 39049,
                    Coordinates = new GeoCoordinate(31.936159 , 34.864906),
                    StationName = "פרנקל/ויתקין",
                    StationAddress =  " רחוב:ישראל פרנקל 10 עיר: רמלה",
                    IsActive=true
                },
                 new BusStation
                {
                    BusStationKey = 39050,
                    Coordinates = new GeoCoordinate(31.936022 , 34.86495),
                    StationName = "פרנקל/ויתקין",
                    StationAddress =  " רחוב:ישראל פרנקל 11 עיר: רמלה",
                    IsActive=true
                }
            };
            for (int i = 0; i < 12; i++)
            {
                BusLineList.Add(new BusLine
                {
                    BusLineKey = RunNumbers.BusLineRunNumber++,
                    LineNumber = r.Next(1000000, 10000000),
                    Area = (Areas)r.Next(0, 5),
                    IsActive = true
                });
            }
            for(int i=0;i<12;i++)
            {
                int prevBusLineStation = -1;
                for (int j=1;j<6;j++)
                {
                    int busStationKey;
                    do
                    {
                        busStationKey = r.Next(38880, RunNumbers.BusStationRunNumber);
                    }
                    while ((BusLineStationList.FirstOrDefault(b => (b.BusLineKey == (20000 + i) & b.BusStationKey == busStationKey)) != null)||(BusStationList.FirstOrDefault(b=>(b.BusStationKey== busStationKey))==null));
                    BusStation bus = BusStationList.FirstOrDefault(b => b.BusStationKey == busStationKey);
                    if (j==1)
                    {
                        BusLineList.Find(b => b.BusLineKey == (20000 + i)).FirstStation = busStationKey;
                        BusLineList.Find(b => b.BusLineKey == (20000 + i)).FirstStationName = bus.StationName;
                    }
                    if(j==5)
                    {
                        BusLineList.Find(b => b.BusLineKey == (20000 + i)).LastStation = busStationKey;
                        BusLineList.Find(b => b.BusLineKey == (20000 + i)).LastStationName = bus.StationName;
                    }
                    BusLineStationList.Add(new BusLineStation
                    {
                        BusLineKey = 20000+i,
                        StationName=bus.StationName,
                        BusStationKey = busStationKey,
                        IsActive = true,
                        StationNumberInLine = j
                    });
                    var ConsecutiveStation = new ConsecutiveStations { Station1Key=prevBusLineStation, Station2Key = busStationKey, IsActive = true };
                    if (ConsecutiveStation.Station1Key == -1)
                    {
                        ConsecutiveStation.Distance = 0;
                        ConsecutiveStation.DriveDistanceTime = TimeSpan.Zero;
                    }
                    else
                    {
                        ConsecutiveStation.Distance = BusStationList.Find(b => (b.BusStationKey == prevBusLineStation & b.IsActive)).Coordinates.GetDistanceTo(BusStationList.Find(b => (b.BusStationKey == busStationKey & b.IsActive)).Coordinates);
                        ConsecutiveStation.DriveDistanceTime = TimeSpan.FromMinutes(ConsecutiveStation.Distance * 0.01);
                    }
                    ConsecutiveStationsList.Add(ConsecutiveStation);
                    prevBusLineStation = busStationKey;
                }
            }
            UserList.Add(new User { UserName = "raaya", Password = "123", IsActive = true, ManagementPermission = true, Gender = gender.female, imagePath = null });
            UserList.Add(new User { UserName = "odelia", Password = "1666", IsActive = true, ManagementPermission = true, Gender = (gender)0, imagePath = "Icons/cancel.png" });
            UserList.Add(new User { UserName = "aviva", Password = "1111", IsActive = true, ManagementPermission = false, Gender = (gender)0, imagePath = null });
            UserList.Add(new User { UserName = "myiah", Password = "6543", IsActive = true, ManagementPermission = false, Gender = (gender)0, imagePath = "Icons/wonan.png" });

        }
    }
}
