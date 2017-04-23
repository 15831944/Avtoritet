namespace CatalogApi
{
    public static class UrlConstants
    {
        #region SERVICE
        public const string WEB_PROXY = "http://40d002f8ae14.sn.mynetname.net:5190";
        #endregion

        #region BMW
        public const string BMW_Root = "bmwgroup";
        public const string BMW_ETKEntry = "https://www.parts.bmwgroup.com/tetis/starteApplikationAction.do?ENTRY_ID=WebETK_START";
        #endregion

        #region partslink24.com
        public const string Partslink24Root = "partslink24";
        public const string Partslink24Com = "https://www.partslink24.com";
        public const string Partslink24LogoutDo = "https://www.partslink24.com/partslink24/user/logout.do";
        public const string Partslink24ComFrame = "https://www.partslink24.com/partslink24/launchCatalog.do?service=bentley_parts";
        public const string Partslink24ComPartslink24UserLoginDo = "https://www.partslink24.com/partslink24/user/login.do";
        public const string Partslink24ComPartslink24BrandMenuDo = "https://www.partslink24.com/partslink24/user/brandMenu.do";
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
    }
}