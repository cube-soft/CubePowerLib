/* ------------------------------------------------------------------------- */
///
/// TohokuClient.cs
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
    /// TohokuClient
    /// 
    /// <summary>
    /// 東北電力から電力情報を取得するためのクラスです。
    /// </summary>
    /// 
    /// <seealso cref="http://setsuden.tohoku-epco.co.jp/graph.html"/>
    ///
    /* --------------------------------------------------------------------- */
    public class TohokuClient : Client
    {
        #region Initialization and Termination

        /* ----------------------------------------------------------------- */
        ///
        /// TohokuClient (constructor)
        /// 
        /// <summary>
        /// 既定の値でオブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public TohokuClient() : base(Area.Tohoku) { }

        #endregion

        #region Override methods

        private bool today = true;

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
            if (time == DateTime.Today) return "http://setsuden.tohoku-epco.co.jp/common/demand/juyo_tohoku.csv";

            today = false;
            if (time >= new DateTime(2008,4,1)) return String.Format("http://setsuden.tohoku-epco.co.jp/common/demand/juyo_{0}_tohoku.csv", time.Year);
            
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

                if (today)
                {
                    // 該当日の電力最大供給量(3行目)
                    for (int line = 1; line <= 2; ++line) sr.ReadLine();
                    if (!GetCapacity(sr, response)) return null;

                    // 現在の電力消費量、取得した情報の取得時刻(45行目以降)
                    for (int line = 4; line <= 44; ++line) sr.ReadLine();
                    if (!GetUsage(sr, response)) return null;
                    return response;
                }
                else
                {
                    for (int line = 1; line <= 3; ++line) sr.ReadLine();
                    return CsvUnityCase(sr, response) ? response : null;
                }
            }
        }

        #endregion
    }
}
