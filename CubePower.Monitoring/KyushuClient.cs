﻿/* ------------------------------------------------------------------------- */
///
/// KyushuClient.cs
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
using System.IO;

namespace CubePower.Monitoring
{
    /* --------------------------------------------------------------------- */
    ///
    /// KyushuClient
    /// 
    /// <summary>
    /// 九州電力から電力情報を取得するためのクラスです。
    /// </summary>
    /// 
    /// <seealso cref="http://www.kyuden.co.jp/power_usages/pc.html"/>
    ///
    /* --------------------------------------------------------------------- */
    public class KyushuClient : Client
    {
        #region Initialization and Termination

        /* ----------------------------------------------------------------- */
        ///
        /// KansaiClient (constructor)
        /// 
        /// <summary>
        /// 既定の値でオブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public KyushuClient() : base(Area.Kyushu) { }

        #endregion

        #region Override methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetUrl
        /// 
        /// <summary>
        /// 電力情報を取得するための URL を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override string GetUrl(DateTime time)
        {
            var format = "http://www.kyuden.co.jp/power_usages/csv/electric_power_usage{0}.csv";
            if (time >= DateTime.Today) return String.Format(format, time.ToString("yyyyMMdd"));

            _today = false;
            format = "http://www.kyuden.co.jp/power_usages/csv/juyo-hourly-{0}.csv";
            if (time >= new DateTime(2012, 6, 29)) return String.Format(format, time.ToString("yyyyMMdd"));

            return null;
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
        protected override Response GetResponse(System.IO.Stream stream, DateTime time)
        {
            if (stream == null) throw new NullReferenceException();

            using (var sr = new StreamReader(stream))
            {
                var response = new Response();
                response.Area = this.Area;
                response.Unit = "万kW";
                response.Time = time;
                response.Usage = 0;

                // 該当日の電力最大供給量(3行目)
                for (int line = 1; line <= 2; ++line) sr.ReadLine();
                if (!GetCapacity(sr, response)) return null;

                // 現在の電力消費量、取得した情報の取得時刻
                var skipline = _today ? 8 : 44;
                for (int line = 4; line <= skipline; ++line) sr.ReadLine();
                return GetUsage(sr, response) ? response : null;
            }
        }

        #endregion

        #region Variables
        private bool _today = true;
        #endregion
    }
}
