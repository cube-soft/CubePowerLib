/* ------------------------------------------------------------------------- */
///
/// Client.cs
///
/// Copyright (c) 2013 CubeSoft, Inc. All rights reserved.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU General Public License as published by
/// the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU General Public License for more details.
///
/// You should have received a copy of the GNU General Public License
/// along with this program.  If not, see < http://www.gnu.org/licenses/ >.
///
/* ------------------------------------------------------------------------- */
using System;
using System.Diagnostics;

namespace CubePower.Monitoring
{
    /* --------------------------------------------------------------------- */
    ///
    /// Client
    /// 
    /// <summary>
    /// サーバへ電力情報を問い合わせるためのクライアントの基底クラスです。
    /// </summary>
    /// 
    /// <remarks>
    /// このクラスを直接インスタンス化する事はできません。必要な実装を行った
    /// 各サブクラスをインスタンス化して下さい。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public class Client
    {
        #region Initialization and Termination

        /* ----------------------------------------------------------------- */
        ///
        /// Client (constructor)
        /// 
        /// <summary>
        /// 引数に指定された地域を利用して、オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected Client(Area area)
        {
            _area = area;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Area
        /// 
        /// <summary>
        /// オブジェクトが電力情報の取得対象としている地域を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Area Area
        {
            get { return _area; }
        }

        #endregion

        #region Public methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetResponse
        /// 
        /// <summary>
        /// 引数に指定された日時に対応する電力情報を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Response GetResponse(DateTime time)
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) return null;

            var request = System.Net.WebRequest.Create(GetUrl(time));
            request.Proxy = null;

            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                return GetResponse(stream, time);
            }
        }

        #endregion

        #region Need to implement in inherited classes

        /* ----------------------------------------------------------------- */
        ///
        /// GetUrl
        /// 
        /// <summary>
        /// 電力情報を取得するための URL を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual string GetUrl(DateTime time)
        {
            throw new NotImplementedException();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetResponse
        /// 
        /// <summary>
        /// 引数に指定されたストリームから問い合わせのあった日時に対応する
        /// レスポンスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual Response GetResponse(System.IO.Stream stream, DateTime time)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Other virtual methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetCapacity
        /// 
        /// <summary>
        /// ストリームからピーク時供給力を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual bool GetCapacity(System.IO.StreamReader reader, Response dest)
        {
            try
            {
                var fields = reader.ReadLine().Split(',');
                dest.Capacity = (int)double.Parse(fields[0]);
                return true;
            }
            catch (Exception err)
            {
                Trace.TraceError(err.ToString());
                return false;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetUsage
        /// 
        /// <summary>
        /// ストリームから要求された時刻の消費電力量を取得します。
        /// Response.Time プロパティは、消費電力量を取得できた時刻で上書き
        /// されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual bool GetUsage(System.IO.StreamReader reader, Response dest)
        {
            var target = dest.Time;
            var found = false;

            for (var line = reader.ReadLine(); !string.IsNullOrEmpty(line); line = reader.ReadLine())
            {
                var fields = line.Split(','); // DATE,TIME,USAGE
                if (fields.Length < 3) continue;
                try
                {
                    var time = DateTime.ParseExact(fields[0] + ',' + fields[1],
                        "yyyy'/'M'/'d','H':'mm",
                        System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    if (time > target) break;
                    dest.Time = time;
                    dest.Usage = (int)double.Parse(fields[2]);
                    found = true;
                }
                catch (FormatException /* err */) { /* フォーマットの不一致は無視 */ }
                catch (Exception err)
                {
                    Trace.TraceError(err.ToString());
                    return false;
                }
            }

            return found;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CsvUnityCase
        /// 
        /// <summary>
        /// ピーク時電力が別記されていない場合に利用します。
        /// ストリームから要求された時刻の消費電力量を取得します。
        /// Response.Time プロパティは、消費電力量を取得できた時刻で上書き
        /// されます。
        /// また、ピーク時の供給電力量も取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual bool CsvUnityCase(System.IO.StreamReader reader, Response dest)
        {
            var target = dest.Time;
            var found = false;
            var range_begin = new DateTime(target.Year, target.Month, target.Day);
            var range_end = range_begin.AddDays(1);

            for (var line = reader.ReadLine(); !string.IsNullOrEmpty(line); line = reader.ReadLine())
            {
                var fields = line.Split(','); // DATE,TIME,USAGE
                if (fields.Length != 3) continue;
                try
                {
                    var time = DateTime.ParseExact(fields[0] + ',' + fields[1],
                        "yyyy'/'M'/'d','H':'mm",
                        System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    if (range_begin <= target && target < range_end)
                    {
                        int elect = (int)double.Parse(fields[2]);
                        if (dest.Capacity < elect) dest.Capacity = elect;
                        if (time <= target)
                        {
                            dest.Time = time;
                            dest.Usage = elect;
                            found = true;
                        }
                    }
                }
                catch (FormatException /* err */) { /* フォーマットの不一致は無視 */ }
                catch (Exception err)
                {
                    Trace.TraceError(err.ToString());
                    return false;
                }
            }
            return found;            
        }

        #endregion

        #region Variables
        private Area _area;
        #endregion
    }
}
