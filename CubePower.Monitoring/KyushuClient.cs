/* ------------------------------------------------------------------------- */
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
            var base_uri = "http://www.kyuden.co.jp/power_usages/csv/electric_power_usage{0}.csv";
            return string.Format(base_uri, time.ToString("yyyymmdd"));
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
            // TODO: implementation
            throw new NotImplementedException();
        }

        #endregion
    }
}
