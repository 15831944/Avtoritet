using RelayServer.DataContext;
using RelayServer.Entities;
using RelayServer.Helpers;
using RelayServer.Interfaces;
using RelayServer.Models;
using RelayServer.Portals;
using RelayServer.Settings;
using RequestHandlers;
using RequestHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace RelayServer.Processors
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, AddressFilterMode = AddressFilterMode.Any)]
	public class RequestProcessor : IRequestProcessor, IFileServer
	{
		private const string TempFolder = "Temp";

		private static readonly OpelPortal OpelPortal;

		private static readonly CitroenPortal CitroenPortal;

		private static readonly PeugeotPortal PeugeotPortal;

		private static readonly ChevroletPortal ChevroletPortal;

		private static readonly PartslinkPortal PartslinkPortal;

		private static readonly object SyncLoadSettings;

		private static readonly object SyncLoadNewUpdate;

		private static readonly object SyncLoadGuiSettings;

		private static readonly System.Collections.Generic.Dictionary<string, System.DateTime> UserStatistics;

		private readonly ISettingsZipper settingsZipper;

		static RequestProcessor()
		{
			RequestProcessor.SyncLoadSettings = new object();
			RequestProcessor.SyncLoadNewUpdate = new object();
			RequestProcessor.SyncLoadGuiSettings = new object();
			RequestProcessor.UserStatistics = new System.Collections.Generic.Dictionary<string, System.DateTime>();
			RequestProcessor.OpelPortal = new OpelPortal();
			RequestProcessor.CitroenPortal = new CitroenPortal();
			RequestProcessor.PeugeotPortal = new PeugeotPortal();
			RequestProcessor.ChevroletPortal = new ChevroletPortal();
			RequestProcessor.PartslinkPortal = new PartslinkPortal();
			AccountProcessor.Instance.Initialize();
		}

		public RequestProcessor()
		{
			RequestProcessor.SetupServicePointManager();
			this.settingsZipper = new SettingsZipper();
		}

		public System.IO.Stream DownloadUpdate()
		{
			System.IO.Stream result;
			try
			{
				lock (RequestProcessor.SyncLoadNewUpdate)
				{
					string fileName = System.IO.Path.Combine("Temp", "Temp.zip");
					if (!System.IO.Directory.Exists("Temp"))
					{
						System.IO.Directory.CreateDirectory("Temp");
					}
					if (!System.IO.File.Exists(fileName))
					{
						ZipFile.CreateFromDirectory("Temp", fileName);
					}
					result = System.IO.File.OpenRead(fileName);
				}
			}
			catch (System.Exception ex)
			{
				ErrorLogHelper.AddErrorInLog("DownloadUpdate()", ex.Message + "|" + ex.StackTrace);
				ConsoleHelper.Error(string.Format("{0} || {1} || {2}", ex.Message, ex.StackTrace, ex.Data));
				result = null;
			}
			return result;
		}

		public System.IO.Stream DownloadSettings()
		{
			System.IO.Stream result;
			try
			{
				lock (RequestProcessor.SyncLoadSettings)
				{
					string settings = this.settingsZipper.CreateZipFromSettings(RequestProcessor.ReadConfigToString());
					result = System.IO.File.OpenRead(settings);
				}
			}
			catch (System.Exception ex)
			{
				ErrorLogHelper.AddErrorInLog("DownloadSettings()", ex.Message + "|" + ex.StackTrace);
				ConsoleHelper.Error(string.Format("{0} || {1} || {2}", ex.Message, ex.StackTrace, ex.Data));
				result = null;
			}
			return result;
		}

		public void OpenSession(string url, bool forceSession)
		{
			try
			{
				if (url.Contains("citroen"))
				{
					RequestProcessor.CitroenPortal.OpenSession(url, forceSession);
				}
				else if (url.Contains("peugeot"))
				{
					RequestProcessor.PeugeotPortal.OpenSession(url, forceSession);
				}
				else if (url.Contains(CatalogApi.UrlConstants.Partslink24Root))
				{
					RequestProcessor.PartslinkPortal.OpenSession(url, forceSession);
				}
				else
				{
					if (!url.Contains(CatalogApi.UrlConstants.ChevroletOpelGroupRoot))
					{
                        //throw new System.Exception("Open session error");
					    RequestProcessor.ChevroletPortal.OpenSession(url.Replace("chevrolet", string.Empty), forceSession);
                    }
					if (url.Contains("opel"))
					{
						RequestProcessor.OpelPortal.OpenSession(url.Replace("opel", string.Empty), forceSession);
					}
					if (url.Contains("chevrolet"))
					{
						RequestProcessor.ChevroletPortal.OpenSession(url.Replace("chevrolet", string.Empty), forceSession);
					}
				}
			}
			catch (System.Exception ex)
			{
				ErrorLogHelper.AddErrorInLog("OpenSession()", ex.Message + "|" + ex.StackTrace);
				ConsoleHelper.Error(string.Format("{0} || {1} || {2}", ex.Message, ex.StackTrace, ex.Data));
			}
		}

		public void CloseSession(string url)
		{
			try
			{
				if (url.Contains("citroen"))
				{
					RequestProcessor.CitroenPortal.CloseSession(url);
				}
				else if (url.Contains("peugeot"))
				{
					RequestProcessor.PeugeotPortal.CloseSession(url);
				}
				else if (url.Contains(CatalogApi.UrlConstants.Partslink24Root))
				{
					RequestProcessor.PartslinkPortal.CloseSession(url);
				}
				else if (url.Contains(CatalogApi.UrlConstants.ChevroletOpelGroupRoot))
				{
					RequestProcessor.ChevroletPortal.CloseSession(url);
				}
                else
                    throw new System.Exception("Close session error");
			}
			catch (System.Exception ex)
			{
				ErrorLogHelper.AddErrorInLog("CloseSession()", ex.Message + "|" + ex.StackTrace);
				ConsoleHelper.Error(string.Format("{0} || {1} || {2}", ex.Message, ex.StackTrace, ex.Data));
			}
		}

		public string GetCookies(string url)
		{
			string result;
			try
			{
				if (url.Contains("citroen"))
				{
					result = RequestProcessor.CitroenPortal.GetCookies(url);
				}
				else if (url.Contains("peugeot"))
				{
					result = RequestProcessor.PeugeotPortal.GetCookies(url);
				}
				else
				{
					if (!url.Contains(CatalogApi.UrlConstants.Partslink24Root))
					{
						if (url.Contains(CatalogApi.UrlConstants.ChevroletOpelGroupRoot))
						{
						    result = RequestProcessor.ChevroletPortal.GetCookies(url);
						    return result;

                            //if (url.Contains("opel"))
                            //{
                            //	result = RequestProcessor.OpelPortal.GetCookies(url.Replace("opel", string.Empty));
                            //	return result;
                            //}
                            //if (url.Contains("chevrolet"))
                            //{
                            //	result = RequestProcessor.ChevroletPortal.GetCookies(url.Replace("opel", string.Empty));
                            //	return result;
                            //}
                        }

						throw new System.Exception("Getting cookies error");
					}

					result = RequestProcessor.PartslinkPortal.GetCookies(url);
				}
			}
			catch (System.Exception ex)
			{
				ErrorLogHelper.AddErrorInLog("GetCookies()", ex.Message + "|" + ex.StackTrace);
				ConsoleHelper.Error(string.Format("{0} || {1} || {2}", ex.Message, ex.StackTrace, ex.Data));
				result = string.Empty;
			}
			return result;
		}

		public string DownloadGuiSettigs()
		{
			string result;
			try
			{
				lock (RequestProcessor.SyncLoadGuiSettings)
				{
					using (AvtoritetEntities ae = new AvtoritetEntities())
					{
						SettingUpdate updateFlag = ae.SettingUpdate.FirstOrDefault<SettingUpdate>();
						string settings = string.Empty;
						string baseDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
						string root = System.IO.Path.Combine(baseDir, ResourceManager.Root);
						string path = string.Join("\\", new string[]
						{
							root,
							"settingui.zip"
						});
						if (updateFlag != null && (!updateFlag.Update || !System.IO.File.Exists(path)))
						{
							settings = RequestProcessor.ReadConfigToString();
							if (System.IO.File.Exists(path))
							{
								System.IO.File.Delete(path);
							}
							System.IO.File.WriteAllText(path, settings, System.Text.Encoding.Unicode);
							updateFlag.Update = true;
							ae.SaveChanges();
						}
						else if (System.IO.File.Exists(path))
						{
							settings = System.IO.File.ReadAllText(path, System.Text.Encoding.Unicode);
						}
						else
						{
							settings = null;
						}
						result = settings;
					}
				}
			}
			catch (System.Exception ex)
			{
				ErrorLogHelper.AddErrorInLog("DownloadGuiSettigs()", ex.Message + "|" + ex.StackTrace);
				result = string.Format("[{0}] {1} / {2}", System.DateTime.Now, ex.Message, ex.StackTrace);
			}
			return result;
		}

		public bool IsServiceAvailable()
		{
			bool result;
			try
			{
				CookieContainer cookieContainer = new CookieContainer();
				IRequestHandler requestHandler = RequestHandlerFactory.Create("http://www.partslink24.com/", string.Empty, string.Empty, null);
				Task<HttpResponseMessage> responseMessage = requestHandler.GetSessionAsync("http://www.partslink24.com/", cookieContainer);
				string content = responseMessage.Result.Content.ReadAsStringAsync().Result;
				result = (!content.Contains("maintenance_general") && int.Parse(RequestProcessor.GetConfig("Partslink")) > 0);
			}
			catch (System.Exception ex)
			{
				ErrorLogHelper.AddErrorInLog("IsServiceAvailable()", ex.Message + "|" + ex.StackTrace);
				ConsoleHelper.Error(string.Format("{0} || {1} || {2}", ex.Message, ex.StackTrace, ex.Data));
				result = false;
			}
			return result;
		}

		public void LogConnection(string machineName, string launcherVersion)
		{
			try
			{
				if (!RequestProcessor.UserStatistics.ContainsKey(machineName))
				{
					RequestProcessor.UserStatistics.Add(machineName, System.DateTime.Now);
					ConsoleHelper.Info(string.Concat(new object[]
					{
						machineName,
						" Connected. ",
						launcherVersion,
						". Total Connections: ",
						RequestProcessor.UserStatistics.Count
					}));
				}
				else if (RequestProcessor.UserStatistics.Remove(machineName))
				{
					RequestProcessor.UserStatistics.Add(machineName, System.DateTime.Now);
					ConsoleHelper.Info(string.Concat(new object[]
					{
						machineName,
						" Connected. ",
						launcherVersion,
						". Total Connections: ",
						RequestProcessor.UserStatistics.Count
					}));
				}
				else
				{
					ConsoleHelper.Error(machineName + " can't be deleted. Total Connections: " + RequestProcessor.UserStatistics.Count);
				}
			}
			catch (System.Exception ex)
			{
				ErrorLogHelper.AddErrorInLog("LogConnection()", ex.Message + "|" + ex.StackTrace);
				ConsoleHelper.Error(ex.Message + " || " + ex.StackTrace);
			}
		}

		public void FreeOccupiedAccount(string loginName)
		{
			AccountProcessor.Instance.FreeOccupiedAccount(loginName);
		}

		public AccountModel GetUnoccupiedAccount()
		{
			return AccountProcessor.Instance.GetUnoccupiedAccount();
		}

		private static string GetConfig(string settingsName)
		{
			return ConfigurationManager.AppSettings[settingsName];
		}

		private static string GetConfigPath()
		{
			string catalog = ConfigurationManager.AppSettings["Catalog"];
			return System.IO.Path.Combine(ResourceManager.Root, catalog);
		}

		private static string ReadConfigToString()
		{
			string buffer = string.Empty;
			string zipBuffer = string.Empty;
			Catalog catalog = new Catalog();
			catalog.Rows = 2;
			using (AvtoritetEntities ae = new AvtoritetEntities())
			{
				System.Collections.Generic.List<RelayServer.DataContext.Group> groups = (from r in ae.Group
				where r.Enable
				select r into t
				orderby t.Order
				select t).ToList<RelayServer.DataContext.Group>();
				catalog.Groups = new System.Collections.Generic.List<RelayServer.Models.Group>();
				using (System.Collections.Generic.List<RelayServer.DataContext.Group>.Enumerator enumerator = groups.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						RelayServer.DataContext.Group group = enumerator.Current;
						RelayServer.Models.Group catalogGroup = new RelayServer.Models.Group();
						System.Collections.Generic.List<RelayServer.DataContext.GroupBox> groupboxs = (from t in ae.GroupBox
						where t.GroupId == @group.GroupId && t.Enable
						select t into r
						orderby r.Title
						select r).ToList<RelayServer.DataContext.GroupBox>();
						catalogGroup.GroupBoxs = new System.Collections.Generic.List<RelayServer.Models.GroupBox>();
						using (System.Collections.Generic.List<RelayServer.DataContext.GroupBox>.Enumerator enumerator2 = groupboxs.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								RelayServer.DataContext.GroupBox groupBox = enumerator2.Current;
								RelayServer.Models.GroupBox catalogGroupBox = new RelayServer.Models.GroupBox();
								System.Collections.Generic.List<RelayServer.DataContext.Brand> brands = (from t in ae.Brand
								where t.GroupBoxId == groupBox.GroupBoxId && t.Enable
								select t into r
								orderby r.NameAndFolder
								select r).ToList<RelayServer.DataContext.Brand>();
								catalogGroupBox.Brands = new System.Collections.Generic.List<RelayServer.Models.Brand>();
								using (System.Collections.Generic.List<RelayServer.DataContext.Brand>.Enumerator enumerator3 = brands.GetEnumerator())
								{
									while (enumerator3.MoveNext())
									{
										RelayServer.DataContext.Brand brand = enumerator3.Current;
										RelayServer.Models.Brand catalogBrand = new RelayServer.Models.Brand();
										System.Collections.Generic.List<RelayServer.DataContext.Provider> providers = (from t in ae.Provider
										where t.BrandId == brand.BrandId && t.Enable
										select t into r
										orderby r.Order
										select r).ToList<RelayServer.DataContext.Provider>();
										catalogBrand.Providers = new System.Collections.Generic.List<RelayServer.Models.Provider>();
										using (System.Collections.Generic.List<RelayServer.DataContext.Provider>.Enumerator enumerator4 = providers.GetEnumerator())
										{
											while (enumerator4.MoveNext())
											{
												RelayServer.DataContext.Provider provider = enumerator4.Current;
												ProviderAccount provAcc = ae.ProviderAccount.FirstOrDefault((ProviderAccount t) => t.ProviderId == provider.ProviderId && t.Enable);
												System.Collections.Generic.List<RelayServer.Models.CommandFile> commandFiles = (from t in ae.CommandFile
												where t.ProviderId == (int?)provider.ProviderId
												select t into r
												select new RelayServer.Models.CommandFile
												{
													FileName = r.FileName,
													FileContent = r.FileContent
												}).ToList<RelayServer.Models.CommandFile>();
												System.Collections.Generic.List<RelayServer.Models.ProviderFile> providerFiles = (from t in ae.ProviderFile
												where t.ProviderId == (int?)provider.ProviderId
												select t into r
												select new RelayServer.Models.ProviderFile
												{
													FileName = r.FileName,
													FileExt = r.FileExt,
													FileSize = (long)r.FileSize,
													FileContent = r.FileContent
												}).ToList<RelayServer.Models.ProviderFile>();
												catalogBrand.Providers.Add(new RelayServer.Models.Provider
												{
													Uri = provider.Uri.Trim(),
													IconPath = provider.IconPath,
													Title = provider.Title,
													Order = (provider.Order ?? 0),
													Commands = provider.commandcontent,
													Login = ((provAcc != null) ? provAcc.Login.Trim() : string.Empty),
													Password = ((provAcc != null) ? provAcc.Password.Trim() : string.Empty),
													CommandFiles = commandFiles,
													ProviderFiles = providerFiles
												});
											}
										}
										catalogBrand.NameAndFolder = brand.NameAndFolder;
										catalogBrand.IconPath = brand.IconPath;
										catalogBrand.IconPath2 = brand.IconPath2;
										catalogBrand.IconPathImg = brand.IconPathImg;
										catalogBrand.IconPath2Img = brand.IconPath2Img;
										catalogBrand.Top = (brand.Top ?? 0);
										catalogBrand.Left = (brand.Left ?? 0);
										catalogBrand.Width = (brand.Width ?? 0);
										catalogBrand.Height = (brand.Height ?? 0);
										catalogBrand.BrandId = brand.BrandId;
										catalogBrand.ButtonStyle = brand.ButtonStyle;
										catalogBrand.MenuWindow = brand.MenuWindow.Value;
										catalogGroupBox.Brands.Add(catalogBrand);
									}
								}
								catalogGroupBox.Left = groupBox.Left.Value;
								catalogGroupBox.Top = groupBox.Top.Value;
								catalogGroupBox.Width = groupBox.Width.Value;
								catalogGroupBox.Height = groupBox.Height.Value;
								catalogGroupBox.Title = groupBox.Title;
								catalogGroupBox.VisibleBorder = groupBox.VisibleBorder.Value;
								catalogGroup.GroupBoxs.Add(catalogGroupBox);
							}
						}
						catalogGroup.Name = group.Name;
						catalogGroup.Width = group.Width.Value;
						catalogGroup.Height = group.Height.Value;
						catalogGroup.Order = (group.Order ?? 0);
						catalog.Groups.Add(catalogGroup);
					}
				}
				buffer = StringZip.Zip(new JavaScriptSerializer
				{
					MaxJsonLength = 2147483647
				}.Serialize(catalog));
			}
			return buffer;
		}

		private static void SetupServicePointManager()
		{
			ServicePointManager.DefaultConnectionLimit = 100;
			ServicePointManager.CheckCertificateRevocationList = false;
			ServicePointManager.UseNagleAlgorithm = false;
		}
	}
}
