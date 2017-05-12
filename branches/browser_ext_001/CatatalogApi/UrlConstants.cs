namespace CatalogApi
{
    public static class UrlConstants
    {
        #region BMW
        public const string BmwGroupRoot = "bmwgroup";
        public const string BMW_WebETKStart = "https://www.parts.bmwgroup.com/tetis/starteApplikationAction.do?ENTRY_ID=WebETK_START";
        public const string BMW_Internet = "https://www.parts.bmwgroup.com/tetis/startTetisAction.do?DOMAIN=Internet";
        public const string BMW_WebETKStartNodeRoot = "https://www.parts.bmwgroup.com/tetis/startNode.do?APP=WebETK&ENTRY_ID=WebETK_START&NODE=ROOT:Favorite:WebETK:WebETK_START";
        #endregion

        #region Citroen
        public const string CitroenRoot = "citroen";
        public const string CitroenLoginDo = "http://service.citroen.com/do/login";
        public const string CitroenLogoutTo = "http://service.citroen.com/do/logout";
        #endregion

        #region Peugeot
        public const string PeugeotRoot = "peugeot";
        public const string PeugeotLoginDo = "http://public.servicebox.peugeot.com/do/login";
        public const string PeugeotLogoutTo = "http://public.servicebox.peugeot.com/do/logout";
        #endregion

        #region partslink24.com
        public const string Partslink24Root = "partslink24";
        public const string Partslink24Com = "https://www.partslink24.com";
        public const string Partslink24ComPartslink24UserLogoutTo = "https://www.partslink24.com/partslink24/user/logout.do";
        public const string Partslink24ComFrame = "https://www.partslink24.com/partslink24/launchCatalog.do?service=bentley_parts";
        public const string Partslink24ComPartslink24UserLoginDo = "https://www.partslink24.com/partslink24/user/login.do";
        public const string Partslink24ComPartslink24BrandMenuDo = "https://www.partslink24.com/partslink24/user/brandMenu.do";
        public const string Partslink24ComPartslink24AjaxLoginAction = "https://www.partslink24.com/partslink24/login-ajax!login.action";
        public const string FormRequest = "org.apache.struts.taglib.html.TOKEN=0ee9d4e3a624fd1989c11c00449c9394&" + "loginAction=formLogin&accountLogin=ru-735778&userLogin=admin&password=Hugoboss5500";
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
        public const string CargoBullRoot = "cargobull";
        public static string CargoBullArticleSearch = "https://www.cargobull-serviceportal.de/Applications/ServicePortal/ArticleSearch.aspx";
        #endregion

        public const string EtkaRoot = "vin-online";
        public const string FordRoot = "Ford";
        public const string HyundaiRoot = "wpc.mobis.co.kr";
        public const string Kia_file_root = "globalserviceway";
        public const string Kia_Http_root = "wpc.mobis.co.kr";
        public const string MazdaRoot = "mazdaeur";
        public const string MercedezRoot = "EWA-net";
        public const string VinOnlineRoot = "vin-online";
        public const string GigantGroupRoot = "gigant-group";
        public const string RangerRoot = "inforanger.roadranger";
        public const string SafAxlesRoot = "saf-axles";
        public const string SsangYongRoot = "ssangyong";
    }
}