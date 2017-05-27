using System.Collections.Generic;
using System.Xml;
using Newtonsoft.Json;
using System;

namespace CatalogApi
{
    public class CatalogConstants : Dictionary<CatalogConstants.Key, string>
    {
        //public static KeyValuePair<CatalogConstants.Key, string> GetEntry
        //    (this IDictionary<CatalogConstants.Key, string> dictionary, CatalogConstants.Key key)
        //{
        //    return new KeyValuePair<CatalogConstants.Key, string>(key, dictionary[key]);
        //}

        public CatalogConstants()
        {
        }

        public CatalogConstants(XmlDocument xmlDoc) : this()
        {
            initialize(xmlDoc);
        }

        public CatalogConstants(string json) : this()
        {
            CatalogConstants config;

            try {
                config = JsonConvert.DeserializeObject<CatalogConstants>(json);
            } catch {
                config = new CatalogConstants();
            }

            initialize(config);
        }

        public string GetJsonValue()
        {
            return JsonConvert.SerializeObject(this);
        }

        //private struct CatalogConstantValue
        //{
        //    public string Key;

        //    public string Value;
        //}

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

        private static CatalogConstants Default = new CatalogConstants() {
            { Key.Abarth, "http://172.16.24.41:7080/navi?COUNTRY=012&SBMK=C&MAKE=F&RMODE=DEFAULT&LANGUAGE=N&WINDOW_ID=1&SB_CODE=-1&KEY=HOME&GUI_LANG=N&EPER_CAT=SP" },
            { Key.AlfaRomeo, "http://172.16.24.41:7080/navi?SBMK=R&COUNTRY=012&DRIVE=S&MAKE=R&LANGUAGE=N&ALL_FIG=0&RMODE=DEFAULT&KEY=HOME&EPER_CAT=SP&GUI_LANG=N&ALL_LIST_PART=0&PRINT_MODE=0&PREVIOUS_KEY=HOME&SB_CODE=-1&WINDOW_ID=1&SAVE_PARAM=COUNTRY" },
            { Key.Chrysler, "http://172.16.24.37:5031/CYWW/login.fve?&height=846&width=1590" },
            { Key.Fiat, "http://172.16.24.41:7080/navi?COUNTRY=012&SBMK=F&MAKE=F&RMODE=DEFAULT&LANGUAGE=N&WINDOW_ID=1&SB_CODE=-1&KEY=HOME&GUI_LANG=N&EPER_CAT=SP" },
            { Key.FiatProfessional, "http://172.16.24.41:7080/navi?COUNTRY=012&SBMK=T&MAKE=F&RMODE=DEFAULT&LANGUAGE=N&WINDOW_ID=1&SB_CODE=-1&KEY=HOME&GUI_LANG=N&EPER_CAT=SP" },
            { Key.GeneralMotors, "http://172.16.24.38:351/PQMace/" },
            { Key.IvecoBus, "http://172.16.24.34:8080/mycatric/" },
            { Key.IvecoTruck, "http://172.16.24.33:8080/mycatric/" },
            { Key.Lancia, "http://172.16.24.41:7080/navi?COUNTRY=012&SBMK=L&MAKE=L&RMODE=DEFAULT&LANGUAGE=N&WINDOW_ID=1&SB_CODE=-1&KEY=HOME&GUI_LANG=N&EPER_CAT=SP" },
            { Key.RenoImpact, "http://172.16.24.31/impact3/application" },

            #region Старая подсеть
            { Key.Chevrolet, "http://10.0.0.11:451/PQMace/login.fve?&width=1590&height=846" },
            { Key.Opel, "http://10.0.0.12:351/PQMace/root.fve?width=1590&height=846&checkNotifications" },
            { Key.Rover, "http://10.0.0.12:8080/MGRover/index.jsp" },
            { Key.Volvo, "http://10.0.60.1/Vida/" },
            { Key.VolvoImpact, "http://10.0.5.4:9907/impact3/application" },
            { Key.Blanket, "http://10.0.0.10:351/PQMace/login.fve" },
            #endregion
        };

        private void initialize(XmlDocument xmlDoc)
        {
            throw new NotImplementedException();
        }

        private void initialize(CatalogConstants config)
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
