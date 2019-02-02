﻿using System.IO;
using Terraria;
using Newtonsoft.Json;
using System;
using HamstarHelpers.Helpers.DebugHelpers;


namespace HamstarHelpers.Components.Config {
	public partial class JsonConfig<T> : JsonConfig where T : class {
		public string GetPathOnly() {
			if( this.PathName != "" ) {
				return Main.SavePath + Path.DirectorySeparatorChar + this.PathName;
			}
			return Main.SavePath;
		}
		public string GetFullPath() {
			return this.GetPathOnly() + Path.DirectorySeparatorChar + this.FileName;
		}

		public void SetFilePath( string filename, string pathname ) {
			this.FileName = filename;
			this.PathName = pathname;
		}


		public bool FileExists() {
			lock( JsonConfig.MyFileLock ) {
				return File.Exists( this.GetFullPath() );
			}
		}

		////////////////

		public bool LoadFile() {
			string path = this.GetFullPath();
			string json;
			bool success = true;

			lock( JsonConfig.MyFileLock ) {
				if( !File.Exists( path ) ) {
					success = false;
				}
			}

			if( success ) {
				using( StreamReader r = new StreamReader( path ) ) {
					lock( JsonConfig.MyFileLock ) {
						json = r.ReadToEnd();
					}
					this.DeserializeMe( json, out success );
				}
			}

			if( this.Data is ConfigurationDataBase ) {
				var data = (object)this.Data;
				var configData = (ConfigurationDataBase)data;
				configData.OnLoad( success );
			}

			return success;
		}

		public void SaveFile() {
			string path = this.GetFullPath();
			string json = this.SerializeMe();

			lock( JsonConfig.MyFileLock ) {
				File.WriteAllText( path, json );
			}

			if( this.Data is ConfigurationDataBase ) {
				var data = (object)this.Data;
				var configData = (ConfigurationDataBase)data;
				configData.OnSave();
			}
		}

		public bool DestroyFile() {
			string path = this.GetFullPath();

			lock( JsonConfig.MyFileLock ) {
				if( !File.Exists( path ) ) { return false; }

				File.Delete( path );
			}

			return true;
		}
	}
}