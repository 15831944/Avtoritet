using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml;

namespace CatalogApi
{
    public class UrlConstants : Dictionary<UrlConstants.Key, string>
    {
        public UrlConstants()
        {
            ////string value = string.Empty;

            ////foreach (Key key in System.Enum.GetValues(typeof(Key))) {
            ////    value = string.Empty;

            ////    switch (key) {
            ////        default:
            ////            break;
            ////    }

            ////    if (string.IsNullOrWhiteSpace(value) == false)
            ////        Add(key, value);
            ////    else
            ////        ;
            ////}
        }

        public UrlConstants(XmlDocument xmlDoc) : this()
        {
            initialize(xmlDoc);
        }

        public UrlConstants(string json) : this()
        {
            UrlConstants config;

            try {
                config = JsonConvert.DeserializeObject<UrlConstants>(json);
            } catch {
                config = new UrlConstants();
            }

            initialize(config);
        }

        public string GetJsonValue()
        {
            return JsonConvert.SerializeObject(this);
        }

        //private struct UrlConstantValue
        //{
        //    public string Key;

        //    public string Value;
        //}

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
            //, Partslink24ComBentlyParts
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
            throw new NotImplementedException();
        }

        private static UrlConstants Default = new UrlConstants() {
            #region SERVICE
            { Key.WEB_PROXY, @"http://40d002f8ae14.sn.mynetname.net:5190" },
            #endregion

            #region BMW
            { Key.BmwGroupRoot, "bmwgroup" },
            { Key.BMW_WebETKStart, "https://www.parts.bmwgroup.com/tetis/starteApplikationAction.do?ENTRY_ID=WebETK_START" },
            { Key.BMW_Internet, "https://www.parts.bmwgroup.com/tetis/startTetisAction.do?DOMAIN=Internet" },
            { Key.BMW_WebETKStartNodeRoot, "https://www.parts.bmwgroup.com/tetis/startNode.do?APP=WebETK&ENTRY_ID=WebETK_START&NODE=ROOT:Favorite:WebETK:WebETK_START" },
            #endregion

            #region Citroen
            { Key.CitroenRoot, "citroen" },
            { Key.CitroenLoginDo, "http://service.citroen.com/do/login" },
            { Key.CitroenLogoutTo, "http://service.citroen.com/do/logout" },
            #endregion

            #region Peugeot
            { Key.PeugeotRoot, "peugeot" },
            { Key.PeugeotLoginDo, "http://public.servicebox.peugeot.com/do/login" },
            { Key.PeugeotLogoutTo, "http://public.servicebox.peugeot.com/do/logout" },
            #endregion

            #region partslink24.com
            { Key.PartslinkRoot, "partslink" },
            { Key.Partslink24Root, "partslink24" },
            { Key.Partslink24Com, "https://www.partslink24.com" },
            { Key.Partslink24ComPartslink24UserLoginDo, "https://www.partslink24.com/partslink24/user/login.do" },
            { Key.Partslink24ComPartslink24UserLogoutTo, "https://www.partslink24.com/partslink24/user/logout.do" },
            //{ Key.Partslink24ComBentlyParts, "https://www.partslink24.com/partslink24/launchCatalog.do?service=bentley_parts" },
            { Key.Partslink24ComPartslink24BrandMenuDo, "https://www.partslink24.com/partslink24/user/brandMenu.do" },
            { Key.Partslink24ComPartslink24AjaxLoginAction, "https://www.partslink24.com/partslink24/login-ajax!login.action" },
            { Key.PartsLink24FormRequestKeyToken, "org.apache.struts.taglib.html.TOKEN" },
            { Key.PartsLink24FormRequestValueToken, "7bfe4fc414c2621c309b7960a92d012e" },
            #endregion

            #region chevrolet/opel
            //private static int chevroletopelgroup_version = 0;
            //private static string[] chevroletopelgroup_root_array = { "imtportal.gm", "gme-infotech" };
            //public static string ChevroletOpelGroupRoot = chevroletopelgroup_root_array[chevroletopelgroup_version];
            { Key.ChevroletOpelGroupRoot, "imtportal.gm" },
            //private static string[] chevroletopelgroup_array = { "https://imtportal.gm.com", "https://gme-infotech.com" };
            //public static string ChevroletOpelGroup = chevroletopelgroup_array[chevroletopelgroup_version];
            { Key.ChevroletOpelGroup, "https://imtportal.gm.com" },
            //private static string[] chevroletopelgroup_userlogindo_array = { "https://imtportal.gm.com/users/login.html", "https://gme-infotech.com/users/login.html" };
            //public static string ChevroletOpelGroupUserLoginDo = chevroletopelgroup_userlogindo_array[chevroletopelgroup_version];
            { Key.ChevroletOpelGroupUserLoginDo, "https://imtportal.gm.com/users/login.html" },
            //private static string[] chevroletopelgroup_userlogoutto_array = { "https://imtportal.gm.com/users/logout.html", "https://gme-infotech.com/users/logout.html" };
            //public static string ChevroletOpelGroupUserLogoutTo = chevroletopelgroup_userlogoutto_array[chevroletopelgroup_version];
            { Key.ChevroletOpelGroupUserLogoutTo, "https://imtportal.gm.com/users/logout.html" },
            #endregion

            #region Cargo bull
            { Key.CargoBullRoot, "cargobull" },
            { Key.CargoBullArticleSearch, "https://www.cargobull-serviceportal.de/Applications/ServicePortal/ArticleSearch.aspx" },
            #endregion

            #region Common items
            { Key.EtkaRoot, "vin-online" },
            { Key.FordRoot, "Ford" },
            { Key.HyundaiRoot, "wpc.mobis.co.kr" },
            { Key.Kia_file_root, "globalserviceway" },
            { Key.Kia_Http_root, "wpc.mobis.co.kr" },
            { Key.MazdaRoot, "mazdaeur" },
            { Key.MercedezRoot, "EWA-net" },
            { Key.VinOnlineRoot, "vin-online" },
            { Key.GigantGroupRoot, "gigant-group" },
            { Key.RangerRoot, "inforanger.roadranger" },
            { Key.SafAxlesRoot, "saf-axles" },
            { Key.SsangYongRoot, "ssangyong" },
            #endregion
        };

        private void initialize(UrlConstants config)
        {
            foreach (Key key in Enum.GetValues(typeof(Key))) {
                if (config.ContainsKey(key) == true)
                    if (ContainsKey(key) == true)
                        if (this[key].Equals(config[key]) == false)
                            this[key] = config[key];
                        else
                        // значения равны - оставить как есть
                            ;
                    else
                        Add(key, config[key]);
                else
                    Add(key, Default[key]);
            }
        }
    }
}