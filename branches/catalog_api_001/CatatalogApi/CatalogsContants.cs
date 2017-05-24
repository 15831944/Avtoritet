using System.Collections.Generic;
using System.Xml;
using Newtonsoft.Json;

namespace CatalogApi
{
    public class CatalogConstants : Dictionary<CatalogConstants.Key, string>
    {
        //public CatalogConstants(XmlDocument xmlDoc)
        //{
        //    initialize(xmlDoc);
        //}

        public CatalogConstants(string json)
        {
            initialize(JsonConvert.DeserializeObject<IEnumerable<CatalogConstantValue>>(json));
        }

        private struct CatalogConstantValue
        {
            public string Key;

            public string Value;
        }

        public enum Key {
            Abarth
            , AlfaRomeo
            , Chrysler
            , Fiat
            , FiatProfessional
            , GeneralMotors
            , IvecoBus
            , IvecoTruck
            , Lancia
            , RenoImpact

            #region Старая подсеть
            , Chevrolet
            , Opel
            , Rover
            , Volvo
            , VolvoImpact
            , Blanket
            #endregion
        }

        private void initialize(XmlDocument xmlDoc)
        {
        }

        private void initialize(IEnumerable<CatalogConstantValue> values)
        {
        }

        /* из NewLauncher-а */
        private const string Abarth = "http://172.16.24.41:7080/navi?COUNTRY=012&SBMK=C&MAKE=F&RMODE=DEFAULT&LANGUAGE=N&WINDOW_ID=1&SB_CODE=-1&KEY=HOME&GUI_LANG=N&EPER_CAT=SP";
        private const string AlfaRomeo = "http://172.16.24.41:7080/navi?SBMK=R&COUNTRY=012&DRIVE=S&MAKE=R&LANGUAGE=N&ALL_FIG=0&RMODE=DEFAULT&KEY=HOME&EPER_CAT=SP&GUI_LANG=N&ALL_LIST_PART=0&PRINT_MODE=0&PREVIOUS_KEY=HOME&SB_CODE=-1&WINDOW_ID=1&SAVE_PARAM=COUNTRY";
        private const string Chrysler = "http://172.16.24.37:5031/CYWW/login.fve?&height=846&width=1590";
        private const string Fiat = "http://172.16.24.41:7080/navi?COUNTRY=012&SBMK=F&MAKE=F&RMODE=DEFAULT&LANGUAGE=N&WINDOW_ID=1&SB_CODE=-1&KEY=HOME&GUI_LANG=N&EPER_CAT=SP";
        private const string FiatProfessional = "http://172.16.24.41:7080/navi?COUNTRY=012&SBMK=T&MAKE=F&RMODE=DEFAULT&LANGUAGE=N&WINDOW_ID=1&SB_CODE=-1&KEY=HOME&GUI_LANG=N&EPER_CAT=SP";
        private const string GeneralMotors = "http://172.16.24.38:351/PQMace/";
        private const string IvecoBus = "http://172.16.24.34:8080/mycatric/";
        private const string IvecoTruck = "http://172.16.24.33:8080/mycatric/";
        private const string Lancia = "http://172.16.24.41:7080/navi?COUNTRY=012&SBMK=L&MAKE=L&RMODE=DEFAULT&LANGUAGE=N&WINDOW_ID=1&SB_CODE=-1&KEY=HOME&GUI_LANG=N&EPER_CAT=SP";
        private const string RenoImpact = "http://172.16.24.31/impact3/application";

        #region Старая подсеть
        private const string Chevrolet = "http://10.0.0.11:451/PQMace/login.fve?&width=1590&height=846";
        private const string Opel = "http://10.0.0.12:351/PQMace/root.fve?width=1590&height=846&checkNotifications";
        private const string Rover = "http://10.0.0.12:8080/MGRover/index.jsp";
        private const string Volvo = "http://10.0.60.1/Vida/";
        private const string VolvoImpact = "http://10.0.5.4:9907/impact3/application";
        private const string Blanket = "http://10.0.0.10:351/PQMace/login.fve";
        #endregion
    }
}
