using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml;

namespace CatalogApi
{
    public class UrlConstants : Dictionary<UrlConstants.Key, string>
    {
        //public UrlConstants (XmlDocument xmlDoc)
        //{
        //    initialize(xmlDoc);
        //}

        public UrlConstants(string json)
        {
            initialize(JsonConvert.DeserializeObject<IEnumerable<UrlConstantValue>>(json));
        }

        private struct UrlConstantValue
        {
            public string Key;

            public string Value;
        }

        public enum Key {
            #region SERVICE
            WEB_PROXY
            #endregion

            #region BMW
            , BmwGroupRoot
            , BMW_WebETKStart
            , BMW_Internet
            , BMW_WebETKStartNodeRoot
            #endregion

            #region Citroen
            , CitroenRoot
            , CitroenLoginDo
            , CitroenLogoutTo
            #endregion

            #region Peugeot
            , PeugeotRoot
            , PeugeotLoginDo
            , PeugeotLogoutTo
            #endregion

            #region partslink24.com
            , PartslinkRoot
            , Partslink24Root
            , Partslink24Com
            , Partslink24ComPartslink24UserLoginDo
            , Partslink24ComPartslink24UserLogoutTo
            , Partslink24ComPartslink24BrandMenuDo
            , Partslink24ComPartslink24AjaxLoginAction
            , Partslink24ComFrame
            , PartsLink24FormRequestKeyToken
            , PartsLink24FormRequestValueToken
            #endregion

            #region chevrolet/opel
            , ChevroletOpelGroupRoot
            , ChevroletOpelGroup
            , ChevroletOpelGroupUserLoginDo
            , ChevroletOpelGroupUserLogoutTo
            #endregion

            #region Cargo bull
            , CargoBullRoot
            , CargoBullArticleSearch
            #endregion

            , EtkaRoot
            , FordRoot
            , HyundaiRoot
            , Kia_file_root
            , Kia_Http_root
            , MazdaRoot
            , MercedezRoot
            , VinOnlineRoot
            , GigantGroupRoot
            , RangerRoot
            , SafAxlesRoot
            , SsangYongRoot

            ,
        }

        private void initialize (XmlDocument xmlDoc)
        {
        }

        private void initialize(IEnumerable<UrlConstantValue> values)
        {
        }

        #region SERVICE
        private const string WEB_PROXY = "http://40d002f8ae14.sn.mynetname.net:5190";
        #endregion

        #region BMW
        private const string BmwGroupRoot = "bmwgroup";
        private const string BMW_WebETKStart = "https://www.parts.bmwgroup.com/tetis/starteApplikationAction.do?ENTRY_ID=WebETK_START";
        private const string BMW_Internet = "https://www.parts.bmwgroup.com/tetis/startTetisAction.do?DOMAIN=Internet";
        private const string BMW_WebETKStartNodeRoot = "https://www.parts.bmwgroup.com/tetis/startNode.do?APP=WebETK&ENTRY_ID=WebETK_START&NODE=ROOT:Favorite:WebETK:WebETK_START";
        #endregion

        #region Citroen
        private const string CitroenRoot = "citroen";
        private const string CitroenLoginDo = "http://service.citroen.com/do/login";
        private const string CitroenLogoutTo = "http://service.citroen.com/do/logout";
        #endregion

        #region Peugeot
        private const string PeugeotRoot = "peugeot";
        private const string PeugeotLoginDo = "http://public.servicebox.peugeot.com/do/login";
        private const string PeugeotLogoutTo = "http://public.servicebox.peugeot.com/do/logout";
        #endregion

        #region partslink24.com
        private const string PartslinkRoot = "partslink";
        private const string Partslink24Root = "partslink24";
        private const string Partslink24Com = "https://www.partslink24.com";
        private const string Partslink24ComPartslink24UserLoginDo = "https://www.partslink24.com/partslink24/user/login.do";
        private const string Partslink24ComPartslink24UserLogoutTo = "https://www.partslink24.com/partslink24/user/logout.do";
        private const string Partslink24ComFrame = "https://www.partslink24.com/partslink24/launchCatalog.do?service=bentley_parts";
        private const string Partslink24ComPartslink24BrandMenuDo = "https://www.partslink24.com/partslink24/user/brandMenu.do";
        private const string Partslink24ComPartslink24AjaxLoginAction = "https://www.partslink24.com/partslink24/login-ajax!login.action";
        private const string PartsLink24FormRequestKeyToken = "org.apache.struts.taglib.html.TOKEN";
        private const string PartsLink24FormRequestValueToken = "7bfe4fc414c2621c309b7960a92d012e";
        #endregion

        #region chevrolet/opel
        private static int chevroletopelgroup_version = 0;
        private static string[] chevroletopelgroup_root_array = { "imtportal.gm", "gme-infotech" };
        public static string ChevroletOpelGroupRoot = chevroletopelgroup_root_array[chevroletopelgroup_version];
        private static string[] chevroletopelgroup_array = { "https://imtportal.gm.com", "https://gme-infotech.com" };
        public static string ChevroletOpelGroup = chevroletopelgroup_array[chevroletopelgroup_version];
        private static string[] chevroletopelgroup_userlogindo_array = { "https://imtportal.gm.com/users/login.html", "https://gme-infotech.com/users/login.html" };
        public static string ChevroletOpelGroupUserLoginDo = chevroletopelgroup_userlogindo_array[chevroletopelgroup_version];
        private static string[] chevroletopelgroup_userlogoutto_array = { "https://imtportal.gm.com/users/logout.html", "https://gme-infotech.com/users/logout.html" };
        public static string ChevroletOpelGroupUserLogoutTo = chevroletopelgroup_userlogoutto_array[chevroletopelgroup_version];
        #endregion

        #region Cargo bull
        private const string CargoBullRoot = "cargobull";
        public static string CargoBullArticleSearch = "https://www.cargobull-serviceportal.de/Applications/ServicePortal/ArticleSearch.aspx";
        #endregion

        private const string EtkaRoot = "vin-online";
        private const string FordRoot = "Ford";
        private const string HyundaiRoot = "wpc.mobis.co.kr";
        private const string Kia_file_root = "globalserviceway";
        private const string Kia_Http_root = "wpc.mobis.co.kr";
        private const string MazdaRoot = "mazdaeur";
        private const string MercedezRoot = "EWA-net";
        private const string VinOnlineRoot = "vin-online";
        private const string GigantGroupRoot = "gigant-group";
        private const string RangerRoot = "inforanger.roadranger";
        private const string SafAxlesRoot = "saf-axles";
        private const string SsangYongRoot = "ssangyong";
    }
}