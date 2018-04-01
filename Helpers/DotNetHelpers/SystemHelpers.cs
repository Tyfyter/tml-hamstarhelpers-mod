﻿using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace HamstarHelpers.Helpers.DotNetHelpers {
	public static class SystemHelpers {
		public static long TimeStampInSeconds() {
			return (DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond) / 1000L;
		}


		/// <summary>
		/// Converts the given decimal number to the numeral system with the
		/// specified radix (in the range [2, 36]).
		/// </summary>
		/// <param name="number">The number to convert.</param>
		/// <param name="radix">The radix of the destination numeral system (in the range [2, 36]).</param>
		/// <returns></returns>
		public static string ConvertDecimalToRadix( long number, int radix ) {
			const int BitsInLong = 64;
			const string Digits = "0123456789abcdefghijklmnopqrstuvwxyz";

			if( radix < 2 || radix > Digits.Length ) {
				throw new ArgumentException( "The radix must be >= 2 and <= " + Digits.Length.ToString() );
			}

			if( number == 0 ) {
				return "0";
			}

			int index = BitsInLong - 1;
			long curr_num = Math.Abs( number );
			char[] chars = new char[ BitsInLong ];

			while( curr_num != 0 ) {
				int remainder = (int)( curr_num % radix );
				chars[index--] = Digits[remainder];
				curr_num = curr_num / radix;
			}

			string result = new String( chars, index + 1, BitsInLong - index - 1 );
			if( number < 0 ) {
				result = "-" + result;
			}

			return result;
		}


		public static string ComputeSHA256Hash( string str ) {
			var crypt = new SHA256Managed();
			byte[] crypto = crypt.ComputeHash( Encoding.ASCII.GetBytes( str ) );
			string hash = Convert.ToBase64String( crypto );
			
			return hash;
		}
		

		public static void OpenUrl( string url ) {
			try {
				Process.Start( url );
			} catch {
				try {
					//if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
					//else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
					//else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
					url = url.Replace( "&", "^&" );
					Process.Start( new ProcessStartInfo( "cmd", "/c start "+url ) { CreateNoWindow = true } );
				} catch( Exception _ ) {
					try {
						Process.Start( "xdg-open", url );
					} catch( Exception __ ) {
						Process.Start( "open", url );
					}
				}
			}
		}
	}
}
